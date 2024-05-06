using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Services;
using Core.UnitOFWork;
using Microsoft.AspNetCore.Http;
using Repository.Data;
using StackExchange.Redis;
using Stripe.Checkout;
using System.Text.Json;

namespace Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IDatabase _database;
        public CourseService(AppDbContext db, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _database = redis.GetDatabase();
        }
        public void Add(string instructorId, CourseDto course)
        {
            var courseEntity = _mapper.Map<Course>(course);
            courseEntity.InstructorId = instructorId;
            _unitOfWork.courseRepository.Add(courseEntity);
            _unitOfWork.Save();
        }

        public void AddChapter(ChapterDto chapter)
        {
            var chapterEntity = _mapper.Map<Chapter>(chapter);
            _unitOfWork.chapterRepository.Add(chapterEntity);
            _unitOfWork.Save();

        }

        public void AddLesson(LessonDto lesson)
        {
            var lessonEntity = _mapper.Map<Lesson>(lesson);
            _unitOfWork.lessonRepository.Add(lessonEntity);
            _unitOfWork.Save();
        }

        public void Delete(int courseId)
        {
            _unitOfWork.courseRepository.Delete(courseId);
            _unitOfWork.Save();
        }

        public void DeleteChapter(int chapterId)
        {
            _unitOfWork.chapterRepository.Delete(chapterId);
            _unitOfWork.Save();
        }

        public void DeleteLesson(int lessonId)
        {
            _unitOfWork.lessonRepository.Delete(lessonId);
            _unitOfWork.Save();
        }



        public object Get(int courseId)
        {
            return _unitOfWork.courseRepository.Get(m => m.Id == courseId).Select(s => new
            {
                title = s.Title,
                description = s.Description,
                price = s.Price,
                teacher = _db.Users?.Where(M => M.Id == s.InstructorId).Select(D => new
                {
                    name = D.UserName,
                    email = D.Email
                }
                ).FirstOrDefault(),
                chapters = _db.Chapters.Where(c => c.CourseId == s.Id).Select(s => new
                {
                    id = s.Id,
                    title = s.Title,
                    description = s.Description
                })
            });
        }

        public IEnumerable<object> GetAll()
        {
            var courses = _database.StringGet("Courses");
            if (!courses.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<IEnumerable<object>>(courses);
            }
            var courses0 = _unitOfWork.courseRepository.GetAll()
                .Select(s => new
                {
                    id = s.Id,
                    title = s.Title,
                    price = s.Price,
                    teacher = _db.Users?.FirstOrDefault(M => M.Id == s.InstructorId)?.FullName
                });
            _database.StringSet("Courses", JsonSerializer.Serialize(courses0));
            return courses0;
        }

        public IEnumerable<object> GetAllLessons(int chapterId)
        {
            var lessons = _database.StringGet($"Lessons{chapterId}");
            if (!lessons.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<IEnumerable<object>>(lessons);
            }
            var lessons0 = _unitOfWork.lessonRepository.Get(m => m.ChapterId == chapterId).Select(s => new
            {
                title = s.Title,
                content = s.Content
            });
            _database.StringSet($"Lessons{chapterId}", JsonSerializer.Serialize(lessons0));
            return lessons0;
        }

        public object GetChapter(int chapterId)
        {
            return _unitOfWork.chapterRepository.Get(m => m.Id == chapterId).Select(s => new
            {
                id = s.Id,
                title = s.Title,
                description = s.Description,
                Lessons = _db.Lessons.Where(l => l.ChapterId == s.Id).Select(s => new
                {
                    id = s.Id,
                    title = s.Title,
                    description = s.Description
                })
            });
        }

        public object GetLesson(int LessonId)
        {
            return _unitOfWork.lessonRepository.Get(m => m.Id == LessonId).Select(s => new
            {
                id = s.Id,
                title = s.Title,
                content = s.Content,
                description = s.Description,
                resources = _db.Resources.Where(r => r.LessonId == s.Id).Select(s => new
                {
                    id = s.Id,
                    title = s.Title,
                    description = s.Description,
                    fileLink = s.FileLink
                })
            });
        }

        public void Update(int courseId, CourseDto course)
        {
            var course0 = _unitOfWork.courseRepository.Get(m => m.Id == courseId).FirstOrDefault();
            if (course0 == null)
                return;
            var courseEntity = _mapper.Map<Course>(course);
            _unitOfWork.courseRepository.Update(courseEntity);
            _unitOfWork.Save();
        }

        public void UpdateChapter(int chapterId, ChapterDto chapter)
        {
            var chapter0 = _unitOfWork.chapterRepository.Get(m => m.Id == chapterId).FirstOrDefault();
            if (chapter0 == null)
                return;
            var chapterEntity = _mapper.Map<Chapter>(chapter);
            _unitOfWork.chapterRepository.Update(chapterEntity);
            _unitOfWork.Save();
        }

        public void UpdateLesson(int lessonId, LessonDto lesson)
        {
            var lesson0 = _unitOfWork.lessonRepository.Get(m => m.Id == lessonId).FirstOrDefault();
            if (lesson0 == null)
                return;
            var lessonEntity = _mapper.Map<Lesson>(lesson);
            _unitOfWork.lessonRepository.Update(lessonEntity);
            _unitOfWork.Save();
        }

        public void UploadResource(int lessonId, ResourceDto resource)
        {
            var resourceEntity = _mapper.Map<Resource>(resource);
            resourceEntity.LessonId = lessonId;
            var lesson = _unitOfWork.lessonRepository.Get(m => m.Id == lessonId).FirstOrDefault();
            resourceEntity.FileName = lesson?.Title + "_" + resourceEntity.Title + Path.GetExtension(resource.File.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resources", resourceEntity.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                resource.File.CopyTo(stream);
            }
            resourceEntity.FileLink = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Resources/{resourceEntity.FileName}";
            _unitOfWork.resourceRepository.Add(resourceEntity);
            _unitOfWork.Save();
        }

        public void DeleteResource(int resourceId)
        {
            var resource = _unitOfWork.resourceRepository.Get(m => m.Id == resourceId).FirstOrDefault();
            if (resource == null)
                return;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resources", Path.GetFileName(resource.FileName));
            if (File.Exists(path))
                File.Delete(path);
            _unitOfWork.resourceRepository.Delete(resourceId);
            _unitOfWork.Save();
        }

        public IEnumerable<object> GetResources(int lessonId)
        {
            var lesson = _database.StringGet($"Resources{lessonId}");
            if (!lesson.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<IEnumerable<object>>(lesson);
            }
            var resources = _unitOfWork.resourceRepository.Get(m => m.LessonId == lessonId).Select(s => new
            {
                title = s.Title,
                description = s.Description,
                fileLink = s.FileLink
            });
            _database.StringSet($"Resources{lessonId}", JsonSerializer.Serialize(resources));
            return resources;
        }

        public string Enroll(string studentId, int courseId)
        {
            var course = _unitOfWork.courseRepository.Get(m => m.Id == courseId).FirstOrDefault();
            if (course == null)
                return "Course Not Found";
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/api/courses/enroll/success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/api/courses/enroll/cancel?sessionId={{CHECKOUT_SESSION_ID}}",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)course.Price * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = course.Title
                            },
                        }
                    },
                },
                Mode = "payment",
            };
            var service = new SessionService();
            var session = service.Create(options);
            _unitOfWork.paymentRepository.Add(new Payment
            {
                CourseId = courseId,
                StudentId = studentId,
                SessionId = session.Id,
                PaymentIntentId = session.PaymentIntentId,
                PaymentStatus = session.PaymentStatus
            });
            _unitOfWork.Save();
            return session.Url;
        }

        public IEnumerable<object>? GetEnrolledCourses(string studentId)
        {
            var courses = _database.StringGet($"EnrolledCourses{studentId}");
            if (!courses.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<IEnumerable<object>>(courses);
            }
            var courses0 = _unitOfWork.paymentRepository.Get(m => m.StudentId == studentId).Select(s => new
            {
                id = s.CourseId,
                course = _db.Courses.Find(s.CourseId)?.Title,
                price = _db.Courses.Find(s.CourseId)?.Price,
                paymentStatus = s.PaymentStatus
            });
            _database.StringSet($"EnrolledCourses{studentId}", JsonSerializer.Serialize(courses0));
            return courses0;
        }

        public IEnumerable<object>? GetInstructorCourses(string insturctorId)
        {
            var courses = _database.StringGet($"InstructorCourses{insturctorId}");
            if (!courses.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<IEnumerable<object>>(courses);
            }
            var courses0 = _unitOfWork.courseRepository.Get(m => m.InstructorId == insturctorId).Select(s => new
            {
                id = s.Id,
                title = s.Title,
                price = s.Price,
                teacher = _db.Users?.Where(M => M.Id == s.InstructorId).Select(D => new
                {
                    name = D.FullName,
                    email = D.Email
                }
                               ).FirstOrDefault(),
                chapters = _db.Chapters.Where(c => c.CourseId == s.Id).Select(s => new
                {
                    id = s.Id,
                    title = s.Title,
                    description = s.Description
                })
            });
            _database.StringSet($"InstructorCourses{insturctorId}", JsonSerializer.Serialize(courses0));
            return courses0;
            
        }

        public string EnrollSuccess(string studentId, string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);
            var payment = _unitOfWork.paymentRepository.Get(m => m.SessionId == sessionId).FirstOrDefault();
            if (payment == null)
                return "Error: Payment information not found. Please contact support.";
            payment.PaymentStatus = session.PaymentStatus;
            payment.PaymentIntentId = session.PaymentIntentId;
            payment.PaymentStatus = session.PaymentStatus;
            _unitOfWork.Save();
            return "Enroll Success";
        }

        public string EnrollFailed(string studentId, string sessionId)
        {
            var payment = _unitOfWork.paymentRepository.Get(m => m.SessionId == sessionId).FirstOrDefault();
            if (payment == null)
                return "Error: Payment information not found. Please contact support.";
            _unitOfWork.paymentRepository.Delete(payment.Id);
            _unitOfWork.Save();
            return "Enroll Failed";
        }

    }

}
