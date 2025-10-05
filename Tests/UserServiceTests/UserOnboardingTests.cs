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
    public class UserOnboardingTests
    {
        private FinContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<FinContext>().UseInMemoryDatabase(
                    databaseName: Guid.NewGuid().ToString()
                ).Options;

            return new FinContext(options);
        }

        /*
        * 
        */
        [Fact]
        public async Task Register_User_Successfully()
        {
            var context = GetInMemoryContext();

            IToolService service = new ToolService();
            var result = new UserService(context, service).RegisterUserAsync(new RegisterUserDto()
            {
                Firstname = "Alexander",
                Lastname = "Agu",
                Email = "ahrity67",
                Password = "!@HGJHJK789uhlkn",
            }); // Adding new user


            Assert.Equal(Result.Success, result.Result["result"]);
        }
    }
}
