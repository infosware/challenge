using Microsoft.Extensions.Options;
using SuperPanel.App.Infrastructure;
using SuperPanel.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SuperPanel.App.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> QueryAll();
        
        IEnumerable<User> Query(int pageSize, int pageNumber);

        int Count();
    }

    public class UserRepository : IUserRepository
    {
        private List<User> _users;

        public UserRepository(IOptions<DataOptions> dataOptions)
        {
            // preload the set of users from file.
            var json = System.IO.File.ReadAllText(dataOptions.Value.JsonFilePath);
            _users = JsonSerializer.Deserialize<IEnumerable<User>>(json)
                .ToList();
        }

        public IEnumerable<User> QueryAll()
        {
            return _users;
        }

        public int Count()
        {
            return _users.Count;
        }

        public IEnumerable<User> Query(int pageSize, int pageNumber)
        {
            return _users
                .OrderBy(u => u.Id)
                .Skip(pageNumber * pageSize - pageSize)
                .Take(pageSize);
        }
    }
}
