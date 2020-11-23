using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LPA.Server.Models;
using LPA.Server;
using LPA.Server.Helpers;
using LPA.DTOs;
using Newtonsoft.Json;

namespace LPA.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestsController : ControllerBase
    {

        private readonly ILogger<TestsController> _logger;

        public TestsController(ILogger<TestsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Test> Get()
        {
            return null;
        }

        [HttpGet("GetAuthoredTestsForCourse")]
        public string GetAuthoredTestsForCourse(string login, string password, int courseId)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user?.IsTeacher != true)
                return null;
            var course = context.Courses.FirstOrDefault(c => c.Id == courseId);
            if (course?.AuthorId != user.Id)
                return null;
            var tests = context.Tests.Where(t => t.CourseId == courseId).Select(c => new TestDTO()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
            return tests.ToJson();
        }

        [HttpGet("GetTestsForCourse")]
        public string GetTestsForCourse(string login, string password, int courseId)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var tests = context.Tests.Where(t => t.CourseId == courseId).Select(t => new TestDTO() {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description
            }).ToList();
            var testSuccesses = context.StudentTestSuccesses.Where(sts => sts.UserId == user.Id).ToList();
            foreach (var test in tests)
            {
                if (testSuccesses.Any(ts => ts.TestId == test.Id))
                {
                    test.IsPassed = true;
                }
            }
            return tests.ToJson();
        }

        [HttpGet("FindTest")]
        public string FindTest(string login, string password, int id)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var test = context.Tests.FirstOrDefault(t => t.Id == id);
            if (test == null)
                return null;
            var testDTO = new TestDTO()
            {
                Id = test.Id,
                CourseId = test.CourseId,
                Description = test.Description,
                Name = test.Name
            };
            testDTO.Questions = context.TestQuestions.Where(tq => tq.TestId == id).Select(tq => new TestQuestionDTO()
            { 
                Id = tq.Id, 
                Answers = new List<TestQuestionAnswerDTO>(),
                AreMultipleAnswersRight=tq.AreMultipleAnswersRight,
                IsOpen = tq.IsOpen,
                TestId = tq.TestId,
                Text = tq.Text
            }).ToList();
            var ids = testDTO.Questions.Select(q => q.Id).ToList();
            var testQuestionsAnswers = context.TestQuestionAnswers.Where(tqa => ids.Any(i => i == tqa.QuestionId)).ToList();
            foreach (var tqa in testQuestionsAnswers)
            {
                var tq = testDTO.Questions.FirstOrDefault(tq => tq.Id == tqa.QuestionId);
                if (tq != null)
                {
                    tq.Answers.Add(new TestQuestionAnswerDTO()
                    {
                        Id = tqa.Id,
                        IsRight = tqa.IsRight,
                        QuestionId = tqa.QuestionId,
                        Text = tqa.Text
                    });
                }
            }
            return testDTO.ToJson();
        }

        [HttpGet("FindQuestion")]
        public string FindQuestion(string login, string password, int id)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var question = context.TestQuestions.FirstOrDefault(tq => tq.Id == id);
            if (question == null)
                return null;
            var questionDTO = new TestQuestionDTO()
            {
                Id = question.Id,
                Answers = new List<TestQuestionAnswerDTO>(),
                AreMultipleAnswersRight = question.AreMultipleAnswersRight,
                IsOpen = question.IsOpen,
                TestId = question.TestId,
                Text = question.Text
            };
            var testQuestionsAnswers = context.TestQuestionAnswers.Where(tqa => tqa.QuestionId == question.Id);
            foreach (var tqa in testQuestionsAnswers)
            {
                {
                    questionDTO.Answers.Add(new TestQuestionAnswerDTO()
                    {
                        Id = tqa.Id,
                        IsRight = tqa.IsRight,
                        QuestionId = tqa.QuestionId,
                        Text = tqa.Text
                    });
                }
            }
            return questionDTO.ToJson();
        }

        [HttpGet("FindAnswer")]
        public string FindAnswer(string login, string password, int id)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var answer = context.TestQuestionAnswers.FirstOrDefault(tqa => tqa.Id == id);
            if (answer == null)
                return null;
            var answerDTO = new TestQuestionAnswerDTO()
            {
                Id = answer.Id,
                IsRight = answer.IsRight,
                Text = answer.Text
            };
            return answerDTO.ToJson();
        }

        [HttpGet("SaveTest")]
        public string SaveTest(string login, string password, string testJson)
        {
            var testDTO = JsonConvert.DeserializeObject<TestDTO>(testJson);
            if (testDTO == null)
                return null;
            using MyContext context = new MyContext();
            try
            {
                var user = AuthHelper.FindUser(login, password, context);
                if (user?.IsTeacher != true)
                    return null;
                var test = context.Tests.FirstOrDefault(tq => tq.Id == testDTO.Id);
                if (test == null)
                {
                    test = new Test() { CourseId = testDTO.CourseId };
                    context.Tests.Add(test);
                }
                test.Name = testDTO.Name;
                test.Description = testDTO.Description;
                context.SaveChanges();
                testDTO.Id = test.Id;
                return testDTO.ToJson();
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("SaveQuestion")]
        public string SaveQuestion(string login, string password, string questionJson)
        {
            var questionDTO = JsonConvert.DeserializeObject<TestQuestionDTO>(questionJson);
            if (questionDTO == null)
                return null;
            using MyContext context = new MyContext();
            try
            {
                var user = AuthHelper.FindUser(login, password, context);
                if (user?.IsTeacher != true)
                    return null;
                var question = context.TestQuestions.FirstOrDefault(tq => tq.Id == questionDTO.Id);
                if (question == null)
                {
                    question = new TestQuestion() { TestId = questionDTO.TestId };
                    context.TestQuestions.Add(question);
                }
                question.IsOpen = questionDTO.IsOpen;
                question.Text = questionDTO.Text;
                question.AreMultipleAnswersRight = questionDTO.AreMultipleAnswersRight;
                context.SaveChanges();
                questionDTO.Id = question.Id;
                return questionDTO.ToJson();
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("SaveAnswer")]
        public string SaveAnswer(string login, string password, string answerJson)
        {
            var answerDTO = JsonConvert.DeserializeObject<TestQuestionAnswerDTO>(answerJson);
            if (answerDTO == null)
                return null;
            using MyContext context = new MyContext();
            try
            {
                var user = AuthHelper.FindUser(login, password, context);
                if (user?.IsTeacher != true)
                    return null;
                var answer = context.TestQuestionAnswers.FirstOrDefault(tqa => tqa.Id == answerDTO.Id);
                if (answer == null)
                {
                    answer = new TestQuestionAnswer() { QuestionId = answerDTO.QuestionId };
                    context.TestQuestionAnswers.Add(answer);
                }
                answer.Text = answerDTO.Text;
                answer.IsRight = answerDTO.IsRight;
                context.SaveChanges();
                answerDTO.Id = answer.Id;
                return answerDTO.ToJson();
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("DeleteTest")]
        public ActionResult DeleteTest(string login, string password, int id)
        {
            try
            {
                using MyContext context = new MyContext();

                var user = AuthHelper.FindUser(login, password, context);
                if (user?.IsTeacher != true)
                    return null;

                var test = context.Tests.FirstOrDefault(t => t.Id == id && t.Course.AuthorId == user.Id);
                if (test == null)
                    return null;
                var questions = context.TestQuestions.Where(tq => tq.TestId == id).ToList();
                var qids = questions.Select(q => q.Id).ToList();
                var answers = context.TestQuestionAnswers.Where(tqa => qids.Any(qid => qid == tqa.QuestionId)).ToList();
                foreach (var answer in answers)
                {
                    context.TestQuestionAnswers.Remove(answer);
                }
                foreach (var question in questions)
                {
                    context.TestQuestions.Remove(question);
                }
                context.Remove(test);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("DeleteQuestion")]
        public ActionResult DeleteQuestion(string login, string password, int id)
        {
            try
            {
                using MyContext context = new MyContext();

                var user = AuthHelper.FindUser(login, password, context);
                if (user?.IsTeacher != true)
                    return null;

                var question = context.TestQuestions.FirstOrDefault(tq => tq.Id == id && tq.Test.Course.AuthorId == user.Id);
                if (question == null)
                    return null;
                var answers = context.TestQuestionAnswers.Where(tqa => tqa.QuestionId == question.Id).ToList();
                foreach (var answer in answers)
                {
                    context.TestQuestionAnswers.Remove(answer);
                }
                context.TestQuestions.Remove(question);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("DeleteAnswer")]
        public ActionResult DeleteAnswer(string login, string password, int id)
        {
            try
            {
                using MyContext context = new MyContext();

                var user = AuthHelper.FindUser(login, password, context);
                if (user?.IsTeacher != true)
                    return null;

                var answer = context.TestQuestionAnswers.FirstOrDefault(tqa => tqa.Id == id && tqa.Question.Test.Course.AuthorId == user.Id);
                if (answer == null)
                    return null;
                context.TestQuestionAnswers.Remove(answer);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("CheckTest")]
        public string CheckTest(string login, string password, string testJson)
        {
            var testDTO = testJson.FromJson<TestDTO>();
            if (testDTO?.Questions == null || testDTO.Questions.Any(q => q.Answers == null))
                return null;
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var answers = context.TestQuestionAnswers.Where(tqa => tqa.Question.TestId == testDTO.Id).ToList();
            var answerDTOs = new List<TestQuestionAnswerDTO>();
            foreach (var questionDTO in testDTO.Questions)
            {
                foreach (var answerDTO in questionDTO.Answers)
                {
                    answerDTOs.Add(answerDTO);
                }
            }
            if (answerDTOs.Count != answers.Count)
                return null;
            bool allRightChecked = true;
            bool anyWrongChecked = false;
            foreach (var answerDTO in answerDTOs)
            {
                var answer = answers.FirstOrDefault(a => a.Id == answerDTO.Id);
                if (answer == null)
                    return null;
                if (answer.IsRight)
                {
                    if (!answerDTO.IsChecked)
                        allRightChecked = false;
                }
                else
                {
                    if (answerDTO.IsChecked)
                        anyWrongChecked = true;
                }
            }
                testDTO.IsPassed = allRightChecked && !anyWrongChecked;
            if (testDTO.IsPassed == true)
            {
                var success = context.StudentTestSuccesses.FirstOrDefault(sts => sts.TestId == testDTO.Id && sts.UserId == user.Id);
                if (success == null)
                {
                    context.StudentTestSuccesses.Add(new StudentTestSuccess() { TestId = testDTO.Id, UserId = user.Id });
                    context.SaveChanges();
                }
            }
            return testDTO.ToJson();
        }
    }
}
