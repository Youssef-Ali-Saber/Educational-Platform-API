using Core.Dtos;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController(ICourseService courseService) : ControllerBase
    {

        [HttpPost("Add")]
        [Authorize(Roles = "Instructor")]
        public IActionResult AddCourse(CourseDto courseDto)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            courseService.Add(instructorId, courseDto);
            return Ok();
        }
        [HttpPut("Update/{courseId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult UpdateCourseInfo(int courseId , CourseDto courseDto)
        {
            courseService.Update(courseId , courseDto);
            return Ok();
        }
        [HttpGet("GetAll")]
        public IActionResult GetCourses(int pageNumber) 
        {
            var courses = courseService.GetAll();
            return Ok(courses);
        }

        [HttpGet("GetInstructorCourses")]
        [Authorize(Roles = "Instructor")]
        public IActionResult GetInstructorCourses()
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = courseService.GetInstructorCourses(instructorId);
            return Ok(courses);
        }
        [HttpGet("GetEnrolledCourses")]
        [Authorize(Roles = "Student")]
        public IActionResult GetEnrolledCourses()
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courses = courseService.GetEnrolledCourses(studentId);
            return Ok(courses);
        }

        [HttpGet("GetDetails/{courseId}")]
        public IActionResult GetCourseInfo(int courseId)
        {
            var course = courseService.Get(courseId);
            return Ok(course);
        }
        [HttpDelete("Delete/{courseId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult DeleteCourse(int courseId) 
        {
            courseService.Delete(courseId);
            return Ok();
        }
        [HttpPost("AddChapter")]
        [Authorize(Roles = "Instructor")]
        public IActionResult AddChapter(ChapterDto chapterDto)
        {
            courseService.AddChapter(chapterDto);
            return Ok();
        }
        [HttpPut("UpdateChapter/{chapterId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult UpdateChapterInfo(int chapterId, ChapterDto chapterDto)
        {
            courseService.UpdateChapter(chapterId, chapterDto);
            return Ok();
        }
        [HttpDelete("DeleteChapter/{chapterId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult DeleteChapter(int chapterId)
        {
            courseService.DeleteChapter(chapterId);
            return Ok();
        }
        [HttpPost("AddLesson")]
        [Authorize(Roles = "Instructor")]
        public IActionResult AddLesson(LessonDto lessonDto)
        {
            courseService.AddLesson(lessonDto);
            return Ok();
        }
        [HttpPut("UpdateLesson/{lessonId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult UpdateLessonInfo(int lessonId, LessonDto lessonDto)
        {
            courseService.UpdateLesson(lessonId, lessonDto);
            return Ok();
        }
        [HttpDelete("DeleteLesson/{lessonId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult DeleteLesson(int lessonId)
        {
            courseService.DeleteLesson(lessonId);
            return Ok();
        }
        [HttpGet("GetChapter/{chapterId}")]
        [Authorize]
        public IActionResult GetChapter(int chapterId)
        {
            var chapter = courseService.GetChapter(chapterId);
            return Ok(chapter);
        }
        [HttpGet("GetLesson/{lessonId}")]
        [Authorize]
        public IActionResult GetLesson(int lessonId)
        {
            var lesson = courseService.GetLesson(lessonId);
            return Ok(lesson);
        }
        [HttpGet("GetLessonsByChapter/{chapterId}")]
        [Authorize]
        public IActionResult GetAllLessons(int chapterId)
        {
            var lessons = courseService.GetAllLessons(chapterId);
            return Ok(lessons);
        }

        [HttpPost("UploadResource/{lessonId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult UploadResource(int lessonId,ResourceDto resourceDto)
        {
            courseService.UploadResource(lessonId, resourceDto);
            return Ok();
        }

        [HttpDelete("DeleteResource/{resourceId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult DeleteResource(int resourceId)
        {
            courseService.DeleteResource(resourceId);
            return Ok();
        }

        [HttpGet("GetResources/{lessonId}")]
        [Authorize]
        public IActionResult GetResources(int lessonId)
        {
            var resources = courseService.GetResources(lessonId);
            return Ok(resources);
        }

        [HttpPut("Enroll")]
        [Authorize(Roles = "Student")]
        public IActionResult Enroll(int courseId) 
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var url = courseService.Enroll(studentId, courseId);
            return Ok(url);
        }
        [HttpGet("enroll/success")]
        [Obsolete]
        public IActionResult EnrollSuccess(string sessionId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mass = courseService.EnrollSuccess(studentId, sessionId);
            return Ok(mass);
        }
        [HttpGet("enroll/failed")]
        [Obsolete]
        public IActionResult EnrollFailed(string sessionId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var mass = courseService.EnrollFailed(studentId, sessionId);
            return Ok(mass);
        }
    }
}
