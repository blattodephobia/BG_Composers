﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public sealed class SendInvitePermission : Permission, ISendInvitePermission
    {
        public override string Name => nameof(ISendInvitePermission);
    }
}
