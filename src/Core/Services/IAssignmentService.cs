using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IAssignmentService
    {
        IEnumerable<object> GetAll(string userId);
        object Get(int assignmentId);
        void Add(AssignmentDto assignment);
        void Update( int assignmentId, AssignmentDto assignment);
        void Delete(string userId ,int assignmentId);
        object GetSubmissions(string userId);
        object GetSubmission(int submissionId);
        void AddSubmission(string StudentId,SubmissionDto submission);
        void UpdateSubmission(int submissionId, SubmissionDto submission);
        void DeleteSubmission(int submissionId);
        void GradeSubmission(int submissionId, int grade);
    }
}
