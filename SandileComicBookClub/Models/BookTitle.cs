namespace SandileComicBookClub.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BookTitle
    {
     
        public BookTitle()
        {
            BorrowedBooks = new HashSet<BorrowedBook>();
        }

        [Key]
        public int TitleID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Series { get; set; }

        [Required]
        public string Author { get; set; }

        [Column("Date Published", TypeName = "date")]
        public DateTime Date_Published { get; set; }

        [Column("Books Available")]
        public int Books_Available { get; set; }

        public int CategoryID { get; set; }

        public string BookBoxArtURL { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
