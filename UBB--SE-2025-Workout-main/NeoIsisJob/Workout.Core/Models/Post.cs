namespace ServerLibraryProject.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;
    using ServerLibraryProject.Enums;

    [Table("Posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("title")]
        required public string Title { get; set; }

        [Column("content")]
        required public string Content { get; set; }

        [Column("created_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        required public DateTime CreatedDate { get; set; }

        [Column("user_id")]
        required public int UserId { get; set; }

        [Column("group_id")]
        public long? GroupId { get; set; }

        [Column("visibility")]
        required public PostVisibility Visibility { get; set; }

        [Column("tag")]
        required public PostTag Tag { get; set; }

        public Post() { }

    }


}
