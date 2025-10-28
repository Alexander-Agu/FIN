using System;
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
    // I want to recieve a confirmation email when I try to register my account
    // So that I can activate my account
    public class SendEmailToUserTests
    {
        private readonly FinContext context;
        private readonly UserService service;
        private readonly ToolService tools;

        public SendEmailToUserTests()
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

        // Scenario 1: User entered correct details when creating an account
        [Fact]
        public async void SendEmailToRegisteredUserAsync()
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

            // And they will be sent an email
            Assert.Equal("Confirmation email sent", result["message"]);
        }


        // Scenario 2: User Requesting to resend a varification email
        [Fact]
        public async void ResendVarificationEmailAsync()
        {
            // Given that user submits all valid information
            // When a request is made to create an account 
            // And the information will be saved in the database
            var registerUser = await service.RegisterUserAsync(new RegisterUserDto()
            {
                Firstname = "Joseph",
                Lastname = "Dabo",
                Email = "ahrity68@gmail.com",
                Password = "!@HGJHJK789uhlkn",
                Phone = "",
            }); // Adding new user

            // And request to resend the varification email
            var result = await service.ResendVarificationMailAsync("ahrity68@gmail.com");

            // Then the user will be sent a varification email
            Assert.Equal("Varification email sent", result["message"]);

            // And the response will be a Success
            Assert.Equal(Result.Success, result["result"]);
        }


        // Scenario 3: User Requesting to resend a varification email
        [Fact]
        public async void ResendVarificationEmailWhenUserIsNotFoundAsync()
        {
            // Given that user does not yet exist in the database
            // And request to resend the varification email
            var result = await service.ResendVarificationMailAsync("ahrity68@gmail.com");

            // Then the user will not be sent a varification email
            Assert.Equal("Email not found, Varification not sent", result["message"]);

            // And the response will be a Error
            Assert.Equal(Result.Error, result["result"]);
        }
    }
}