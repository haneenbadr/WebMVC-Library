using EntityFramework2.Entities;

namespace WebApp.ViewModel
{
    public class EditBookViewModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public int ISBN { get; set; }
        public double? Price { get; set; }
        public DateTime PublishYear { get; set; }
        public int AuthorID { get; set; }
        public int CategoryID { get; set; }
 
        public List<BookAttachment>? ExistingAttachments { get; set; }

         public List<IFormFile>? NewAttachments { get; set; }
    }
}
