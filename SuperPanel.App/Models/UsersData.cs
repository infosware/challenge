using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.Models
{
    public class UsersData
    {
        public IEnumerable<User> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
