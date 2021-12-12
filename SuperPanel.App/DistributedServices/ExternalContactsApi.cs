using SuperPanel.App.DistributedServices.Abstract;
using SuperPanel.App.Helpers;
using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.DistributedServices
{
    public class ExternalContactsApi : ApiEndpoint, IExternalContacts
    {
        public ExternalContactsApi()
        {
            Endpoint = AppSettings.ExternalContactsApiUrl;
        }

        public User GetUserBy(int userId)
        {
            return GetData<User>($"v1/contacts/{userId}");
        }

        public User GetUserBy(string userEmail)
        {
            return GetData<User>($"v1/contacts/{userEmail}");
        }
        
        public User GDPR(int userId)
        {
            return PutData<User>($"v1/contacts/{userId}/gdpr", null);
        }

        public User GDPRDelete(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
