using FIN.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FIN.Entities;
using FIN.Service.UserService;
using FIN.Service.ToolService;
using FIN.Dtos.UserDtos;
using FIN.Enums;

namespace Tests.UserServiceTests
{
    /*
    *   As a user
    *   I want to create an account
    *   So I can get access to the application
    */
    public class LoginUserTests
    {
        private readonly FinContext context;
        private readonly UserService service;
        private readonly ToolService tools;

        public LoginUserTests()
        {
            var options = new DbContextOptionsBuilder<FinContext>().UseInMemoryDatabase(
                    databaseName: Guid.NewGuid().ToString()
                ).Options;

            context = new FinContext(options);
            tools = new ToolService();
            service = new UserService(context, tools);
        }

        // Delete memory DB after each test
        public void Dispose()
        {
            context.Dispose();
        }


        // Scenario 1: User logged in successfully
        [Fact]
        public async Task Login_User_Successfuly()
        {
            // Given that theg user exists in the database
            await context.users.AddAsync(new User()
            {
                Id = 1,
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "ahrity68@gmail.com",
                Password = "al3x@gu2024#",
                Phone = "0784322389",
                Enabled = true,
            });
            await context.SaveChangesAsync();

            // And tries to log into their account
            var result = await service.LoginAsync(new LoginDto()
            {
                Email = "ahrity68@gmail.com",
                Password = "al3x@gu2024#"
            });

            // Than they will receive a successful response
            Assert.Equal(Result.Success, result["result"]);

            // And receive a Logged in successful message
            Assert.Equal("Logged in successfully", result["message"]);
        }


        // Scenario 2: Log in to a not existing account'
        [Fact]
        public async Task Log_Into_A_Non_Existant_Account()
        {
            // Given that the user does not yet exist in the database
            // And tries to make a request to log into theor
            var result = await service.LoginAsync(new LoginDto()
            {
                Email = "alex@gmail.com",
                Password = "GFHGJBnklmfgjkhj^%&*546789"
            });

            // Then they will get an error response
            Assert.Equal(Result.Error, result["result"]);

            // And they will ' Invalid email or password ' message
            Assert.Equal("Invalid email or password", result["message"]);
        }


        // Scenario 3: User tries to login will an invalid email
        [Fact]
        public async Task Login_With_An_Invalid_Email()
        {
            // Given that theg user exists in the database
            await context.users.AddAsync(new User()
            {
                Id = 1,
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "ahrity68@gmail.com",
                Password = "al3x@gu2024#",
                Phone = "0784322389",
                Enabled = true,
            });
            await context.SaveChangesAsync();

            // And tries to log into their account with an invalid email
            var result = await service.LoginAsync(new LoginDto()
            {
                Email = "ty68@gmail.com",
                Password = "al3x@gu2024#"
            });

            // Than they will receive an error response
            Assert.Equal(Result.Error, result["result"]);

            // And receive a Invalid email or password message
            Assert.Equal("Invalid email or password", result["message"]);
        }


        // Scenario 4: User tries to login will an invalid password
        [Fact]
        public async Task Login_With_An_Invalid_Password()
        {
            // Given that theg user exists in the database
            await context.users.AddAsync(new User()
            {
                Id = 1,
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "ahrity68@gmail.com",
                Password = "al3x@gu2024#",
                Phone = "0784322389",
                Enabled = true,
            });
            await context.SaveChangesAsync();

            // And tries to log into their account with an invalid password
            var result = await service.LoginAsync(new LoginDto()
            {
                Email = "ahrity68@gmail.com",
                Password = "al3x@gu2"
            });

            // Than they will receive an error response
            Assert.Equal(Result.Error, result["result"]);

            // And receive a Invalid email or password message
            Assert.Equal("Invalid email or password", result["message"]);
        }
    }
}
