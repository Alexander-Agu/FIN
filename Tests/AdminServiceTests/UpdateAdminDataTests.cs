using System;
using System.Threading.Tasks;
using FIN.Dtos.AdminDtos;
using FIN.Entities;
using FIN.Enums;
using FIN.Repository;
using FIN.Service.ToolService;
using FIN.Service.AdminService;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection.Metadata;


namespace Tests.AdminServiceTests
{
    // As an admin
    // I want to be able to update my profile
    // So that i can be able to update my profile when needed
    public class UpdateAdminDataTests
    {
        private readonly FinContext context;
        private readonly AdminService service;
        private readonly ToolService tools;

        public UpdateAdminDataTests()
        {
            var options = new DbContextOptionsBuilder<FinContext>().UseInMemoryDatabase(
                databaseName: Guid.NewGuid().ToString()
            ).Options;
            context = new FinContext(options);
            tools = new ToolService();
            service = new AdminService(context, tools);
        }

        // Dispose context after use
        public void Dispose()
        {
            context.Dispose();
        }

        // Create an admin account
        private async Task<Admin> CreateAdminAccount()
        {
            Admin? admin = new Admin();
            admin.Firstname = "Alexander";
            admin.Lastname = "Agu";
            admin.Email = "agu1@gmail.com";
            admin.Password = "asdfghHTGJ5676879@#$%^";
            admin.Phone = "";
            admin.Enable = true;

            await context.admins.AddAsync(admin);
            await context.SaveChangesAsync();

            return admin;

        }

        // Scenario 1: When an admin want to update their profile name
        [Fact]
        public async Task UpdateAdminNameTestAsync()
        {
            // Given that the admin already has an account
            Task<Admin>? admin = CreateAdminAccount();

            // When the admin tries to update their name
            var result = await service.UpdateAdminProfileAsync(1, new UpdateAdminProfileDto()
            {
                Firstname = "Goated",
                Lastname = "HIM",
                Phone = ""
            });

            // Then their names will be updated
            Assert.Equal("Profile updated", result["message"]);
        }

        // Scenario 2: When an admin tried to update their number
        [Fact]
        public async Task UpdateAdminNumberTestAsync()
        {
            // Given that the admin already has an account
            Task<Admin>? admin = CreateAdminAccount();

            // When the admin tries to update their number
            var result = await service.UpdateAdminProfileAsync(1, new UpdateAdminProfileDto()
            {
                Firstname = "",
                Lastname = "",
                Phone = "0987654321"
            });

            // Then their number will be updated
            Assert.Equal("Profile updated", result["message"]);
        }

        // Scenario 3: When an admin does not exist int the database
        [Fact]
        public async Task AdminNotExistTestAsync()
        {
            // Given that the admin does not have an account or account is not activated
            // When the admin tries to update their number
            var result = await service.UpdateAdminProfileAsync(1, new UpdateAdminProfileDto()
            {
                Firstname = "",
                Lastname = "",
                Phone = "0987654321"
            });

            // Then their number will not be updated
            Assert.Equal("failed to updated admin", result["message"]);
        }

        // Scenario 4: When an admin tried to update their email
        [Fact]
        public async Task UpdateAdminEmailTestAsync()
        {
            // Given that the admin already has an account
            Task<Admin>? admin = CreateAdminAccount();

            // When the admin tries to update their email
            var result = await service.UpdateEmailAsync(1, new UpdateEmailDto()
            {
                Email = "new@gmail.com"
            });

            // Then their email will be updated
            Assert.Equal("Email updated", result["message"]);
        }

        // Scenario 4: When an admin tried to update their email with an invalid email
        [Fact]
        public async Task UpdateInvalidEmailTestAsync()
        {
            // Given that the admin already has an account
            Task<Admin>? admin = CreateAdminAccount();

            // When the admin tries to update their email with an invalid email
            var result = await service.UpdateEmailAsync(1, new UpdateEmailDto()
            {
                Email = "mail.com"
            });

            // Then their email will not be updated
            Assert.Equal("Invalid email type", result["message"]);
        }

        // Scenario 4: When an admin tried to update their Password
        [Fact]
        public async Task UpdateAdminPasswordTestAsync()
        {
            // Given that the admin already has an account
            Task<Admin>? admin = CreateAdminAccount();

            // When the admin tries to update their password
            var result = await service.UpdatePasswordAsync(1, new UpdatePasswordDto()
            {
                Password = "n567FHGJHKew@gmail.com"
            });

            // Then their password will be updated
            Assert.Equal("Passowrd changed", result["message"]);
        }

        // Scenario 4: When an admin tried to update their password with an invalid password
        [Fact]
        public async Task UpdateInvalidPasswordTestAsync()
        {
            // Given that the admin already has an account
            Task<Admin>? admin = CreateAdminAccount();

            // When the admin tries to update their password with an invalid password
            var result = await service.UpdatePasswordAsync(1, new UpdatePasswordDto()
            {
                Password = "mail.com"
            });

            // Then their password will not be updated
            Assert.Equal("Invalid password type", result["message"]);
        }
    }
}