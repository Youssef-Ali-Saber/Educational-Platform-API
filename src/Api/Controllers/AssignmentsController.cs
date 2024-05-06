using Core.Dtos;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController(IAssignmentService assignmentService) : ControllerBase
    {
        [HttpPost("Add")]
        [Authorize(Roles = "Instructor")]
        public IActionResult Add(AssignmentDto assignmentDto)
        {
            assignmentService.Add(assignmentDto);            
            return Ok();
        }
        [HttpDelete("Delete/{assignmentId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult Delete( int assignmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            assignmentService.Delete(userId,assignmentId);
            return Ok();
        }
        [HttpGet("Get/{assignmentId}")]
        [Authorize]
        public IActionResult Get(int assignmentId)
        {
            return Ok(assignmentService.Get(assignmentId));
        }
        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(assignmentService.GetAll(userId));
        }
        [HttpPut("Update/{assignmentId}")]
        [Authorize(Roles = "Instructor")]
        public IActionResult Update(int assignmentId, AssignmentDto assignmentDto)
        {
            assignmentService.Update(assignmentId, assignmentDto);
            return Ok();
        }
        [HttpPost("AddSubmission")]
        [Authorize(Roles ="Student")]
        public IActionResult AddSubmission(SubmissionDto submissionDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            assignmentService.AddSubmission(userId, submissionDto);
            return Ok();
        }
        [HttpGet("GetAllSubmissions")]
        [Authorize]
        public IActionResult GetSubmissions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(assignmentService.GetSubmissions(userId));
        }
        [HttpGet("GetSubmission/{submissionId}")]
        [Authorize]
        public IActionResult GetSubmission(int submissionId)
        {
            return Ok(assignmentService.GetSubmission(submissionId));
        }
        [HttpPut("UpdateSubmission/{submissionId}")]
        [Authorize(Roles ="Student")]
        public IActionResult UpdateSubmission(int submissionId, SubmissionDto submissionDto)
        {
            assignmentService.UpdateSubmission(submissionId, submissionDto);
            return Ok();
        }
        [HttpDelete("DeleteSubmission/{submissionId}")]
        [Authorize(Roles ="Student")]
        public IActionResult DeleteSubmission(int submissionId)
        {
            assignmentService.DeleteSubmission(submissionId);
            return Ok();
        }
        [HttpPost("GradeSubmission")]
        [Authorize(Roles = "Instructor")]
        public IActionResult GradeSubmission(int submissionId, int grade)
        {
            assignmentService.GradeSubmission(submissionId, grade);
            return Ok();
        }

    }
}
