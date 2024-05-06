using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UnitOFWork
{
    public interface IUnitOfWork
    {
        public ICourseRepository courseRepository { get; set; }
        public IAssignmentRepository assignmentRepository { get; set; }
        public ISubmissionRepository submissionRepository { get; set; }
        public ILessonRepository lessonRepository { get; set; }
        public IChapterRepository chapterRepository { get; set; }
        public IResourceRepository resourceRepository { get; set; }
        public IPaymentRepository paymentRepository { get; set; }
        void Save();
    }
}
