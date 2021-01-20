
using Models.POCO;
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
            // Tạo Db Context
            var a = new Test("MySqlConnectionString");
            // tạo Dbset<Staff>
            var b = a.Set<Staff>();
            //var test1 = b.Where(x => x.Account_id == 1).Excute();
            // Kiếm tra join được với Account không?
            var t = b.CanJoin<Staff>();
            //var p = a.Set<Account>();
            //var i = p.CanJoin<Staff>();
            // Join Account với forced =true (ép join))
            var op = a.Set<Account>();
            var account_list = (IEnumerable<Account>)op.Join<Staff>(Tuple.Create("ID", "Account_id"), true);

            // khởi tạo một đôi tương staff để insert
            var item = new Staff();
            item.Email = "Sangdoan956@gmail.com";
            item.Account_id = 1;
            item.IdentityCard = "215483062";
            item.Phone = "0589005648";
            item.Name = "123aaaa";
            item.Address = "bbb23123bbb";
            // Insert staff()
            Console.WriteLine(b.Insert(item));
            // tạo object cho việc update
            var item1 = new { Email = "Doansang789@gmail.com" };
            // Update với điều kiên là lamdaExpression
            var y = b.Update(x => x.ID == 1000, item);
            var d = b.Update(x => x.ID == 10 || x.Account_id == 15, item);
            // Xóa một phần tử với điều kiên là lamdaExpression
            var z = b.Delete(x => x.ID == 1014);
            // OrderBy
            var g8 = b.OrderBy("ID", Order.DESC).Excute();
            //Lọc rồi orderby
            var g = b.Where(x => x.Account_id == 1).OrderBy("ID", Order.DESC).Excute();
            var qqq = b.GroupBy("Account_id").Having("SUM(ID)>4000").Select("Name, Account_id").Run();



            ////****MS SQL
            //var a = new Test("SqlConnectionString");
            //var b = a.Set<LOAIDV>();

            ////Insert
            //var item = new LOAIDV();
            //item.LoaiDVID = "N224442";
            //item.TenLoai = "Chăm sóc";
            //item.NgayTao = DateTime.Now;
            //Console.WriteLine(b.Insert(item));

            ////JOIN
            //var t = b.CanJoin<DICHVU>();
            //var c = b.Join<DICHVU>(Tuple.Create("LoaiDVID", "LoaiDVID"), true);

            ////DELETE
            //var z = b.Delete(x => x.LoaiDVID == "N224442");

            ////UPDATE
            //var item1 = new { TenLoai = "Loại Dịch vụ Giặt ủi" };
            //var y = b.Update(x => x.LoaiDVID == "GIAT", item1);

            ////ORDER
            //var d = a.Set<DICHVU>();
            //var g = d.Where(x => x.LoaiDVID == "GIAT").OrderBy("GiaBan", Order.DESC).Excute();

            ////***POSTGRES
            //var aa = new Test("PostgreSqlQuyen");
            //var bb = aa.Set<Loaidichvu>();

            ////Insert
            //var item = new Loaidichvu();
            //item.tenloai = "Chăm sóc";
            //Console.WriteLine(bb.Insert(item));

            ////JOIN
            //var tt = bb.CanJoin<Dichvu>();
            //var cc = bb.Join<Dichvu>(Tuple.Create("maloai", "maloai"), true);

            ////DELETE
            //var z = bb.Delete(x => x.maloai == 4);

            ////UPDATE
            //var item1 = new { tenloai = "do an ngon" };
            //var y = bb.Update(x => x.maloai == 1, item1);

            ////ORDER
            //var d = aa.Set<Dichvu>();
            //var g = d.Where(x => x.maloai == 1).OrderBy("gia", Order.DESC).Excute();

            Console.ReadKey();

        }
    }
}
