﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Contract.Requests
{
    public class LoginRequest
    {
        public String? UserName { get; set; }
        public String? PassWord { get; set; }
    }
}
