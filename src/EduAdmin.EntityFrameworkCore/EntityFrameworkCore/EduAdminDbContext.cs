using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using EduAdmin.Authorization.Roles;
using EduAdmin.Authorization.Users;
using EduAdmin.MultiTenancy;
using EduAdmin.Entities;

namespace EduAdmin.EntityFrameworkCore
{
    public class EduAdminDbContext : AbpZeroDbContext<Tenant, Role, User, EduAdminDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<FileManagement> FileManagement { get; set; } //文件管理
        public virtual DbSet<Teacher> Teacher { get; set; } //
        public virtual DbSet<Student> Student { get; set; } //
        public virtual DbSet<Course> Course { get; set; } //
        public virtual DbSet<CourseObjective> CourseObjectives { get; set; } //
        public virtual DbSet<ClassCourse> ClassCourse { get; set; } //
        public virtual DbSet<StudentCourse> StudentCourse { get; set; } //
        public virtual DbSet<StudentHomework> StudentHomework { get; set; } //
        public virtual DbSet<Classes> Classes { get; set; } //
        public virtual DbSet<Outline> Outline { get; set; } //
        public virtual DbSet<GraduationDesign> GraduationDesign { get; set; } //
        public virtual DbSet<GraduationRequirement> GraduationRequirement { get; set; } //
        public virtual DbSet<Homework> Homework { get; set; } //
        public virtual DbSet<Question> Question { get; set; } //
        public virtual DbSet<RoleInfo> RoleInfo { get; set; } //
        public virtual DbSet<ScoreAchievement> ScoreAchievement { get; set; } //
        public virtual DbSet<ScoreWeight> ScoreWeight { get; set; } //
        public virtual DbSet<Target> Target { get; set; } //
        public virtual DbSet<TestQuestion> TestQuestion { get; set; } //
        public virtual DbSet<AchievementTarget> AchievementTarget { get; set; } //
        public virtual DbSet<DesignObjective> DesignObjective { get; set; } //
        public virtual DbSet<TeachingTask> TeachingTask { get; set; } //
        public virtual DbSet<QuestionScore> QuestionScore { get; set; } //
        public virtual DbSet<DesignDefenseRecord> DesignDefenseRecord { get; set; } //答辩记录表
        public virtual DbSet<SwDetail> SwDetail { get; set; } //权重对应作业详情
        public virtual DbSet<CourseFile> CourseFile { get; set; }//课程的文件

        public EduAdminDbContext(DbContextOptions<EduAdminDbContext> options)
            : base(options)
        {
        }
    }
}
