namespace SandileComicBookClub.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            Customers = new HashSet<Customer>();
        }
        
        [Key]
        public int MemberID { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "The Username field must be from 4 to 12 characters long", MinimumLength = 4)]
        public string Username { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The Password field must be at least 6 characters long", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [StringLength(25)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Cell Phone")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string CellPhone { get; set; }

        public bool Administrator { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
