using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.DistributedServices.Abstract
{
    public interface IApiEndpoint
    {
        T GetData<T>(string url);
        T PostData<T>(string url, string body);
        T PutData<T>(string url, string body);
        T DeleteData<T>(string url);
    }
}
