using Azure.Core;

namespace SelfServicePortal.Models
{
    public class Status
    {
        public int StatusId { get; set; }
        public string Name { get; set; } = "";
        public List<Request> Requests { get; set; } = new();
    }
}
