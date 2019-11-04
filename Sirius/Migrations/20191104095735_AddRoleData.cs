using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Sirius.DAL;
using Sirius.Helpers;
using Sirius.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sirius.Migrations
{
    public partial class AddRoleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SiriusContext>();

            string connectionString = "Host=127.0.0.1;Port=5432;Database=siriusdb;Username=siriususer;Password=Uncle340571578;Integrated Security=false;";

            var options = optionsBuilder
                    .UseNpgsql(connectionString)
                    .Options;

            using (var context = new SiriusContext(options))
            {

                Role adminRole = new Role { Id = DefaultValues.Roles.Admin.Id, Name = "Администратор" };
                Role viewerRole = new Role { Id = DefaultValues.Roles.Viewer.Id, Name = "Просмотр" };
                context.Roles.AddRange(new Role[] { adminRole, viewerRole });
                /*
                User admin = context.Users.Where(user => user.FirstName == "Администратор").FirstOrDefault();
                if (admin != null)
                {
                    admin.Role = adminRole;
                    admin.RoleId = adminRole.Id;
                    context.Users.Update(admin);
                }*/
                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
