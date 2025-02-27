
namespace FinanceTracker.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string DefaultTransactionType { get; set; } = default!;

        public bool IsDeleted { get; set; } = default!;

        public string UserId { get; set; } = default!;

        public User User { get; set; } = default!;
    }
}
