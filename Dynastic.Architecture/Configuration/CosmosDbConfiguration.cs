using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynastic.Architecture.Configuration
{
    public class CosmosDbConfiguration
    {
        public string EndpointUri { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseId { get; set; }
        public string ContainerId { get; set; }
    }
}