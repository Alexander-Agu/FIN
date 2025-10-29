using System;
using System.Threading.Tasks;
using FIN.Dtos.UserDtos;
using FIN.Entities;
using FIN.Enums;
using FIN.Repository;
using FIN.Service.ToolService;
using FIN.Service.UserService;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UserServiceTests
{
    // As a user
    // I want to be able to update my profile
    // So that I can be able to change my data when needed
    public class UpdateUserDataTests
    {
        private readonly FinContext context;
        private readonly UserService service;
        private readonly ToolService tools;

        public UpdateUserDataTests()
        {
            var options = new DbContextOptionsBuilder<FinContext>().UseInMemoryDatabase(
                databaseName: Guid.NewGuid().ToString()
            ).Options;

            context = new FinContext(options);
            tools = new ToolService();
            service = new UserService(context, tools);
        }

        // Dispose context after use
        public void Dispose()
        {
            context.Dispose();
        }


        // Scenaro 1: User exists in the databse and attempts to update their profile
        [Fact]
        public async void UpdateUserProfileAsync()
        {
            // Given the user created an account
            // And account is activated
            User? user = new User();
            user.Id = 1;
            user.Firstname = "Alexander";
            user.Lastname = "Agu";
            user.Email = "agu@gmail.com";
            user.Phone = user.Phone;
            user.ConfirmationToken = Guid.NewGuid().ToString();
            user.ConfirmationDeadline = DateTime.Now;
            user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            user.Enabled = true;

            await context.users.AddAsync(user);
            await context.SaveChangesAsync();

            // When user tries to update their profile
            var result = await service.UpdateUserAsync(user.Id, new UpdateUserDto()
            {
                Firstname = "Joseph",
                Lastname = "The man",
                Phone = "0876541111"
            });

            // Then their profile will be updated
            Assert.Equal("User updated", result["message"]);

            // And the response will be Success
            Assert.Equal(Result.Success, result["result"]);
        }


        // Scenario 2: User tries to update their profile while they don't exist in the database
        [Fact]
        public async void UpdateANonExistentUserAsync()
        {
            // Given that the user does not yet exist in the database
            // When they try to update their profile
            var result = await service.UpdateUserAsync(1, new UpdateUserDto()
            {
                Firstname = "Joseph",
                Lastname = "The man",
                Phone = "0876541111"
            });

            // Then their profile will not be updated because the use will not be found
            Assert.Equal("User not found", result["message"]);

            // And the response will be Error
            Assert.Equal(Result.Error, result["result"]);
        }


        // Scenario 3: User tries to update their email
        [Fact]
        public async Task UpdateUserEmailAsync()
        {
            // Given the user created an account
            // And account is activated
            User? user = new User();
            user.Id = 1;
            user.Firstname = "Alexander";
            user.Lastname = "Agu";
            user.Email = "agu@gmail.com";
            user.Phone = user.Phone;
            user.ConfirmationToken = Guid.NewGuid().ToString();
            user.ConfirmationDeadline = DateTime.Now;
            user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            user.Enabled = true;

            await context.users.AddAsync(user);
            await context.SaveChangesAsync();

            // When the user tried to update their email
            var result = await service.UpdateEmailAsync(1, new UpdateEmailDto()
            {
                Email = "newemail@gmail.com"
            });

            // Then their email will be updated
            Assert.Equal("Email updated", result["message"]);

            // And the response will a success response
            Assert.Equal(Result.Success, result["result"]);
        }


        // Scenario 4: User tries to update an invalid email
        [Fact]
        public async Task UpdateAnInvalidEmailAsync()
        {
            // Given the user created an account
            // And account is activated
            User? user = new User();
            user.Id = 1;
            user.Firstname = "Alexander";
            user.Lastname = "Agu";
            user.Email = "agu@gmail.com";
            user.Phone = user.Phone;
            user.ConfirmationToken = Guid.NewGuid().ToString();
            user.ConfirmationDeadline = DateTime.Now;
            user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            user.Enabled = true;

            await context.users.AddAsync(user);
            await context.SaveChangesAsync();

            // When the user tried to update an invalid email
            var result = await service.UpdateEmailAsync(1, new UpdateEmailDto()
            {
                Email = "123456"
            });

            // Then their email will be updated
            Assert.Equal("Failed to update email", result["message"]);

            // And the response will an Error response
            Assert.Equal(Result.Error, result["result"]);
        }


        // Scenario 5: User tries to reset their password
        [Fact]
        public async Task ResetUserPasswordAsync()
        {
            // Given the user created an account
            // And account is activated
            User? user = new User();
            user.Id = 1;
            user.Firstname = "Alexander";
            user.Lastname = "Agu";
            user.Email = "agu@gmail.com";
            user.Phone = user.Phone;
            user.ConfirmationToken = Guid.NewGuid().ToString();
            user.ConfirmationDeadline = DateTime.Now;
            user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            user.Enabled = true;

            await context.users.AddAsync(user);
            await context.SaveChangesAsync();

            // When the user tried to reset their password
            var result = await service.ResetPasswordAsync(user.ConfirmationToken, "ASDFG1234@#$%^&sdfgh");

            // Then their password will be changed
            Assert.Equal("Password changed", result["message"]);

            // And the response will a Success response
            Assert.Equal(Result.Success, result["result"]);
        }


        // Scenario 6: User tries to reset their password with an invalid new password
        [Fact]
        public async Task ResetInvalidPasswordAsync()
        {
            // Given the user created an account
            // And account is activated
            User? user = new User();
            user.Id = 1;
            user.Firstname = "Alexander";
            user.Lastname = "Agu";
            user.Email = "agu@gmail.com";
            user.Phone = user.Phone;
            user.ConfirmationToken = Guid.NewGuid().ToString();
            user.ConfirmationDeadline = DateTime.Now;
            user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            user.Enabled = true;

            await context.users.AddAsync(user);
            await context.SaveChangesAsync();

            // When the user tried to reset their password with an invalid password
            var result = await service.ResetPasswordAsync(user.ConfirmationToken, "the goat");

            // Then their password will be changed
            Assert.Equal("Invalid password", result["message"]);

            // And the response will an Error response
            Assert.Equal(Result.Error, result["result"]);
        }
    }
}