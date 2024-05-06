using AutoMapper;
using Core.Dtos;
using Core.Services;
using Core.UnitOFWork;
using Core.Entities;
using Repository.Data;

namespace Services
{
    public class AssignmentService(IUnitOfWork unitOFWork, AppDbContext db, IMapper mapper) : IAssignmentService
    {
        public void Add(AssignmentDto assignment)
        {
            var assignmentEntity = mapper.Map<Assignment>(assignment);
            unitOFWork.assignmentRepository.Add(assignmentEntity);
            unitOFWork.Save();
        }

        public void Delete(string userId, int assignmentId)
        {

            unitOFWork.assignmentRepository.Delete(assignmentId);
            unitOFWork.Save();
        }

        public object Get(int assignmentId)
        {
            return unitOFWork.assignmentRepository.Get(m => m.Id == assignmentId).Select(s => new
            {
                dateOfCreation = s.DateOfCreation,
                dateOfLastModification = s.DateOfLastModification,
                deadline = s.Deadline,
                subject = s.Subject,
                content = s.Content
            });
        }

        public IEnumerable<object> GetAll(string userId)
        {
            return unitOFWork.assignmentRepository.Get(m => m.Course.AppUsers.FirstOrDefault(m => m.Id == userId).Id == userId || m.Course.InstructorId == userId).Select(s => new
            {
                id = s.Id,
                dateOfLastModification = s.DateOfLastModification,
                deadline = s.Deadline,
                courseTitle = s.Course.Title
            });
        }


        public void Update(int assignmentId, AssignmentDto assignment)
        {
            var assignment0 = unitOFWork.assignmentRepository.Get(m => m.Id == assignmentId).FirstOrDefault();
            if (assignment0 == null)
                return;
            var assignmentEntity = mapper.Map<Assignment>(assignment);
            unitOFWork.assignmentRepository.Update(assignmentEntity);
            unitOFWork.Save();
        }

        public void AddSubmission(string studentId, SubmissionDto submission)
        {
            var submissionEntity = mapper.Map<Submission>(submission);
            submissionEntity.StudentId = studentId;
            unitOFWork.submissionRepository.Add(submissionEntity);
            unitOFWork.Save();
        }
        public void UpdateSubmission(int submissionId, SubmissionDto submission)
        {
            var submission0 = unitOFWork.submissionRepository.Get(m => m.Id == submissionId).FirstOrDefault();
            if (submission0 == null)
                return;
            var submissionEntity = mapper.Map<Submission>(submission);
            unitOFWork.submissionRepository.Update(submissionEntity);
            unitOFWork.Save();
        }

        public object GetSubmissions(string userId)
        {
            return unitOFWork.submissionRepository.Get(m => m.StudentId == userId || m.Assignment.Course.InstructorId == userId).Select(s => new
            {
                id = s.Id,
                subject = s.Assignment.Subject,
                courseTitle = s.Assignment.Course.Title
            });
        }

        public void DeleteSubmission(int submissionId)
        {
            unitOFWork.submissionRepository.Delete(submissionId);
            unitOFWork.Save();
        }

        public object GetSubmission(int submissionId)
        {
            return unitOFWork.submissionRepository.Get(m => m.AssignmentId == submissionId).Select(s => new
            {
                id = s.Id,
                subject = s.Assignment.Subject,
                courseTitle = s.Assignment.Course.Title,
                dateOfSubmission = s.DateOfSubmission,
                content = s.Content,
                link = s.Link,
                grade = s.Grade
            });
        }
        public void GradeSubmission(int submissionId, int grade)
        {
            var submission = unitOFWork.submissionRepository.Get(m => m.Id == submissionId).FirstOrDefault();
            if (submission == null)
                return;
            submission.Grade = grade;
            unitOFWork.Save();
        }
    }
}
