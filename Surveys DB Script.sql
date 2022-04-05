
/*==============================================================*/
/* Domain: Boolean                                              */
/*==============================================================*/
create type Boolean
   from bit not null
go

/*==============================================================*/
/* Domain: CreationDate                                         */
/*==============================================================*/
create type CreationDate
   from datetime not null
go

/*==============================================================*/
/* Domain: Date                                                 */
/*==============================================================*/
create type ShortDate
   from date
go

/*==============================================================*/
/* Domain: IntegerID                                            */
/*==============================================================*/
create type IntegerID
   from int
go

/*==============================================================*/
/* Domain: IntegerID_Identity                                   */
/*==============================================================*/
create type IntegerID_Identity
   from int not null
go

/*==============================================================*/
/* Domain: NumberInteger                                        */
/*==============================================================*/
create type NumberInteger
   from int
go

/*==============================================================*/
/* Domain: NumberTiny                                           */
/*==============================================================*/
create type NumberTiny
   from tinyint not null
go

execute sp_bindrule R_NumberTiny, NumberTiny
go

/*==============================================================*/
/* Domain: VC_100                                               */
/*==============================================================*/
create type VC_100
   from varchar(100)
go

/*==============================================================*/
/* Domain: VC_20                                                */
/*==============================================================*/
create type VC_20
   from varchar(20)
go

/*==============================================================*/
/* Domain: VC_255                                               */
/*==============================================================*/
create type VC_255
   from varchar(255)
go

/*==============================================================*/
/* Table: Questionnaires                                        */
/*==============================================================*/
create table Questionnaires (
   QuestionnaireId      IntegerID_Identity   identity,
   QuestionnaireName    VC_100               not null,
   [Description]          VC_255               not null,
   CreationDate         CreationDate         not null,
   constraint PK_QUESTIONNAIRES primary key (QuestionnaireId)
)
go

/*==============================================================*/
/* Table: Questions                                             */
/*==============================================================*/
create table Questions (
   QuestionId           IntegerID_Identity   identity,
   QuestionnaireId      IntegerID            not null,
   QuestionCode         VC_20                not null,
   QuestionText         VC_255               not null,
   QuestionType         NumberTiny           not null,
   IsRequired           Boolean              not null,
   constraint PK_QUESTIONS primary key (QuestionId)
)
go

/*==============================================================*/
/* Table: Responses                                             */
/*==============================================================*/
create table Responses (
   SurveyId             int                  not null,
   QuestionId           int                  not null,
   TextResponse         VC_255               null,
   NumericResponse      NumberInteger        null,
   DateResponse         ShortDate            null,
   constraint PK_RESPONSES primary key (SurveyId, QuestionId)
)
go

/*==============================================================*/
/* Table: Surveys                                               */
/*==============================================================*/
create table Surveys (
   SurveyId             IntegerID_Identity   identity,
   QuestionnaireId      IntegerID            not null,
   CreationDate         CreationDate         not null,
   constraint PK_SURVEYS primary key (SurveyId)
)
go

alter table Questions
   add constraint FK_QUESTION_REFERENCE_QUESTION foreign key (QuestionnaireId)
      references Questionnaires (QuestionnaireId)
go

alter table Responses
   add constraint FK_RESPONSE_REFERENCE_SURVEYS foreign key (SurveyId)
      references Surveys (SurveyId)
go

alter table Responses
   add constraint FK_RESPONSE_REFERENCE_QUESTION foreign key (QuestionId)
      references Questions (QuestionId)
go

alter table Surveys
   add constraint FK_SURVEYS_REFERENCE_QUESTION foreign key (QuestionnaireId)
      references Questionnaires (QuestionnaireId)
go
