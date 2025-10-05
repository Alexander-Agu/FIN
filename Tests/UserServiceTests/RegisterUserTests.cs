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
    public class RegisterUserTests
    {
        private readonly FinContext context;
        private readonly UserService service;
        private readonly ToolService tools;

        public RegisterUserTests()
        {
            var options = new DbContextOptionsBuilder<FinContext>().UseInMemoryDatabase(
                    databaseName: Guid.NewGuid().ToString()
                ).Options;

            context = new FinContext(options);
            tools = new ToolService();
            service = new UserService(context, tools);
        }
        
        // Delete memory DB after each test
        public void Dispose() {
            context.Dispose();
        }


        /*
        *   Scenario 1: User does not exist in the database yet
        */
        [Fact]
        public async Task Register_User_Successfully()
        {
            // Given that user submits all valid information
            // When a request is made to create an account 
            var result = await service.RegisterUserAsync(new RegisterUserDto()
            {
                Firstname = "Joseph",
                Lastname = "Dabo",
                Email = "ahrity68@gmail.com",
                Password = "!@HGJHJK789uhlkn",
                Phone = "",
            }); // Adding new user


            // Then the information will be saved in the database
            // And an email will be sent to the user to activate their account
            // And the response will be a Success
            Assert.Equal(Result.Success, result["result"]);
        }


        // Scenario 2: User already exists in the database
        [Fact]
        public async Task Register_Existing_User()
        {
            // Given that a user exists in the database
            // And a user tries to create an account with an existing email
            await context.users.AddAsync(new User()
            {
                Id = 1,
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "ahrity68@gmail.com",
                Password = "al3x@gu2024#",
                Phone = "0784322389",
                Enabled = false,
            });
            await context.SaveChangesAsync();

            // When a request is made to create a new account

            var result = await service.RegisterUserAsync(new RegisterUserDto()
            {
                Firstname = "Joseph",
                Lastname = "Dabo",
                Email = "ahrity68@gmail.com",
                Password = "!@HGJHJK789uhlkn",
                Phone = "",
            }); // Adding new user

            // Then the information will not be saved
            // And the response will be an error
            Assert.Equal(Result.Error, result["result"]);

            // And the message will be Account already exists
            Assert.Equal("Account already exists", result["message"]);
        }


        // Scenario 3: User submits an invalid email
        [Fact]
        public async Task Register_Invalid_Email()
        {
            // Given that the submits an invalid email
            // When a request is made to a user is made
            var result = await service.RegisterUserAsync(new RegisterUserDto()
            {
                Firstname = "Joseph",
                Lastname = "Dabo",
                Email = "gmail.com",
                Password = "!@HGJHJK789uhlkn",
                Phone = "",
            }); // Adding new user

            // Then user information will not be saved
            // And the user will recieve an error response
            Assert.Equal(Result.Error, result["result"]);

            // And a message of Invalid email type
            Assert.Equal("Invalid email type", result["message"]);
        }


        // Scenario 4: User submits an invalid Password
        [Fact]
        public async Task Register_Invalid_Password()
        {
            // Given that the submits an invalid password
            // When a request is made to a user is made
            var result = await service.RegisterUserAsync(new RegisterUserDto()
            {
                Firstname = "Joseph",
                Lastname = "Dabo",
                Email = "alex@gmail.com",
                Password = "1234",
                Phone = "",
            }); // Adding new user

            // Then user information will not be saved
            // And the user will recieve an error response
            Assert.Equal(Result.Error, result["result"]);

            // And a message of Invalid password type
            Assert.Equal("Invalid password type", result["message"]);
        }


        // Scenario 5: User submits an invalid Phone number
        [Fact]
        public async Task Register_Invalid_Phone_Number()
        {
            // Given that the submits an invalid phone number
            // When a request is made to a user is made
            var result = await service.RegisterUserAsync(new RegisterUserDto()
            {
                Firstname = "Joseph",
                Lastname = "Dabo",
                Email = "alex@gmail.com",
                Password = "123HHggg%^&*(4",
                Phone = "asd345678",
            }); // Adding new user

            // Then user information will not be saved
            // And the user will recieve an error response
            Assert.Equal(Result.Error, result["result"]);

            // And a message of Invalid phone number type
            Assert.Equal("Invalid phone number type", result["message"]);
        }
    }
}
