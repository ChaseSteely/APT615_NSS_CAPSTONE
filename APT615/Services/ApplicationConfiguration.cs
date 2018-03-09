using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APT615.Services
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string GoogleApiKey { get; set; }
    }
}
