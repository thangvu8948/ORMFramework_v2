using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Models
{
    using System;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    /// <summary>
    /// This class represents a schema reading utitlity class.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Cleans up string matching the compiled regex supplied.
        /// </summary>
        public static Regex RxCleanUp = new Regex(@"[^\w\d_]", RegexOptions.Compiled);

        /// <summary>
        /// Cleans up function.
        /// </summary>
        public static Func<string, string> CleanUp = (str) =>
        {
            str = RxCleanUp.Replace(str, "_");
            if (char.IsDigit(str[0])) str = "_" + str;

            return str;
        };

        /// <summary>
        /// Checks if column is nullable.
        /// </summary>
        public static string CheckNullable(Column col)
        {
            string result = "";
            if (col.IsNullable &&
                col.PropertyType != "byte[]" &&
                col.PropertyType != "string" &&
                col.PropertyType != "Microsoft.SqlServer.Types.SqlGeography" &&
                col.PropertyType != "Microsoft.SqlServer.Types.SqlGeometry"
                )
                result = "?";
            return result;
        }

        /// <summary>
        /// Singularize word.
        /// </summary>
        public static string Singularize(string word)
        {
            var singularword =
                PluralizationService.CreateService(
                    CultureInfo.GetCultureInfo("en-us")).Singularize(word);
            return singularword;
        }

        /// <summary>
        /// Pluralize word.
        /// </summary>
        public static string Pluralize(string word)
        {
            word = PascalCase(word);
            var singularword =
                PluralizationService.CreateService(
                    CultureInfo.GetCultureInfo("en-us")).Pluralize(word);
            return singularword;
        }

        /// <summary>
        /// Function removes table prefixes not required.
        /// </summary>
        public static string RemoveTablePrefixes(string word)
        {
            var cleanword = word;
            if (cleanword.StartsWith("tbl_")) cleanword = cleanword.Replace("tbl_", "");
            if (cleanword.StartsWith("tbl")) cleanword = cleanword.Replace("tbl", "");
            return cleanword;
        }

        /// <summary>
        /// Gets or sets a value indicating whether listed table prefixes are excluded.
        /// </summary>
        public static bool IsExcluded(string tablename, string[] excludeTablePrefixes)
        {
            for (int i = 0; i < excludeTablePrefixes.Length; i++)
            {
                string s = excludeTablePrefixes[i];
                if (tablename.StartsWith(s)) return true;
            }
            return false;
        }

        /// <summary>
        /// Cleans table name.
        /// </summary>
        public static string CleanName(string tablename)
        {
            string cleanName = CleanUp(tablename);
            if (cleanName.StartsWith("tbl_"))
            {
                cleanName = cleanName.Replace("tbl_", "");
            }
            if (cleanName.StartsWith("tbl"))
            {
                cleanName = cleanName.Replace("tbl", "");
            }
            return cleanName;
        }

        /// <summary>
        /// Converts singularized cleaned  table (or column) name to first letter capitalized.
        /// </summary>
        public static string CleanNameToClassName(string cleanName)
        {
            string className = Singularize(RemoveTablePrefixes(cleanName));
            className = PascalCase(className);

            return className;
        }

        /// <summary>
        /// Make get property name prettier.
        /// </summary>
        public static string PascalCase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            string modName = string.Empty;
            bool toUpper = false;
            for (int ctr = 0; ctr <= name.Length - 1; ctr++)
            {
                if (ctr == 0)
                {
                    modName = char.ToUpper(name[ctr]).ToString(CultureInfo.InvariantCulture);
                    continue;
                }
                if (name[ctr] == '_')
                {
                    toUpper = true;
                    continue;
                }
                if (toUpper)
                {
                    modName += char.ToUpper(name[ctr]).ToString(CultureInfo.InvariantCulture);
                    toUpper = false;
                    continue;
                }
                modName += name[ctr].ToString(CultureInfo.InvariantCulture);
            }
            return modName;
        }

        public static string getConnectionString(string projectLocation, string connectionName)
        {
            string[] lines = System.IO.File.ReadAllLines(Path.Combine(projectLocation, "App.config"));
            string cntLine = "";
            const string NameLabel = "name=\"";
            const string CntStrLabel = "connectionString=\"";
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int idxNameCfg = line.IndexOf("name=\"");
                if (idxNameCfg > -1)
                {
                    string name = "";
                    int endNameIdx = line.IndexOf("\"", idxNameCfg + NameLabel.Length);
                    name = line.Substring(idxNameCfg + NameLabel.Length, endNameIdx - (idxNameCfg + NameLabel.Length));
                    if (name == connectionName)
                    {
                        string cntstr = "";
                        int startCntIdx = line.IndexOf("\"", endNameIdx + 1);
                        int endCntIdx = line.IndexOf("\"", startCntIdx + CntStrLabel.Length);
                        cntLine = line.Substring(startCntIdx + 1, endCntIdx - (startCntIdx + 1));
                        return cntLine;
                    }
                }

            }
            return cntLine;
        }
    }
}
