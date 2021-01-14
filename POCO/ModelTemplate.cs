using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Templates
{
    using Models;

    /// <summary>
    /// This class represents ModelTemplate class.
    /// This a partial class of the template class generated from ModelTemplate.tt.
    /// Note that the namespace - PocoGen.Templates matches the template file namespace.
    /// </summary>
    partial class ModelTemplate
    {
        public string Namespace { get; set; }
        public bool IncludeRelationships { get; set; }
        public Table Table { get; set; }
        public Tables Tables { get; set; }
    }
}
