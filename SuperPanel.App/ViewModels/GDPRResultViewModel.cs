using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.ViewModels
{
    public class GDPRResultViewModel
    {
        public Dictionary<string, User> UpdatedUsers { get; set; } = new Dictionary<string, User>();
        public List<string> NotFoundUserEmails { get; set; } = new List<string>();
    }
}
