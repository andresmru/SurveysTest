using System;

namespace Surveys.Model
{
    public class ResponseDTO
    {
        public int SurveyId { get; set; }

        public int QuestionnaireId { get; set; }

        public int QuestionId { get; set; }

        public string QuestionCode { get; set; }

        public string QuestionText { get; set; }

        public byte QuestionType { get; set; }

        public bool IsRequired { get; set; }

        #nullable enable
        public string? TextResponse { get; set; }

        public int? NumericResponse { get; set; }

        public DateTime? DateResponse { get; set; }
        #nullable disable
    }
}
