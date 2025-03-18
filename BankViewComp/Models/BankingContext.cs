using Microsoft.EntityFrameworkCore;

namespace BankViewComp.Models
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options)
            : base(options)
        {
        }
        public DbSet<Types> Type { get; set; } = default!;
        public DbSet<Customer> Customer{ get; set; } = default!;
        public DbSet<Account> Account { get; set; } = default!;
    }
}
