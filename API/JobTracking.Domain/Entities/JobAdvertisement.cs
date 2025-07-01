using System;
using System.Collections.Generic;

namespace JobTracking.Domain.Entities
{
    public class JobAdvertisement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsActive { get; set; } // Статус: активна или неактивна

       
        public string Location { get; set; }
        public string Requirements { get; set; }
        public string Responsibilities { get; set; }
        public string Benefits { get; set; }

        
        public ICollection<Application> Applications { get; set; }
    }
}
