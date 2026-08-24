using EQuickKYC.Domain.Entities;

namespace EQuickKYC.Domain.Models
{
    public class UserPaginationResponseInfraDTO
    {
        public List<User> Users { get; set; } = new();

        public int Page { get; set; }
        public int PageSize { get; set; }

        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
