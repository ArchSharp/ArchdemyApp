﻿using Application.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IJwtService : IAutoDependencyService
    {
        string GetJwtToken(User user);
    }
}
