using POCO.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCO
{
    class Program
    {
        static void Main(string[] args)
        {
            var readerParameters = new ReaderParameters();
            var result = readerParameters.ParseArguments(args);

            switch (result)
            {
                case ParserResult.Invalid:
                    Console.Write(readerParameters.Usage);
                    break;
                case ParserResult.Failure:
                    Console.Write(readerParameters.ErrorMessage);
                    break;
                case ParserResult.Success:
                    try
                    {
                        ModelsGenerator.Generate(readerParameters);
                        Console.WriteLine("Completed successfully.");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        Console.WriteLine(exception.StackTrace);
                    }

                    break;
            }

            Console.ReadKey();
        }
    }
}
