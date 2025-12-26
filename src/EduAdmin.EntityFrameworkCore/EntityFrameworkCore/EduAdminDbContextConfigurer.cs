using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EduAdmin.EntityFrameworkCore
{
    public static class EduAdminDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<EduAdminDbContext> builder, string connectionString)
        {
            builder.UseMySql(connectionString,new MySqlServerVersion(new System.Version(8,0,19)));
        }

        public static void Configure(DbContextOptionsBuilder<EduAdminDbContext> builder, DbConnection connection)
        {
            builder.UseMySql(connection, new MySqlServerVersion(new System.Version(8, 0, 19)));
        }
    }
}
