using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APT615.Services
{
    public interface IApplicationConfiguration
    {
        string GoogleApiKey { get; set; }
    }
}
