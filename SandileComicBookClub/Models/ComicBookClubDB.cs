using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SandileComicBookClub.Models
{
    public partial class ComicBookClubDB : DbContext
    {
        public ComicBookClubDB()
            : base("name=ComicBookClubDB")
        {
        }

        public virtual System.Data.Entity.DbSet<SandileComicBookClub.Models.BookTitle> BookTitles { get; set; }
        public virtual System.Data.Entity.DbSet<SandileComicBookClub.Models.BorrowedBook> BorrowedBooks { get; set; }
        public virtual System.Data.Entity.DbSet<SandileComicBookClub.Models.Category> Categories { get; set; }
        public virtual System.Data.Entity.DbSet<SandileComicBookClub.Models.Customer> Customers { get; set; }
        public virtual System.Data.Entity.DbSet<SandileComicBookClub.Models.Member> Members { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public System.Data.Entity.DbSet<SandileComicBookClub.Models.BorrowedBookVM> BorrowedBookVMs { get; set; }
    }
}
