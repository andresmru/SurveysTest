using System;
using System.Collections.Generic;

namespace Surveys.Model
{
    public class SurveyDTO
    {
        public int SurveyId { get; set; }

        public int QuestionnaireId { get; set; }

        public string QuestionnaireName { get; set; }

        public List<ResponseDTO> Responses { get; set; }

    }
}
