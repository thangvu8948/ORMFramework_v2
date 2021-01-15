//using Models.POCO;
using ORMFramework.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var a = new Test("MySqlConnectionString");
            //var b = a.Set<Staff>();
            //var t=b.CanJoin<Account>();
            //var p = a.Set<Account>();
            //var i = p.CanJoin<Staff>();
            //var c = b.Join<Account>(Tuple.Create("Account_id", "ID"), true);
            //var item = new Staff();
            //item.Email = "Sangdoan956@gmail.com";
            //item.Account_id = 1;
            //item.IdentityCard = "215483062";
            //item.Phone = "0589005648";
            //item.Name = "123aaaa";
            //item.Address = "bbb23123bbb";
            //Console.WriteLine(b.Insert(item));
            //var item1 = new { Email = "Doansang789@gmail.com" };
            //var y = b.Update(x => x.ID == 1000, item);
            //var d = b.Update(x => x.ID == 1000 && x.Account_id == 1000, item);
            //var z = b.Delete(x => x.ID == 1014);
            //var g8 = b.OrderBy("ID", Order.DESC).Excute();
            //var g = b.Where(x => x.Account_id == 1).OrderBy("ID", Order.DESC).Excute();


            ////****MS SQL
            //var a = new Test("SqlConnectionString");
            //var b = a.Set<LOAIDV>();

            //////Insert
            ////var item = new LOAIDV();
            ////item.LoaiDVID = "N224442";
            ////item.TenLoai = "Chăm sóc";
            ////item.NgayTao = DateTime.Now;
            ////Console.WriteLine(b.Insert(item));

            //////JOIN
            //var t = b.CanJoin<DICHVU>();
            //var c = b.Join<DICHVU>(Tuple.Create("LoaiDVID", "LoaiDVID"), true);

            ////DELETE
            //var z = b.Delete(x => x.LoaiDVID == "N224442");

            ////UPDATE
            //var item1 = new { TenLoai = "Giặt ủi" };
            //var y = b.Update(x => x.LoaiDVID == "GIAT", item1);

            ////ORDER
            //var d = a.Set<DICHVU>();
            //var g = d.Where(x => x.LoaiDVID == "GIAT").OrderBy("GiaBan", Order.DESC).Excute();



            Console.ReadKey();

        }
    }
}
