using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELibraryManagement.Models
{
    public class Publisher
    {
        public string PublisherId { get; set; }
        public string PublisherName { get; set; }

        public Publisher( string publisherId , string publisherName)
        {
            this.PublisherId = publisherId;
            this.PublisherName = publisherName;
        }
    }
}