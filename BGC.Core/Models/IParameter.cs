﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public interface IParameter<T>
    {
        string Name { get; }

        T Value { get; set; }
    }
}
