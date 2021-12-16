using Moq;
using SuperPanel.App.Data;
using SuperPanel.App.DistributedServices.Abstract;
using SuperPanel.App.Models;
using SuperPanel.App.Services;
using SuperPanel.App.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SuperPanel.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IExternalContacts> _externalContactsApiMock;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _externalContactsApiMock = new Mock<IExternalContacts>();
            _userService = new UserService(_userRepoMock.Object, _externalContactsApiMock.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnListOfUsers()
        {
            var fakeUsers = new List<User>
            {
                new User { Id = 1, FirstName = "Danny", LastName = "Davis", Email = "dd@example.com", IsAnonymized = false, Login = "dd", Phone = "123", CreatedAt = DateTime.MinValue },
                new User { Id = 2, FirstName = "Tom", LastName = "Twen", Email = "tt@example.com", IsAnonymized = false, Login = "tt", Phone = "123", CreatedAt = DateTime.MinValue },
                new User { Id = 3, FirstName = "Ricky", LastName = "Romero", Email = "rr@example.com", IsAnonymized = false, Login = "rr", Phone = "123", CreatedAt = DateTime.MinValue }
            };
            _userRepoMock.Setup(r => r.QueryAll()).Returns(fakeUsers);

            var users = await _userService.GetAllUsers();

            Assert.Equal(fakeUsers, users);
        }

        [Fact]
        public async Task GetUsersBy_ShouldReturnListOfUsers()
        {
            int pageSize = 10;
            int pageNumber = 1;
            
            var fakeUsers = new List<User>
            {
                new User { Id = 1, FirstName = "Danny", LastName = "Davis", Email = "dd@example.com", IsAnonymized = false, Login = "dd", Phone = "123", CreatedAt = DateTime.MinValue },
                new User { Id = 2, FirstName = "Tom", LastName = "Twen", Email = "tt@example.com", IsAnonymized = false, Login = "tt", Phone = "123", CreatedAt = DateTime.MinValue },
                new User { Id = 3, FirstName = "Ricky", LastName = "Romero", Email = "rr@example.com", IsAnonymized = false, Login = "rr", Phone = "123", CreatedAt = DateTime.MinValue }
            };

            var expectedUsersViewModel = new UsersViewModel { Users = fakeUsers, TotalCount = fakeUsers.Count };


            _userRepoMock.Setup(r => r.Query(pageSize, pageNumber)).Returns(fakeUsers);
            _userRepoMock.Setup(r => r.Count()).Returns(fakeUsers.Count);

            var usersViewModel = await _userService.GetUsersBy(pageSize, pageNumber);


            Assert.Equal(expectedUsersViewModel.Users, usersViewModel.Users);
            Assert.Equal(expectedUsersViewModel.TotalCount, usersViewModel.TotalCount);
        }

        [Fact]
        public async Task GetUserBy_Email_ShouldReturnUser()
        {
            var fakeUser = new User { Id = 1, FirstName = "Danny", LastName = "Davis", Email = "dd@example.com", IsAnonymized = false, Login = "dd", Phone = "123", CreatedAt = DateTime.MinValue };

            _externalContactsApiMock.Setup(api => api.GetUserBy(fakeUser.Email)).ReturnsAsync(fakeUser);

            var user = await _userService.GetUserBy(fakeUser.Email);
            
            Assert.Equal(fakeUser, user);
        }

        [Fact]
        public async Task GetUserBy_Email_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var fakeUser = new User { Id = 1, FirstName = "Danny", LastName = "Davis", Email = "dd@example.com", IsAnonymized = false, Login = "dd", Phone = "123", CreatedAt = DateTime.MinValue };

            _externalContactsApiMock.Setup(api => api.GetUserBy(fakeUser.Email)).ReturnsAsync(() => null);

            var user = await _userService.GetUserBy(fakeUser.Email);

            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserBy_Id_ShouldReturnUser()
        {
            var fakeUser = new User { Id = 1, FirstName = "Danny", LastName = "Davis", Email = "dd@example.com", IsAnonymized = false, Login = "dd", Phone = "123", CreatedAt = DateTime.MinValue };

            _externalContactsApiMock.Setup(api => api.GetUserBy(fakeUser.Id)).ReturnsAsync(fakeUser);

            var user = await _userService.GetUserBy(fakeUser.Id);

            Assert.Equal(fakeUser, user);
        }

        [Fact]
        public async Task RequestGDPR_ShouldReturnExistingUsersDictonary()
        {
            var userEmails = new List<string> { "dd@example.com" };
            
            var fakeUpdatedUsers = new Dictionary<string, User>();
            fakeUpdatedUsers.Add("dd@example.com", new User { Id = 1, FirstName = "Danny", LastName = "Davis", Email = "dd@example.com", IsAnonymized = true, Login = "dd", Phone = "123", CreatedAt = DateTime.MinValue });
            
            var expectedGDPRResultViewModel = new GDPRResultViewModel { UpdatedUsers = fakeUpdatedUsers, NotFoundUserEmails = It.IsAny<List<string>>() };

            
            _externalContactsApiMock.Setup(api => api.GetUserBy(fakeUpdatedUsers.First().Key)).ReturnsAsync(fakeUpdatedUsers.First().Value);
            _externalContactsApiMock.Setup(api => api.GDPR(fakeUpdatedUsers.First().Value.Id)).ReturnsAsync(fakeUpdatedUsers.First().Value);
            
            var gdprResultViewModel = await _userService.RequestGDPR(userEmails);


            Assert.Equal(expectedGDPRResultViewModel.UpdatedUsers, gdprResultViewModel.UpdatedUsers);
        }

        [Fact]
        public async Task RequestGDPR_ShouldReturnNotFoundUserEmails()
        {
            var userEmails = new List<string> { "dd@example.com" };

            var expectedGDPRResultViewModel = new GDPRResultViewModel { UpdatedUsers = It.IsAny<Dictionary<string, User>>(), NotFoundUserEmails = userEmails };


            _externalContactsApiMock.Setup(api => api.GetUserBy(userEmails.First())).ReturnsAsync(() => null);

            var gdprResultViewModel = await _userService.RequestGDPR(userEmails);


            Assert.Equal(expectedGDPRResultViewModel.NotFoundUserEmails, gdprResultViewModel.NotFoundUserEmails);
        }
    }
}
