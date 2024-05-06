using AutoMapper;
using Core.Dtos;
using Core.Entities;

namespace Core.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Chapter, ChapterDto>().ReverseMap();
            CreateMap<Lesson, LessonDto>().ReverseMap();
            //CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<Submission, SubmissionDto>().ReverseMap();
            CreateMap<Assignment, AssignmentDto>().ReverseMap();
            CreateMap<Resource, ResourceDto>().ReverseMap();
        }


    }
}
