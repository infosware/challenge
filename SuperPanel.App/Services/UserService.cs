using Microsoft.Extensions.Configuration;
using SuperPanel.App.Data;
using SuperPanel.App.DistributedServices.Abstract;
using SuperPanel.App.Helpers;
using SuperPanel.App.Models;
using SuperPanel.App.Services.Abstract;
using System.Collections.Generic;

namespace SuperPanel.App.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExternalContacts _externalContactsProxy;

        public UserService(IUserRepository userRepository, IExternalContacts externalContactsProxy)
        {
            _userRepository = userRepository;
            _externalContactsProxy = externalContactsProxy;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.QueryAll();
        }

        public UsersData GetUsersBy(int pageSize, int pageNumber)
        {
            return new UsersData { 
                Users = _userRepository.Query(pageSize, pageNumber),
                TotalCount = _userRepository.Count()
            };
        }

        public User RequestGDPR(int userId)
        {
            return _externalContactsProxy.GDPR(userId);
        }

        public User RequestGDPRDelete(int userId)
        {
            return _externalContactsProxy.GDPRDelete(userId);
        }
    }
}
