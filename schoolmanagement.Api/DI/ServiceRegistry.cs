using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Schoolmangenment.Repository.Interfaces;
using Schoolmangenment.Repository;
using Schoolmanagement.Services.Mapping;
using Schoolmanagement.Services.Interface;
using Schoolmanagement.Services.Service;
using Microsoft.OpenApi.Models;
using Asp.Versioning;

namespace schoolmanagement.Api.DI
{
    public static class ServiceRegistry
    {
        public static void RegisteredServices(this IServiceCollection service, IConfiguration configuration)
        {
            // 1. Configure and Register Controllers
            service.AddControllers(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json", "text/plain", "text/json"));
            }).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // 2. Configure and Register AutoMapper
            service.AddAutoMapper(typeof(MappingProfile).Assembly);

            // 3. Register Repositories and Services
            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IRoleService, RoleService>();
            service.AddTransient<IStudentService, StudentService>();
            service.AddTransient<ITeacherService, TeacherService>();
            service.AddTransient<IClassService, ClassService>();
            service.AddTransient<IHomeWorkService, HomeWorkService>();
            service.AddTransient<IAnnouncementService, AnnouncementService>();
            service.AddTransient<IAssignmentService, AssignmentService>();
            service.AddTransient<IAssignmentSubmissionService, AssignmentSubmissionService>();
            service.AddTransient<IExamService, ExamService>();
            service.AddTransient<INotificationService, NotificationService>();
            service.AddTransient<IResultService, ResultService>();
            service.AddTransient<ITimetableService, TimetableService>();

            // 4. Register Swagger Gen & API Versioning
            service.AddEndpointsApiExplorer();
            
            service.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "School Management API - V1", Version = "v1.0" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your JWT token."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public static void RegisterCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(optionBuilder =>
            {
                var corsOrigins = "http://localhost:5173";
                if (corsOrigins != null)
                {
                    optionBuilder.AddPolicy(name: "CorsPolicy_UISettings", policy =>
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                    );
                }
            });
        }
    }
}
