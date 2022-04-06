using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Surveys.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Surveys.Controllers
{
    [Route("Questionnaires/{questionnaireId}/Questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionsController(ApplicationDbContext context) => _dbContext = context;

        [HttpGet()]
        public Response Get(int questionnaireId)
        {
            Response response = new Response { Success = false };
            IEnumerable<Question> questions;

            try
            {
                questions = _dbContext.Questions.Where(item => item.QuestionnaireId == questionnaireId && item.StatusId < 255).ToList();

                response.Success = true;
                response.Component = questions;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        [HttpGet("{questionId}")]
        public Response Get(int questionnaireId, int questionId)
        {
            Response response = new Response { Success = false };
            Question question;

            try
            {
                question = _dbContext.Questions.Where(item => item.QuestionId == questionId && item.StatusId < 255).FirstOrDefault();

                if (question != null && question.QuestionnaireId != questionnaireId)
                {
                    response.ErrorMessage = "Question does not belongs to questionnaire";
                }
                else
                {
                    response.Success = true;
                    response.Component = question;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        [HttpPost]
        public Response Post(Question question)
        {
            Response response = new Response { Success = false };

            if (question == null)
            {
                response.ErrorMessage = "Invalid request: null reference";
                return response;
            }

            if (question.QuestionnaireId == 0)
            {
                response.ErrorMessage = "Invalid request: QuestionnaireId not valid";
                return response;
            }

            Question dbQuestion = _dbContext.Questions.Where(item => item.QuestionnaireId == question.QuestionnaireId && item.QuestionCode == question.QuestionCode && item.StatusId < 255).FirstOrDefault();
            if (dbQuestion != null)
            {
                response.ErrorMessage = "Invalid request: QuestionCode exists in current questionnaire";
                return response;
            }

            if (question.QuestionType != (byte)eQuestionTypes.Text && question.QuestionType != (byte)eQuestionTypes.Numeric && question.QuestionType != (byte)eQuestionTypes.Date)
            {
                response.ErrorMessage = "Invalid request: Invalid question type";
                return response;
            }

            using (IDbContextTransaction dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    question.StatusId = 10;
                    question.CreationDate = DateTime.Now;
                    _dbContext.Questions.Add(question);
                    _dbContext.SaveChanges();

                    dbTransaction.Commit();

                    response.ResultCode = question.QuestionId;
                    response = Get(question.QuestionnaireId, question.QuestionId);
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

        [HttpPut()]
        public Response Put(Question question)
        {
            Response response = new Response { Success = false };

            try
            {
                Question dbQuestion = _dbContext.Questions.Where(item => item.QuestionId == question.QuestionId && item.StatusId < 255).FirstOrDefault();
                if (dbQuestion == null)
                {
                    response.ErrorMessage = $"QuestionId {question.QuestionId} not found";
                    return response;
                }

                dbQuestion = _dbContext.Questions.Where(item => item.QuestionId != question.QuestionId && item.QuestionCode == question.QuestionCode && item.StatusId < 255).FirstOrDefault();
                if (dbQuestion != null)
                {
                    response.ErrorMessage = "Invalid request: QuestionCode exists in current questionnaire";
                    return response;
                }

                dbQuestion = _dbContext.Questions.Where(item => item.QuestionId == question.QuestionId && item.StatusId < 255).FirstOrDefault();

                if (question.QuestionType != (byte)eQuestionTypes.Text && question.QuestionType != (byte)eQuestionTypes.Numeric && question.QuestionType != (byte)eQuestionTypes.Date)
                {
                    response.ErrorMessage = "Invalid request: Invalid question type";
                    return response;
                }

                using (IDbContextTransaction dbTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbQuestion.QuestionCode = question.QuestionCode;
                        dbQuestion.QuestionText = question.QuestionText;
                        dbQuestion.QuestionType = question.QuestionType;
                        dbQuestion.IsRequired = question.IsRequired;

                        _dbContext.SaveChanges();
                        dbTransaction.Commit();

                        response = Get(question.QuestionnaireId, question.QuestionId);
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

        [HttpDelete("{questionId}")]
        public Response Delete(int questionId)
        {
            Response response = new Response { Success = false };

            try
            {
                Question dbQuestion = _dbContext.Questions.Where(item => item.QuestionId == questionId && item.StatusId < 255).FirstOrDefault();
                if (dbQuestion == null)
                {
                    response.ErrorMessage = $"QuestionId {questionId} not found";
                    return response;
                }

                dbQuestion.StatusId = 255;

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
