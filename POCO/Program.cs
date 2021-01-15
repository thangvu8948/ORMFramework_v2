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
            //MY SQL CONNECTION STRING
            //string[] agruments = { "-dbtype:mysql", "-conn_string:server=127.0.0.1;port=3306;user id=root; database=hotel", "-include_relationships:true", "-namespace:POCO", "-models_location:E:\\OneDrive - VNU-HCMUS\\FIT-HCMUS\\FIT-NAM4\\3.MẪU THIẾT KẾ HĐT\\DoAn\\ORM - Copy\\POCO\\HOTEL\\" };

            //MICROSOFT SQL SERVER CONNECTION STRING
            //string[] agruments = { "-dbtype:mssql", "-conn_string:Data Source =.; Initial Catalog = QLKHACHSAN; Persist Security Info = True; User ID = sa; Password = ", "-include_relationships:true", "-namespace:POCO", "-models_location:E:\\OneDrive - VNU-HCMUS\\FIT-HCMUS\\FIT-NAM4\\3.MẪU THIẾT KẾ HĐT\\DoAn\\ORM - Copy\\POCO\\QLKHACHSAN\\" };

            //POSTGRES CONNECTION STRING
            string[] agruments = { "-dbtype:postgres", "-conn_string:Server =127.0.0.1;Port=5432;Database=QLKhachSan;User Id=postgres;Password=root;", "-include_relationships:true", "-namespace:POCO", "-models_location:E:\\OneDrive - VNU-HCMUS\\FIT-HCMUS\\FIT-NAM4\\3.MẪU THIẾT KẾ HĐT\\DoAn\\ORM\\POCO\\MODEL-POSTGRES\\" };

            var readerParameters = new ReaderParameters();
            var result = readerParameters.ParseArguments(agruments);//result là kết quả parse các tham số,  thành công sẽ lưu các tham số vào readerParameters

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
                        ModelsGenerator.Generate(readerParameters);//tạo các class như ở trên csdl
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
