using SelfServicePortal.Models;

namespace SelfServicePortal.Services
{
    public interface IRequestService
    {
        Task<List<Request>> GetRequestsByUser(int userId);
        Task<List<Request>> GetAllRequests();
        Task<Request?> GetRequestById(int requestId);
        Task<bool> CreateRequest(Request request);
        Task<bool> UpdateStatus(int requestId, int newStatusId, int changedByUserId);
        Task<bool> AssignRequest(int requestId, int adminUserId);
        Task<bool> CancelRequest(int requestId, int userId);
        Task AddComment(Comment comment);
        Task<List<Category>> GetCategories();
        Task<List<Status>> GetStatuses();
    }
}