USE [master]
GO
/****** Object:  Database [Surveys]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE DATABASE [Surveys]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Surveys', FILENAME = N'D:\SQLServer\MSSSDB\Surveys.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Surveys_log', FILENAME = N'D:\SQLServer\MSSSDB\Surveys_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Surveys] SET COMPATIBILITY_LEVEL = 130
GO



USE [Surveys]
GO
/****** Object:  UserDefinedDataType [dbo].[Boolean]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[Boolean] FROM [bit] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[CreationDate]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[CreationDate] FROM [datetime] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[IntegerID]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[IntegerID] FROM [int] NULL
GO
/****** Object:  UserDefinedDataType [dbo].[IntegerID_Identity]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[IntegerID_Identity] FROM [int] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[NumberInteger]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[NumberInteger] FROM [int] NULL
GO
/****** Object:  UserDefinedDataType [dbo].[NumberTiny]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[NumberTiny] FROM [tinyint] NOT NULL
GO
/****** Object:  UserDefinedDataType [dbo].[ShortDate]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[ShortDate] FROM [date] NULL
GO
/****** Object:  UserDefinedDataType [dbo].[VC_100]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[VC_100] FROM [varchar](100) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[VC_20]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[VC_20] FROM [varchar](20) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[VC_255]    Script Date: 06/04/2022 01:00:19 p. m. ******/
CREATE TYPE [dbo].[VC_255] FROM [varchar](255) NULL
GO
/****** Object:  Table [dbo].[Questionnaires]    Script Date: 06/04/2022 01:00:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questionnaires](
	[QuestionnaireId] [dbo].[IntegerID_Identity] IDENTITY(1,1) NOT NULL,
	[QuestionnaireName] [dbo].[VC_100] NOT NULL,
	[Description] [dbo].[VC_255] NOT NULL,
	[CreationDate] [dbo].[CreationDate] NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_QUESTIONNAIRES] PRIMARY KEY CLUSTERED 
(
	[QuestionnaireId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 06/04/2022 01:00:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[QuestionId] [dbo].[IntegerID_Identity] IDENTITY(1,1) NOT NULL,
	[QuestionnaireId] [dbo].[IntegerID] NOT NULL,
	[QuestionCode] [dbo].[VC_20] NOT NULL,
	[QuestionText] [dbo].[VC_255] NOT NULL,
	[QuestionType] [dbo].[NumberTiny] NOT NULL,
	[IsRequired] [dbo].[Boolean] NOT NULL,
	[CreationDate] [dbo].[CreationDate] NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_QUESTIONS] PRIMARY KEY CLUSTERED 
(
	[QuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Responses]    Script Date: 06/04/2022 01:00:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Responses](
	[SurveyId] [int] NOT NULL,
	[QuestionId] [int] NOT NULL,
	[TextResponse] [dbo].[VC_255] NULL,
	[NumericResponse] [dbo].[NumberInteger] NULL,
	[DateResponse] [dbo].[ShortDate] NULL,
 CONSTRAINT [PK_Responses] PRIMARY KEY CLUSTERED 
(
	[SurveyId] ASC,
	[QuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Surveys]    Script Date: 06/04/2022 01:00:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Surveys](
	[SurveyId] [dbo].[IntegerID_Identity] IDENTITY(1,1) NOT NULL,
	[QuestionnaireId] [dbo].[IntegerID] NOT NULL,
	[CreationDate] [dbo].[CreationDate] NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_SURVEYS] PRIMARY KEY CLUSTERED 
(
	[SurveyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_QUESTION_REFERENCE_QUESTION] FOREIGN KEY([QuestionnaireId])
REFERENCES [dbo].[Questionnaires] ([QuestionnaireId])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_QUESTION_REFERENCE_QUESTION]
GO
ALTER TABLE [dbo].[Responses]  WITH CHECK ADD  CONSTRAINT [FK_RESPONSE_REFERENCE_QUESTION] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([QuestionId])
GO
ALTER TABLE [dbo].[Responses] CHECK CONSTRAINT [FK_RESPONSE_REFERENCE_QUESTION]
GO
ALTER TABLE [dbo].[Responses]  WITH CHECK ADD  CONSTRAINT [FK_RESPONSE_REFERENCE_SURVEYS] FOREIGN KEY([SurveyId])
REFERENCES [dbo].[Surveys] ([SurveyId])
GO
ALTER TABLE [dbo].[Responses] CHECK CONSTRAINT [FK_RESPONSE_REFERENCE_SURVEYS]
GO
ALTER TABLE [dbo].[Surveys]  WITH CHECK ADD  CONSTRAINT [FK_SURVEYS_REFERENCE_QUESTION] FOREIGN KEY([QuestionnaireId])
REFERENCES [dbo].[Questionnaires] ([QuestionnaireId])
GO
ALTER TABLE [dbo].[Surveys] CHECK CONSTRAINT [FK_SURVEYS_REFERENCE_QUESTION]
GO
/****** Object:  StoredProcedure [dbo].[SurveyResponses_Select]    Script Date: 06/04/2022 01:00:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SurveyResponses_Select]
	@QuestionnaireId INT,
	@SurveyId INT
AS 
BEGIN

SELECT  ISNULL(R.SurveyId, 0) SurveyId, 
		Q.QuestionnaireId, 
		Q.QuestionId, 
		Q.QuestionCode, 
		Q.QuestionText, 
		Q.QuestionType,
        Q.IsRequired,
		R.TextResponse, 
		R.DateResponse, 
		R.NumericResponse 
FROM    Questions Q
		LEFT JOIN Responses R ON Q.QuestionId = R.QuestionId
		LEFT JOIN Surveys S ON R.SurveyId = S.SurveyId AND S.StatusId < 255
WHERE  (( @SurveyId > 0 AND R.SurveyId = @SurveyId ) OR (Q.QuestionnaireId = @QuestionnaireId ))

END
GO
/****** Object:  StoredProcedure [dbo].[Surveys_Select]    Script Date: 06/04/2022 01:00:19 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Surveys_Select]
	@QuestionnaireId INT,
	@SurveyId INT,
	@IsNewSurvey BIT
AS 
BEGIN

IF @IsNewSurvey > 0
BEGIN
	SELECT  0 SurveyId, 
			QuestionnaireId, 
			QuestionnaireName
	FROM    Questionnaires
	WHERE  (QuestionnaireId = @QuestionnaireId )
END
ELSE
BEGIN
	SELECT  ISNULL(S.SurveyId, 0) SurveyId, 
			Q.QuestionnaireId, 
			Q.QuestionnaireName
	FROM    Surveys S 
			INNER JOIN Questionnaires Q ON S.QuestionnaireId = Q.QuestionnaireId
	WHERE  ( S.SurveyId = @SurveyId )
END

END
GO
USE [master]
GO
ALTER DATABASE [Surveys] SET  READ_WRITE 
GO
