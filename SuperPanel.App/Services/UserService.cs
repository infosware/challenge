using Microsoft.Extensions.Configuration;
using SuperPanel.App.Data;
using SuperPanel.App.Helpers;
using SuperPanel.App.Models;
using SuperPanel.App.Services.Abstract;
using System.Collections.Generic;

namespace SuperPanel.App.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.QueryAll();
        }

        public UsersData GetUsersBy(int pageSize, int pageNumber)
        {
            pageSize = pageSize > 0 ? pageSize : AppSettings.DefaultPageSize;

            return new UsersData { 
                Users = _userRepository.Query(pageSize, pageNumber),
                TotalCount = _userRepository.Count()
            };
        }
    }
}
