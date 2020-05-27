using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace dotnetfundaGenericRepositoryEF.Models
{
    public class PostModel
    {
        public Int64 PostId { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [Display(Name = "Tags (Comma ',' Seperated)")]
        [Required(ErrorMessage = "Tags is/are required")]
        public string Tags { get; set; }

    }
}