﻿using SuperPanel.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.App.Services.Abstract
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        
        UsersData GetUsersBy(int pageSize, int pageNumber);
    }
}