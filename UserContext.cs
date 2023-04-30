using Microsoft.EntityFrameworkCore;

namespace SessionFinalProject
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        }

        public DbSet<User> Users { get;set; }
        public DbSet<SignUpCode> SignUpCodes { get;set; }
        public DbSet<Session> Sessions { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }
    }

    public class SignUpCode
    {
        public int Id { get; set; }  
        public string Code { get; set; }
        public DateTime ExpiresOn { get; set;}
        public string Email { get; set; }
    }

    public class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string SessionId { get; set; }
        public DateTime ExpiresOn { get; set; }
    }


}