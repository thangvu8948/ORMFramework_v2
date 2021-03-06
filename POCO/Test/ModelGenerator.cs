﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO.Test
{
    using POCO.Models;
    using POCO.Readers;
    using POCO.Templates;
    using System;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// This class represents ModelsGenerator class.
    /// </summary>
    public class ModelsGenerator//make cs file
    {
        public static void Generate(ReaderParameters readerParameters)
        {
            using (var schemaReader = SchemaReaderProvider.GetReader(readerParameters.ServerType))
            {
                var connectionString = readerParameters.ConnectionString;
                if (readerParameters.UseConnectionStringName)
                {
                    connectionString = Utils.getConnectionString(readerParameters.ProjectLocation, readerParameters.ConnectionStringName);
                    //connectionString = ConfigurationManager.ConnectionStrings[readerParameters.ConnectionStringName].ConnectionString;
                }

                if (schemaReader == null || string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Schema reader object or connection ring not valid!");
                }

                if (string.IsNullOrEmpty(readerParameters.Namespace))
                {
                    readerParameters.Namespace = readerParameters.ServerType.ToString() + ".Models";
                }
                if (string.IsNullOrEmpty(readerParameters.ModelsLocation))
                {
                    readerParameters.ModelsLocation = readerParameters.ServerType.ToString() + ".Models";
                }

                var tables = schemaReader.ReadSchema(connectionString);
                var p = new Microsoft.Build.Evaluation.Project();
                
                if (!string.IsNullOrEmpty(readerParameters.CsprojLocation))
                {
                    p = new Microsoft.Build.Evaluation.Project(readerParameters.CsprojLocation); ;
                }

                foreach (var table in tables)
                {
                    var model = new ModelTemplate();
                    model.Namespace = readerParameters.Namespace;
                    model.IncludeRelationships = readerParameters.IncludeRelationships;
                    model.Table = table;
                    model.Tables = tables;

                    // get page content
                    string pageContent = model.TransformText();


                    if (!Directory.Exists(readerParameters.ModelsLocation))
                        Directory.CreateDirectory(readerParameters.ModelsLocation);

                    // Write model to file
                    string fileName = table.ClassName + ".cs";
                    Console.WriteLine(string.Format("Creating file {0} ...", fileName));
                    File.WriteAllText(Path.Combine(readerParameters.ModelsLocation, fileName), pageContent);

                    //Add file to project

                    if (!string.IsNullOrWhiteSpace(readerParameters.CsprojLocation))
                    {
                        try
                        {
                            p.AddItem("Compile", Path.Combine(readerParameters.ModelsLocation, fileName));
                            p.Save();
                            Console.WriteLine("Auto Add file {0} success", fileName);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Auto Add file failed");
                        };
                    }
                }
            }
        }
    }
}
