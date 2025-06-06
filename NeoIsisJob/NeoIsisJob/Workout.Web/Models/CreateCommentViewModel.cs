namespace ServerMVCProject.Models
{
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;

    public class CreateCommentViewModel
    {
        [Required]
        [MaxLength(250)]
        public string Content { get; set; }

        [Required]
        // Add HiddenInput to hide this field in form
        [HiddenInput(DisplayValue = false)]
        public long PostId { get; set; }
    }
}
