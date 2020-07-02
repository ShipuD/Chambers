using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chambers.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        //1 Active; 0 Inactive 2 = Deleted
        
        public int Status { get; set; }
        [Required]
        public byte[] Data { get; set; }
        [Required]
        public int Order { get; set;}
        //public string Content { get; set; }
    }
}
