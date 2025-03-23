using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetWebApiWithDbContext.Domain;

public class User:BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
