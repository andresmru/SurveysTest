using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surveys.Model
{
    [Table("Responses", Schema = "dbo")]
    public class Response
    {
        [Required]
        [Column(TypeName = "int")]
        public int SurveyId { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int QuestionId { get; set; }

        #nullable enable

        [Column(TypeName = "varchar(255)")]
        public string? TextResponse { get; set; }

        [Column(TypeName = "int")]
        public int? NumericResponse { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateResponse { get; set; }

        #nullable disable
    }
}
