using FirstCoreApp.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstCoreApp.Models
{
    public class NewsContext : DbContext
    {
        public NewsContext()
        {

        }
        public NewsContext(DbContextOptions<NewsContext> options)
            : base(options)
        {
        }
        

        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<ContactUs> Contacts { get; set; }
        public DbSet<Teammember> Teammembers { get; set; }



    }

}
