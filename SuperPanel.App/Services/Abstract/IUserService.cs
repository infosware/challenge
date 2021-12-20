using SuperPanel.App.Models;
using SuperPanel.App.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperPanel.App.Services.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        
        Task<UsersViewModel>  GetUsersBy(int pageSize, int pageNumber);

        Task<User> GetUserBy(int userId);
        
        Task<User> GetUserBy(string userEmail);

        Task<GDPRResultViewModel> RequestGDPR(List<string> userEmails);
    }
}
