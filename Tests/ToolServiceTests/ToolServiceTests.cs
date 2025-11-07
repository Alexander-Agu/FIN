using System;
using System.Threading.Tasks;
using FIN.Enums;
using FIN.Service.ToolService;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.ToolServiceTests
{
    public class ToolServiceTests
    {
        public ToolService tools;

        public ToolServiceTests()
        {
            tools = new ToolService();
        }

        // Testing a valid email
        [Fact]
        public void ValidateEmailTest()
        {
            bool result = tools.ValidateEmail("agu@gmail.com");

            // If email is valid == True
            Assert.Equal(true, result);
        }

        // Testing an invalid email
        [Fact]
        public void ValidateInvalidEmailTest()
        {
            bool result = tools.ValidateEmail("gmail.com");

            // If email is invalid == False
            Assert.Equal(false, result);
        }

        // Testing a valid password
        [Fact]
        public void ValidatePasswordTest()
        {
            bool result = tools.ValidatePassword("gm1223GHKLL@#ail*m");

            // If password is valid == True
            Assert.Equal(true, result);
        }

        // Testing a valid password less than 8 characters
        [Fact]
        public void ValidateLowerLengthPasswordTest()
        {
            bool result = tools.ValidatePassword("Qw1@");

            // If password is lower than 8 characters == False
            Assert.Equal(false, result);
        }

        // Testing a valid password has no lowercase
        [Fact]
        public void ValidateNoneLowercasePasswordTest()
        {
            bool result = tools.ValidatePassword("CBN456789#$%^&*()");

            // If password has no lowercase == False
            Assert.Equal(false, result);
        }

        // Testing a valid password has no uppercase
        [Fact]
        public void ValidateNoneUppercasePasswordTest()
        {
            bool result = tools.ValidatePassword("hgj456789#$%^&*()");

            // If password has no uppercase == False
            Assert.Equal(false, result);
        }

        // Testing a valid password has no numbers
        [Fact]
        public void ValidatePasswordWithNoNumbersTest()
        {
            bool result = tools.ValidatePassword("CBNghbm,njm,#$%^&*()");

            // If password has no numberse == False
            Assert.Equal(false, result);
        }

        // Testing a valid password has no special characters
        [Fact]
        public void ValidatePasswordWithNoSpecialCharTest()
        {
            bool result = tools.ValidatePassword("CBNghbm657890");

            // If password has no special characters == False
            Assert.Equal(false, result);
        }

        // Testing a valid phone number
        [Fact]
        public void ValidatePhoneNumberTest()
        {
            bool result = tools.ValidatePhoneNumber("0628745390");

            // If phone number is valid == True
            Assert.Equal(true, result);
        }

        // Testing an invalid phone number
        [Fact]
        public void ValidateInvalidPhoneNumberTest()
        {
            bool result = tools.ValidatePhoneNumber("0987651234ert");

            // If phone number is invalid == False
            Assert.Equal(false, result);
        }
    }
}