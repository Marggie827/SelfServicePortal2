using Microsoft.EntityFrameworkCore;
using SelfServicePortal.Data;
using SelfServicePortal.Models;

namespace SelfServicePortal.Services
{
    public class RequestService : IRequestService
    {
        private readonly AppDbContext _db;
        public RequestService(AppDbContext db) { _db = db; }

        public async Task<List<Request>> GetRequestsByUser(int userId)
        {
            return await _db.Requests
                .Include(r => r.Status)
                .Include(r => r.Category)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Request>> GetAllRequests()
        {
            return await _db.Requests
                .Include(r => r.Status)
                .Include(r => r.Category)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Request?> GetRequestById(int requestId)
        {
            return await _db.Requests
                .Include(r => r.Status)
                .Include(r => r.Category)
                .Include(r => r.User)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(r => r.RequestId == requestId);
        }

        public async Task<bool> CreateRequest(Request request)
        {
            try
            {
                request.StatusId = 1;
                request.CreatedAt = DateTime.Now;
                _db.Requests.Add(request);
                await _db.SaveChangesAsync();

                var admins = await _db.Users
                    .Where(u => u.Role == "Admin").ToListAsync();
                foreach (var admin in admins)
                {
                    _db.Notifications.Add(new Notification
                    {
                        UserId = admin.UserId,
                        RequestId = request.RequestId,
                        Message = $"New request: {request.Title}",
                        CreatedAt = DateTime.Now
                    });
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateStatus(int requestId,
            int newStatusId, int changedByUserId)
        {
            var request = await _db.Requests
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.RequestId == requestId);
            if (request == null) return false;

            string oldStatus = request.Status?.Name ?? "";
            var newStatus = await _db.Statuses.FindAsync(newStatusId);
            if (newStatus == null) return false;

            _db.AuditLogs.Add(new AuditLog
            {
                RequestId = requestId,
                OldStatus = oldStatus,
                NewStatus = newStatus.Name,
                ChangedBy = changedByUserId,
                ChangedAt = DateTime.Now
            });

            _db.Notifications.Add(new Notification
            {
                UserId = request.UserId,
                RequestId = requestId,
                Message = $"Your request '{request.Title}' changed to {newStatus.Name}",
                CreatedAt = DateTime.Now
            });

            request.StatusId = newStatusId;
            request.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignRequest(int requestId, int adminUserId)
        {
            var request = await _db.Requests.FindAsync(requestId);
            if (request == null) return false;
            request.AssignedTo = adminUserId;
            request.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelRequest(int requestId, int userId)
        {
            var request = await _db.Requests
                .FirstOrDefaultAsync(r => r.RequestId == requestId
                    && r.UserId == userId);
            if (request == null || request.StatusId != 1) return false;
            request.StatusId = 4;
            request.UpdatedAt = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task AddComment(Comment comment)
        {
            comment.CreatedAt = DateTime.Now;
            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<List<Status>> GetStatuses()
        {
            return await _db.Statuses.ToListAsync();
        }
    }
}