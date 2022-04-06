using System;
using Microsoft.EntityFrameworkCore;

namespace Surveys.Model
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Response> Responses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Response>().HasKey(s => new { s.SurveyId, s.QuestionId});

            // Queries
            var queryMethod = typeof(ModelBuilder).GetMethod("Query", new Type[] { });
            Type type = null;

            type = typeof(SurveyDTO);
            queryMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });
            type = typeof(ResponseDTO);
            queryMethod.MakeGenericMethod(type).Invoke(modelBuilder, new object[] { });

            base.OnModelCreating(modelBuilder);
        }
    }
}
