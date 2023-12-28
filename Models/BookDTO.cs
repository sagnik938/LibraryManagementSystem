using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELibraryManagement.Models
{
    public class BookDTO
    {
        public string ImagePath { get; set; }
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string Language { get; set; }
        public string PublisherName { get; set; }
        public string AuthorName { get; set; }
        public string Genre { get; set; }
        public string PublisherDate { get; set; }
        public string Edition { get; set; }
        public string CostPerUnit { get; set; }
        public string Pages { get; set; }
        public string ActualStock { get; set; }
        public string CurrentStock { get; set; }
        public string IssuedBooks { get; set; }
        public string BookDescription { get; set; }

        public string IssueDate { get; set; }
        public string DueDate { get; set; }
        public BookDTO()
        {

        }
        public BookDTO(string imagePath, string bookId, string bookName, string language, string publisherName ,string authorName, string genre, string publisherDate, string edition, string costPerUnit, string pages, string actualStock, string currentStock, string issuedBooks, string bookDescription)
        {
            ImagePath = imagePath;
            BookId = bookId;
            BookName = bookName;
            Language = language;
            PublisherName = publisherName;
            AuthorName = authorName;
            Genre = genre;
            PublisherDate = publisherDate;
            Edition = edition;
            CostPerUnit = costPerUnit;
            Pages = pages;
            ActualStock = actualStock;
            CurrentStock = currentStock;
            IssuedBooks = issuedBooks;
            BookDescription = bookDescription;
        }

        public BookDTO(string imagePath, string bookId, string bookName, string language, string publisherName, string authorName, string genre, string publisherDate, string edition, string costPerUnit, string pages, string actualStock, string bookDescription)
        {
            ImagePath = imagePath;
            BookId = bookId;
            BookName = bookName;
            Language = language;
            PublisherName = publisherName;
            AuthorName = authorName;
            Genre = genre;
            PublisherDate = publisherDate;
            Edition = edition;
            CostPerUnit = costPerUnit;
            Pages = pages;
            ActualStock = actualStock;
            BookDescription = bookDescription;
        }

    }


}