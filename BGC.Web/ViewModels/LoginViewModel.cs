﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.WebAPI.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}