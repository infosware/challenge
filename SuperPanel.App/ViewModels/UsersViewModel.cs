using SuperPanel.App.Models;
using System.Collections.Generic;

namespace SuperPanel.App.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();
        public int TotalCount { get; set; }
    }
}
