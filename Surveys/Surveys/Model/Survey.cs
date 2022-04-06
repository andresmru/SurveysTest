using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surveys.Model
{
    [Table("Surveys", Schema = "dbo")]
    public class Survey : MainEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SurveyId { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int QuestionnaireId { get; set; }

        public List<Response> Responses { get; set; }

    }
}
