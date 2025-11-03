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


namespace Tests.AdminServiceTests
{
    // As an admin 
    // I want to log into my account
    // So that I can gain access to my account
    public class AdminOnboardingTests
    {
        private readonly FinContext context;
        private readonly AdminService service;
        private readonly ToolService tools;

        public AdminOnboardingTests()
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

        // Scenario 1: When an admin creates an account with valid information
        [Fact]
        public async Task RegisterAdminTestAsync()
        {
            // Given that the admin has valid information
            // And attempts to create an account
            var result = await service.RegisterAsync(new CreateAdminDto()
            {
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "alex@gmail.com",
                Password = "asdfghjkl!@#$%^&*(123456789DFGHJ)",
                Phone = ""
            });

            // Then their account should be created and 
            // And the response message is Admin account created
            Assert.Equal("Admin account created", result["message"]);

            // And the response type is a Success
            Assert.Equal(Result.Success, result["result"]);
        }

        // Scenario 2: When an admin creates an account with an invalid password
        [Fact]
        public async Task RegisterAdminWithInvalidPasswordTestAsync()
        {
            // Given that the admin inputs invalid password
            // And attempts to create an account
            var result = await service.RegisterAsync(new CreateAdminDto()
            {
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "alex@gmail.com",
                Password = "asdfgh",
                Phone = ""
            });

            // Then their account should not be created and 
            // And the response message is Invalid password type
            Assert.Equal("Invalid password type", result["message"]);

            // And the response type is a Error
            Assert.Equal(Result.Error, result["result"]);
        }

        // Scenario 3: When an admin creates an account with an invalid email
        [Fact]
        public async Task RegisterAdminWithInvalidEmailTestAsync()
        {
            // Given that the admin inputs invalid email
            // And attempts to create an account
            var result = await service.RegisterAsync(new CreateAdminDto()
            {
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "@gmail.com",
                Password = "asdfghHTGJ5676879@#$%^",
                Phone = ""
            });

            // Then their account should not be created and 
            // And the response message is Invalid email type
            Assert.Equal("Invalid email type", result["message"]);

            // And the response type is a Error
            Assert.Equal(Result.Error, result["result"]);
        }

        // Scenario 4: When an admin creates an account with an invalid phone number
        [Fact]
        public async Task RegisterAdminWithInvalidPhoneTestAsync()
        {
            // Given that the admin inputs invalid phone number
            // And attempts to create an account
            var result = await service.RegisterAsync(new CreateAdminDto()
            {
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "agu@gmail.com",
                Password = "asdfghHTGJ5676879@#$%^",
                Phone = "576879nbmn,"
            });

            // Then their account should not be created and 
            // And the response message is Invalid phone number
            Assert.Equal("Invalid phone number", result["message"]);

            // And the response type is a Error
            Assert.Equal(Result.Error, result["result"]);
        }

        // Scenario 5: When an admin creates an account with an invalid phone number
        [Fact]
        public async Task RegisterAnExistingUserTestAsync()
        {
            // Given that the admin already created an account
            var result = await service.RegisterAsync(new CreateAdminDto()
            {
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "agu@gmail.com",
                Password = "asdfghHTGJ5676879@#$%^",
                Phone = ""
            });

            // And attempts to create another account
            result = await service.RegisterAsync(new CreateAdminDto()
            {
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "agu@gmail.com",
                Password = "asdfghHTGJ5676879@#$%^",
                Phone = ""
            });

            // Then their account should not be created and 
            // And the response message is Admin already exists
            Assert.Equal("Admin already exists", result["message"]);

            // And the response type is a Error
            Assert.Equal(Result.Error, result["result"]);
        }

        // Scenario 6: When an admin logs into their account
        [Fact]
        public async Task LoginAdminTestAsync()
        {
            // Given that the admin already created an account
            Admin? admin = new Admin();
            admin.Firstname = "Alexander";
            admin.Lastname = "Agu";
            admin.Email = "agu1@gmail.com";
            admin.Password = "asdfghHTGJ5676879@#$%^";
            admin.Phone = "";
            admin.Enable = true;

            await context.admins.AddAsync(admin);
            await context.SaveChangesAsync();

            // And attempts to log into their account
            var result = await service.LoginAsync(new LoginDto()
            {
                Email = "agu1@gmail.com",
                Password = "asdfghHTGJ5676879@#$%^",
            });

            // Then they get logged into their account 
            // And the response message is Logged in
            Assert.Equal("Logged in", result["message"]);

            // And the response type is a Success
            Assert.Equal(Result.Success, result["result"]);
        }

        // Scenario 7: When an admin logs into their account with an un-activated account
        [Fact]
        public async Task LoginUnActivatedAdminTestAsync()
        {
            // Given that the admin already created an account thats not activated
            Admin? admin = new Admin();
            admin.Firstname = "Alexander";
            admin.Lastname = "Agu";
            admin.Email = "agu1@gmail.com";
            admin.Password = "asdfghHTGJ5676879@#$%^";
            admin.Phone = "";
            admin.Enable = false;

            await context.admins.AddAsync(admin);
            await context.SaveChangesAsync();

            // And attempts to log into their account
            var result = await service.LoginAsync(new LoginDto()
            {
                Email = "agu1@gmail.com",
                Password = "asdfghHTGJ5676879@#$%^",
            });

            // Then they will not get logged into their account 
            // And the response message is Invalid password or email
            Assert.Equal("Invalid password or email", result["message"]);

            // And the response type is a Error
            Assert.Equal(Result.Error, result["result"]);
        }

        // Scenario 8: When an admin logs into their account win an invalid email
        [Fact]
        public async Task LoginAdminWithInvalidEmailTestAsync()
        {
            // Given that the admin already created an account
            Admin? admin = new Admin();
            admin.Firstname = "Alexander";
            admin.Lastname = "Agu";
            admin.Email = "agu1@gmail.com";
            admin.Password = "asdfghHTGJ5676879@#$%^";
            admin.Phone = "";
            admin.Enable = true;

            await context.admins.AddAsync(admin);
            await context.SaveChangesAsync();

            // And attempts to log into their account with an invalid email
            var result = await service.LoginAsync(new LoginDto()
            {
                Email = "yoman@gmail.com",
                Password = "asdfghHTGJ5676879@#$%^",
            });

            // Then they will not get logged into their account 
            // And the response message is Invalid password or email
            Assert.Equal("Invalid password or email", result["message"]);

            // And the response type is a Error
            Assert.Equal(Result.Error, result["result"]);
        }

        // Scenario 9: When an admin logs into their account win an invalid password
        [Fact]
        public async Task LoginAdminWithInvalidPasswordTestAsync()
        {
            // Given that the admin already created an account
            Admin? admin = new Admin();
            admin.Firstname = "Alexander";
            admin.Lastname = "Agu";
            admin.Email = "agu1@gmail.com";
            admin.Password = "asdfghHTGJ5676879@#$%^";
            admin.Phone = "";
            admin.Enable = true;

            await context.admins.AddAsync(admin);
            await context.SaveChangesAsync();

            // And attempts to log into their account with an invalid password
            var result = await service.LoginAsync(new LoginDto()
            {
                Email = "agu1@gmail.com",
                Password = "asdfghHTGJ567^",
            });

            // Then they will not get logged into their account 
            // And the response message is Invalid password or email
            Assert.Equal("Invalid password or email", result["message"]);

            // And the response type is a Error
            Assert.Equal(Result.Error, result["result"]);
        }
    }
}