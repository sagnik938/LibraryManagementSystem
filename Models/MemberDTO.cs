using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELibraryManagement.Models
{
    public class MemberDTO
    {
        public string MemberId { get; set; }
        public string FullName { get; set; } 
        public string AccountStatus { get; set; }
        public string DOB { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string FullAddress { get; set; }

        public MemberDTO( string memberId , string fullName , string accountStatus , string dob , string contactNo , string emailId , string state , string city , string pincode , string fullAddress)
        {
            this.MemberId = memberId;
            this.FullName = fullName;
            this.AccountStatus = accountStatus;
            this.DOB = dob;
            this.ContactNo = contactNo;
            this.EmailId = emailId;
            this.State = state;
            this.City = city;
            this.Pincode = pincode;
            this.FullAddress = fullAddress;
        }

        public MemberDTO()
        {
        }
    }
}