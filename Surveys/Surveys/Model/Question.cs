using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surveys.Model
{
    public enum eQuestionTypes { Text = 1, Numeric = 2, Date = 3 };

    [Table("Questions", Schema = "dbo")]
    public class Question : MainEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int QuestionnaireId { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string QuestionCode { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string QuestionText { get; set; }

        [Required]
        [Column(TypeName = "tiny")]
        public byte QuestionType { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool IsRequired { get; set; }

    }
}
