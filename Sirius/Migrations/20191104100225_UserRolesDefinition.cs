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
    public partial class UserRolesDefinition : Migration
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
                Role adminRole = context.Roles.Where(role => role.Id == DefaultValues.Roles.Admin.Id).FirstOrDefault();
                //User admin = context.Users.Where(user => user.FirstName == "Администратор").FirstOrDefault();
                var users = context.Users.ToArray();
                foreach (var user in users)
                {
                    user.Role = adminRole;
                    context.Users.Update(user);
                }
                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
