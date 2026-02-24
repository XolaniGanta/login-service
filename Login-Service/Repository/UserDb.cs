using Login_Service.Entities;
using Microsoft.EntityFrameworkCore;

namespace Login_Service.Repository;

public class UserDb : DbContext
{
    public UserDb(DbContextOptions<UserDb> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
}