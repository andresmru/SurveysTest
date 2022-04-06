using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Surveys.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Surveys.Controllers
{
    [Route("Questionnaires")]
    [ApiController]
    public class QuestionnairesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionnairesController(ApplicationDbContext context) => _dbContext = context;

        [HttpGet]
        public Response Get()
        {
            Response response = new Response { Success = false };

            try
            {
                response.Component = _dbContext.Questionnaires;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        [HttpGet("{questionnaireId}")]
        public Response Get(int questionnaireId)
        {
            Response response = new Response { Success = false };

            try
            {
                Questionnaire questionnaire = _dbContext.Questionnaires.Find(questionnaireId);
                if (questionnaire != null)
                {
                    questionnaire.Questions = _dbContext.Questions.Where(item => item.QuestionnaireId == questionnaireId && item.StatusId < 255).ToList();
                }

                response.Success = true;
                response.ResultCode = questionnaire.QuestionnaireId;
                response.Component = questionnaire;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public Response Post(Questionnaire questionnaire)
        {
            Response response = new Response { Success = false };

            if (questionnaire == null)
            {
                response.ErrorMessage = "Invalid request";
                return response;
            }

            using (IDbContextTransaction dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    questionnaire.StatusId = 10;
                    questionnaire.CreationDate = DateTime.Now;
                    _dbContext.Questionnaires.Add(questionnaire);
                    _dbContext.SaveChanges();

                    if (questionnaire.Questions != null && questionnaire.Questions.Count() > 0)
                    {
                        foreach (Question question in questionnaire.Questions)
                        {
                            question.QuestionnaireId = questionnaire.QuestionnaireId;
                            _dbContext.Questions.Add(question);
                        }
                    }
                    _dbContext.SaveChanges();

                    dbTransaction.Commit();

                    response = Get(questionnaire.QuestionnaireId);
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

        public Response Put(Questionnaire questionnaire)
        {
            Response response = new Response { Success = false };

            try
            {
                Questionnaire dbQuestionnaire = _dbContext.Questionnaires.Where(item => item.QuestionnaireId == questionnaire.QuestionnaireId && item.StatusId < 255).FirstOrDefault();
                if (dbQuestionnaire == null)
                {
                    response.ErrorMessage = $"QuestionnaireId {questionnaire.QuestionnaireId} not found";
                    return response;
                }

                using (IDbContextTransaction dbTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbQuestionnaire.QuestionnaireName = questionnaire.QuestionnaireName;
                        dbQuestionnaire.Description = questionnaire.Description;

                        _dbContext.SaveChanges();
                        dbTransaction.Commit();

                        response = Get(questionnaire.QuestionnaireId);
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
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        [HttpDelete("{questionnaireId}")]
        public Response Delete(int questionnaireId)
        {
            Response response = new Response { Success = false };

            try
            {
                Questionnaire dbQuestionnaire = _dbContext.Questionnaires.Where(item => item.QuestionnaireId == questionnaireId && item.StatusId < 255).FirstOrDefault();
                if (dbQuestionnaire == null)
                {
                    response.ErrorMessage = $"QuestionnaireId {questionnaireId} not found";
                    return response;
                }

                dbQuestionnaire.StatusId = 255;

                using (IDbContextTransaction dbTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.SaveChanges();
                        dbTransaction.Commit();

                        response.Success = true;
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
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

    }
}
