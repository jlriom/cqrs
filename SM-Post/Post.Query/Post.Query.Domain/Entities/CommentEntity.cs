using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("Comment")]
public class CommentEntity
{
    [Key]
    public Guid CommentId {get; set;}
    public required string Username {get; set;}
    public required DateTime CommentDate {get; set;}
    public required string Comment {get; set;}
    public bool Edited {get; set;}
    public Guid PostId { get; set;}

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual PostEntity Post {get; set;}
}
