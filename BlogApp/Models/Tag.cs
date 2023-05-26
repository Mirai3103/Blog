using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models;

public class Tag
{
    [Key]
    [Column(TypeName = "varchar(50)")]
    public string Slug { get; set; } =null!;
    [Column(TypeName = "varchar(100)")]
    public string Name { get; set; } =null!;
    public virtual ICollection<Post> Posts { get; set; }

    public Tag()
    {
        Posts = new HashSet<Post>();
    }
}