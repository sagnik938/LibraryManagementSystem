using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELibraryManagement.Models
{
    public class Author
    {
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }

        public Author(string authorId , string authorName)
        {
            this.AuthorId = authorId;
            this.AuthorName = authorName;
        }
    }
}