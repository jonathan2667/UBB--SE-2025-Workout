namespace ServerMVCProject.Models
{
    using System.ComponentModel.DataAnnotations;
    using ServerLibraryProject.Enums;

    public class CreatePostViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(250)]
        public string Content { get; set; }

        [Required]
        public PostVisibility Visibility { get; set; }

        [Required]
        public PostTag Tag { get; set; }

        [Required]
        public long GroupId { get; set; }
    }
}