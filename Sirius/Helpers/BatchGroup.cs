using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sirius.Helpers
{
    public class BatchGroup
    {
        public string Name { get; set; }
        public IEnumerable<Batch> Batches { get; set; }
}
}
