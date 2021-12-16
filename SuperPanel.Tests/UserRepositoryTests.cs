using Microsoft.Extensions.Options;
using SuperPanel.App.Data;
using SuperPanel.App.Infrastructure;
using SuperPanel.App.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SuperPanel.Tests
{
    public class UserRepositoryTests
    {
        private readonly UserRepository r;
        public UserRepositoryTests()
        {
            r = new UserRepository(Options.Create<DataOptions>(new DataOptions()
            {
                JsonFilePath = "./../../../../data/users.json"
            }));
        }

        [Fact]
        public void QueryAll_ShouldReturnEverything()
        {
            var all = r.QueryAll();

            Assert.Equal(5000, all.Count());
        }

        [Fact]
        public void Query_ShouldReturnByPaginatedItems()
        {
            var pageSize = 20;
            var pageNumber = 3;

            var expectedUsersCount = pageSize;
            var expectedFirstUserId = 10000 + pageSize * pageNumber - pageSize;
            var expectedLastUserId = expectedFirstUserId + pageSize - 1;

            var users = r.Query(pageSize, pageNumber);

            Assert.Equal(expectedUsersCount, users.Count());
            Assert.Equal(expectedFirstUserId, users.First().Id);
            Assert.Equal(expectedLastUserId, users.Last().Id);
        }

        [Fact]
        public void UpdateGdpr_ShouldUpdateUserProps()
        {
            var realUserId = 10000;
            var realUserEmail = "Joelle.Bergstrom@hotmail.com";

            var fakeUser = new User
            {
                Id = realUserId,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email@example.com",
                IsAnonymized = true,
                Login = "Login",
                Phone = "123",
                CreatedAt = System.DateTime.MinValue
            };

            var anonymizedUsers = new Dictionary<string, User>();
            anonymizedUsers.Add(realUserEmail, fakeUser);

            r.UpdateGdpr(anonymizedUsers);
            
            var dbUser = r.QueryAll().SingleOrDefault(u => u.Id == realUserId);

            Assert.NotNull(dbUser);

            Assert.Equal(dbUser.Id, fakeUser.Id);
            Assert.Equal(dbUser.FirstName, fakeUser.FirstName);
            Assert.Equal(dbUser.LastName, fakeUser.LastName);
            Assert.Equal(dbUser.Email, fakeUser.Email);
            Assert.Equal(dbUser.IsAnonymized, fakeUser.IsAnonymized);
            Assert.Equal(dbUser.Login, fakeUser.Login);
            Assert.Equal(dbUser.Phone, fakeUser.Phone);
            Assert.Equal(dbUser.CreatedAt, fakeUser.CreatedAt);
        }
    }
}
