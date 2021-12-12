using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.DistributedServices.Abstract
{
    public interface IExternalContacts
    {
        User GetUserBy(int userId);
        
        User GetUserBy(string userEmail);
        
        User GDPR(int userId);

        User GDPRDelete(int userId);
    }
}
