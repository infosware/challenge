using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.DistributedServices.Abstract
{
    public interface IExternalContacts
    {
        Task<User> GetUserBy(int userId);

        Task<User> GetUserBy(string userEmail);

        Task<User> GDPR(int userId);
    }
}
