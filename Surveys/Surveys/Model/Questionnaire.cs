using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surveys.Model
{
    [Table("Questionnaires", Schema = "dbo")]
    public class Questionnaire : MainEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionnaireId { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string QuestionnaireName { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; }

        public List<Question> Questions { get; set; }

    }
}
