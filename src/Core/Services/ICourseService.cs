using Core.Dtos;
using Core.Entities;
using System.Linq.Expressions;

namespace Core.Services
{
    public interface ICourseService
    {
        IEnumerable<object> GetAll();
        object Get(int courseId);
        void Add(string instructorId, CourseDto course);
        void Update(int courseId, CourseDto course);
        void Delete(int courseId);
        object GetChapter(int chapterId);
        void AddChapter(ChapterDto chapter);
        void UpdateChapter(int chapterId, ChapterDto chapter);
        void DeleteChapter(int chapterId);
        IEnumerable<object> GetAllLessons(int chapterId);
        object GetLesson(int LessonId);
        void AddLesson(LessonDto lesson);
        void UpdateLesson(int lessonId, LessonDto lesson);
        void DeleteLesson(int lessonId);
        void UploadResource(int lessonId, ResourceDto resource);
        void DeleteResource(int resourceId);
        IEnumerable<object> GetResources(int lessonId);
        string Enroll(string studentId, int courseId);
        IEnumerable<object>? GetEnrolledCourses(string studentId);
        IEnumerable<object>? GetInstructorCourses(string insturctorId);
        string EnrollSuccess(string studentId, string sessionId);
        string EnrollFailed(string studentId, string sessionId);

    }
}
