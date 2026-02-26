using System.ComponentModel.DataAnnotations.Schema;

namespace Login_Service.Entities;

public class User
{
    [Column("id")]
    public int Id { get; set; }
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("email")]
    public string Email { get; set; }
    [Column("password_hash")]
    public string PasswordHash { get; set; }
    
    [Column("registration_date")]
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}
