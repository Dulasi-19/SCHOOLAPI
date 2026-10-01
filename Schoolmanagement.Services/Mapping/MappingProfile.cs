using AutoMapper;
using Schoolmanagement.Services.ViewModel.Announcement;
using Schoolmanagement.Services.ViewModel.Assignment;
using Schoolmanagement.Services.ViewModel.AssignmentSubmission;
using Schoolmanagement.Services.ViewModel.Class;
using Schoolmanagement.Services.ViewModel.Exam;
using Schoolmanagement.Services.ViewModel.HomeWork;
using Schoolmanagement.Services.ViewModel.Notification;
using Schoolmanagement.Services.ViewModel.Result;
using Schoolmanagement.Services.ViewModel.Role;
using Schoolmanagement.Services.ViewModel.Student;
using Schoolmanagement.Services.ViewModel.Teacher;
using Schoolmanagement.Services.ViewModel.Timetable;
using Schoolmanagement.Services.ViewModel.User;
using Schoolmangenment.Entities.Model;

namespace Schoolmanagement.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Class Mappings
            CreateMap<SchoolClass, ClassVm>();
            CreateMap<ClassRequest, SchoolClass>();

            // Student Mappings
            CreateMap<Student, StudentVm>();
            CreateMap<StudentRequest, Student>();

            // Teacher Mappings
            CreateMap<Teacher, TeacherVm>();
            CreateMap<TeacherRequest, Teacher>();

            // User Mappings
            CreateMap<User, UserVm>();
            CreateMap<UserRequest, User>();

            // Role Mappings
            CreateMap<Role, RoleVm>();
            CreateMap<RoleRequest, Role>();

            // HomeWork Mappings
            CreateMap<HomeWork, HomeWorkVm>();
            CreateMap<HomeWorkRequest, HomeWork>();

            // Announcement Mappings
            CreateMap<Announcement, AnnouncementVm>();
            CreateMap<AnnouncementRequest, Announcement>();

            // Assignment Mappings
            CreateMap<Assignment, AssignmentVm>();
            CreateMap<AssignmentRequest, Assignment>();

            // AssignmentSubmission Mappings
            CreateMap<AssignmentSubmission, AssignmentSubmissionVm>();
            CreateMap<AssignmentSubmissionRequest, AssignmentSubmission>();

            // Exam Mappings
            CreateMap<Exam, ExamVm>();
            CreateMap<ExamRequest, Exam>();

            // Notification Mappings
            CreateMap<Notification, NotificationVm>();
            CreateMap<NotificationRequest, Notification>();

            // Result Mappings
            CreateMap<Result, ResultVm>();
            CreateMap<ResultRequest, Result>();

            // Timetable Mappings
            CreateMap<Timetable, TimetableVm>();
            CreateMap<TimetableRequest, Timetable>();
        }
    }
}
