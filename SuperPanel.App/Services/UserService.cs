using Microsoft.Extensions.Configuration;
using SuperPanel.App.Data;
using SuperPanel.App.DistributedServices.Abstract;
using SuperPanel.App.Helpers;
using SuperPanel.App.Models;
using SuperPanel.App.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await Task.FromResult(_userRepository.QueryAll());
        }

        public async Task<UsersViewModel> GetUsersBy(int pageSize, int pageNumber)
        {
            return await Task.FromResult(new UsersViewModel { 
                Users = _userRepository.Query(pageSize, pageNumber),
                TotalCount = _userRepository.Count()
            });
        }

        public async Task<User> GetUserBy(int userId)
        {
            return await _externalContactsProxy.GetUserBy(userId);
        }

        public async Task<User> GetUserBy(string userEmail)
        {
            return await _externalContactsProxy.GetUserBy(userEmail);
        }

        public async Task<GDPRResultViewModel> RequestGDPR(List<string> userEmails)
        {
            var result = new GDPRResultViewModel();

            foreach (var email in userEmails)
            {
                var user = await _externalContactsProxy.GetUserBy(email);
                if (user == null)
                {
                    result.NotFoundUserEmails.Add(email);
                }
                else
                { 
                    var anonymizedUser = await _externalContactsProxy.GDPR(user.Id);
                    result.UpdatedUsers.Add(email, anonymizedUser);
                }                
            }

            _userRepository.UpdateGdpr(result.UpdatedUsers);

            return await Task.FromResult(result);
        }
    }
}
