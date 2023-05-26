using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models;
// Id int
// DisplayName string
// Email string
// HashPassword string
// Avatar string
// Description string
public class User
{
    public enum Role
    {
        ADMIN,
        CREATOR,
        USER
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public string DisplayName { get; set; } =null!;
    [Column(TypeName = "varchar(100)")]
    public string Email { get; set; } =null!;
    [Column(TypeName = "varchar(100)")]
    public string HashPassword { get; set; } =null!;

    [Column(TypeName = "varchar(255)")] public string Avatar { get; set; } = "";

    [Column(TypeName = "nvarchar(255)")] public string Description { get; set; } = "";
    public virtual ICollection<Post> Posts { get; set; } 
    public virtual ICollection<Comment> Comments { get; set; }
    public Role UserRole { get; set; } = Role.USER;
    

    public User()
    {
        Posts = new HashSet<Post>();
        Comments = new HashSet<Comment>();
    }
}