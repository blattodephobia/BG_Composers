﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    [ServiceContract]
    public interface IUserManagementService
    {
        [OperationContract]
        IEnumerable<BgcUser> GetUsers();

        [OperationContract]
        Invitation Invite(string email, IEnumerable<BgcRole> role);

        BgcUser Administrator { get; set; }
    }
}
