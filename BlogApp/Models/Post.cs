using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models;
// Id int
// AuthorId int [ref: > User.Id]
// Title string
// Slug string
// Summary string
// Published boolean
// CreatedAt datetime
// UpdatedAt datetime
// Content string
// ThumbnailUrl string
public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int AuthorId { get; set; }
    [Column(TypeName = "nvarchar(100)")]
    public string Title { get; set; } =null!;
    [Column(TypeName = "varchar(100)")]
    public string Slug { get; set; } =null!;
    [Column(TypeName = "nvarchar(500)")]
    public string Summary { get; set; } =null!;
    public bool Published { get; set; }
    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [Column(TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    [Column(TypeName = "text")]
    public string Content { get; set; } =null!;
    [Column(TypeName = "varchar(255)")]
    public string ThumbnailUrl { get; set; } =null!;
    public virtual ICollection<Tag> Tags { get; set; } 
    public virtual ICollection<Comment> Comments { get; set; }
    [ForeignKey("AuthorId")]
    public virtual User Author { get; set; }

    public Post()
    {
        Tags = new HashSet<Tag>();
        Comments = new HashSet<Comment>();
    }
}