﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Test
{
    using System;

    /// <summary>
    /// This class represents Argumentc class.
    /// </summary>
    public class Argument
    {
        public const string DbTypeArgTag = "dbtype";
        public const string ConnStringArgTag = "conn_string";
        public const string ConnStringNameArgTag = "conn_string_name";
        public const string IncludeRelationshipsArgTag = "include_relationships";
        public const string NamespaceArgTag = "namespace";
        public const string ModelsLocationArgTag = "models_location";
        public const string CsprojLocationArgTag  = "csproj_location";
        public const string ProjectLocationArgTag = "project_location";
        public string Name { get; set; }
        public string Value { get; set; }

        public bool Required
        {
            get
            {
                switch (Name)
                {
                    case DbTypeArgTag:
                    case ConnStringArgTag:
                    case ConnStringNameArgTag:
                    case ProjectLocationArgTag:
                        return true;
                }

                return false;
            }
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Value))
            {
                return false;
            }

            switch (Name)
            {
                case DbTypeArgTag:
                case ConnStringArgTag:
                case ConnStringNameArgTag:
                case NamespaceArgTag:
                case ModelsLocationArgTag:
                case CsprojLocationArgTag:
                case ProjectLocationArgTag:
                    return true;
                case IncludeRelationshipsArgTag:
                    if (string.Compare("true", Value, StringComparison.CurrentCultureIgnoreCase) == 0 ||
                        string.Compare("false", Value, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}
