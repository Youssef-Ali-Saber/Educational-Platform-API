using Core.Repositories;
using Core.UnitOFWork;
using Repository.Data;
using Repository.Repositories;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            courseRepository = new CourseRepository(_db);
            assignmentRepository = new AssignmentRepository(_db);
            submissionRepository = new SubmissionRepository(_db);
            lessonRepository = new LessonRepository(_db);
            chapterRepository = new ChapterRepository(_db);
            resourceRepository = new ResourceRepository(_db);
            paymentRepository = new PaymentRepository(_db);
        }
        public ICourseRepository courseRepository { get ; set ; }
        public IAssignmentRepository assignmentRepository { get ; set ; }
        public ISubmissionRepository submissionRepository { get ; set ; }
        public ILessonRepository lessonRepository { get ; set ; }
        public IChapterRepository chapterRepository { get ; set ; }
        public IResourceRepository resourceRepository { get ; set ; }
        public IPaymentRepository paymentRepository { get; set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
