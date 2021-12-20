using SuperPanel.App.Models;
using System.Collections.Generic;

namespace SuperPanel.App.ViewModels
{
    public class GDPRResultViewModel
    {
        public Dictionary<string, User> UpdatedUsers { get; set; } = new Dictionary<string, User>();
        public List<string> NotFoundUserEmails { get; set; } = new List<string>();
    }
}
