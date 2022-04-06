using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surveys.Model
{
    public class MainEntity
    {
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreationDate { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int StatusId { get; set; }
    }
}
