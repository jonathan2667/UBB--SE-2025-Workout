namespace ServerLibraryProject.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Comments")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }
      
        [Column("user_id")]
        required public int UserId { get; set; }

        [Column("post_id")]
        required public long PostId { get; set; }

        [Column("content")]
        required public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("created_date")]
        required public DateTime CreatedDate { get; set; }
    }
}
