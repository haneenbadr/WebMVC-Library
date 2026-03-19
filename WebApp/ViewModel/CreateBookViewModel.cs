using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModel
{
    public class CreateBookViewModel
    {
        [Required(ErrorMessage = "must write the book title")]
        public string Title { get; set; }

        public int ISBN { get; set; }

        public double? Price { get; set; }

        [Display(Name = " must write the publish year")]
        public DateTime PublishYear { get; set; }

        [Display(Name = "must write the authorName")]
        public int AuthorID { get; set; }

        [Display(Name = "must write the category id")]
        public int CategoryID { get; set; }

 
        [Display(Name = "images")]
        public List<IFormFile>? Attachments { get; set; }
    }
}
