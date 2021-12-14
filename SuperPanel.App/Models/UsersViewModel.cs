using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.Models
{
    public class UsersViewModel
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();
        public int TotalCount { get; set; }
    }
}
