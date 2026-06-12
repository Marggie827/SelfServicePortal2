using Azure.Core;

namespace SelfServicePortal.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<Request> Requests { get; set; } = new();
    }
}
