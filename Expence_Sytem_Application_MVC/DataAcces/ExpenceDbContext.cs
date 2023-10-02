using Expence_Sytem_Application_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Expence_Sytem_Application_MVC.DataAcces
{
    public class ExpenceDbContext : DbContext
    {
        public ExpenceDbContext(DbContextOptions options) :base(options){}

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
