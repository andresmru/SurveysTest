using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Surveys.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Surveys.Controllers
{
    [Route("Questionnaires/{questionnaireId}/Surveys")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public SurveysController(ApplicationDbContext context) => _dbContext = context;

        [HttpGet]
        public Response GetSurveys(int questionnaireId)
        {
            Response response = new Response { Success = false };

            try
            {
                IEnumerable<SurveyDTO> surveys = _dbContext.Set<SurveyDTO>().FromSqlRaw("exec dbo.Surveys_Select @QuestionnaireId={0}, @SurveyId=0, @IsNewSurvey=0", questionnaireId);

                response.Component = surveys;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
        [HttpGet("{surveyId}")]
        public Response GetSurvey(int surveyId)
        {
            Response response = new Response { Success = false };

            try
            {
                SurveyDTO survey = _dbContext.Set<SurveyDTO>().FromSqlRaw("exec dbo.Surveys_Select @QuestionnaireId=0, @SurveyId={0}, @IsNewSurvey=0", surveyId).ToList().FirstOrDefault();

                if (survey != null)
                {
                    survey.Responses = _dbContext.Set<ResponseDTO>().FromSqlRaw("exec dbo.SurveyResponses_Select @QuestionnaireId=0, @SurveyId={0}", surveyId).ToList();
                }

                response.Component = survey;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
        [Route("ApplySurvey")]
        [HttpGet]
        public Response GetApplySurvey(int questionnaireId)
        {
            Response response = new Response { Success = false };

            try
            {
                SurveyDTO surveyToApply = _dbContext.Set<SurveyDTO>().FromSqlRaw("exec dbo.Surveys_Select @QuestionnaireId={0}, @SurveyId=0, @IsNewSurvey=1", questionnaireId).ToList().FirstOrDefault();
                surveyToApply.Responses = _dbContext.Set<ResponseDTO>().FromSqlRaw("exec dbo.SurveyResponses_Select @QuestionnaireId={0}, @SurveyId=0", questionnaireId).ToList();
                
                response.Component = surveyToApply;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public Response Post(int questionnaireId, Survey survey)
        {
            Response response = new Response { Success = false };

            if (survey == null)
            {
                response.ErrorMessage = "Invalid request";
                return response;
            }

            using (IDbContextTransaction dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    survey.QuestionnaireId = questionnaireId;
                    survey.StatusId = 10;
                    survey.CreationDate = DateTime.Now;
                    _dbContext.Surveys.Add(survey);
                    _dbContext.SaveChanges();

                    if (survey.Responses != null && survey.Responses.Count() > 0)
                    {
                        foreach(Model.Response surveyResponse in survey.Responses)
                        {
                            surveyResponse.SurveyId = survey.SurveyId;
                            _dbContext.Responses.Add(surveyResponse);
                            _dbContext.SaveChanges();
                        }
                    }

                    //_dbContext.SaveChanges();

                    dbTransaction.Commit();

                    response = GetSurvey(survey.SurveyId);
                }
                catch (DbUpdateException ex)
                {
                    response.ErrorMessage = ex.Message;

                    dbTransaction.Rollback();
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = ex.Message;

                    dbTransaction.Rollback();
                }
            }

            return response;
        }

    }
}
