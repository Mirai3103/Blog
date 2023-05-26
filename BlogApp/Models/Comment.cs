using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int PostId { get; set; }
    public int AuthorId { get; set; }
    [Column(TypeName = "nvarchar(255)")]
    public string Content { get; set; } =null!;
    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [ForeignKey("PostId")]
    public virtual Post Post { get; set; }
    public int ParentId { get; set; }
    [ForeignKey("ParentId")]
    public virtual Comment Parent { get; set; } 
    
    public virtual ICollection<Comment> Children { get; set; }

    public Comment()
    {   
        Children = new HashSet<Comment>();
    }
}