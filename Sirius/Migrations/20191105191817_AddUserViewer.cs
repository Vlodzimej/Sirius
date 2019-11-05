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
    public partial class AddUserViewer : Migration
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
                Role viewerRole = context.Roles.Where(role => role.Id == DefaultValues.Roles.Viewer.Id).FirstOrDefault();

                var startDate = DateConverter.ConvertToRTS(DateTime.UtcNow.ToLocalTime());

                byte[] passwordHash, passwordSalt;
                PasswordHash.CreatePasswordHash("user2019", out passwordHash, out passwordSalt);

                var user = new User {
                    FirstName = "Пользователь",
                    LastName = "",
                    Username = "user",
                    Id = Guid.NewGuid(),
                    Role = viewerRole,
                    StartDate = startDate,
                    IsConfirmed = true,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
