namespace SandileComicBookClub.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web;

    public class BorrowedBookVM
    {
       
       
        [Key]
        [Display(Name = "Borrowing ID")]
        public int BorrowingID { get; set; }

        [Required]
        [Display(Name = "Borrow Date")]
        public string BorrowDate { get; set; }


        [Display(Name = "Book Title")]
        public int TitleID { get; set; }

        
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        
        public string Time { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Return Date")]
        public string ReturnDate { get; set; }
        
        public BookTitle BookTitle { get; set; }

        public Member Member { get; set; }


       
        
    }
}
