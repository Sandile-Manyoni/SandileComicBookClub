namespace SandileComicBookClub.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    public partial class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [ForeignKey ("BookBorrowing")]
        public int BorrowingID { get; set; }

        [Required]
        [ForeignKey("MemberInfo")]
        public int MemberID { get; set; }

        public virtual BorrowedBook BookBorrowing { get; set; }

        public virtual Member MemberInfo { get; set; }
    }
}
