using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELibraryManagement.Models
{
    public class BookIssueDTO
    {
        public string BookId { get; set; }
        public string MemberId { get; set; }
        public string BookName { get; set; }
        public string MemberName { get; set; }
        public string IssueDate { get; set; }
        public string DueDate { get; set; }

        public BookIssueDTO(string bookId, string memberId, string bookName, string memberName, string issueDate, string dueDate)
        {
            BookId = bookId;
            MemberId = memberId;
            BookName = bookName;
            MemberName = memberName;
            IssueDate = issueDate;
            DueDate = dueDate;
        }
        public BookIssueDTO()
        {
        }
    }
}