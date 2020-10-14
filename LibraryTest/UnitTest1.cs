using CS_Money;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace LibraryTest
{
    [TestClass]
    public class UnitTest1
    {
        //GetCSGOItems
        [TestMethod]
        public void ÑomplianceTest()
        {
            //"44":{"u":"sbk","m":"Sealed Graffiti | Still Happy (War Pig Pink)","a":0,"g":14,"h":7,"j":"Çàïå÷àòàííûé ãðàôôèòè | Ñ÷àñòüå (Òðàíøåéíûé ðîçîâûé)"}
            string k = null;
            string k2 = "Sealed Graffiti | Still Happy (War Pig Pink)";
            var a = Items.GetCSGOItems();
            foreach (var item2 in a)
            {
                if (item2.Value.U == "sbk")
                {
                    k = item2.Value.M;
                }
            }
            Assert.AreEqual(k, k2);
        }
        [TestMethod]
        public void NullTest()
        {
            var a = Items.GetCSGOItems();
            Assert.IsNotNull(a);
        }
        //GetCSMItems
        [TestMethod]
        public void NullTest2()
        {
            var a = Items.GetCSMItems();
            //var item = a.Where(x => x.O == 1).FirstOrDefault();
            Assert.IsNotNull(a);
        }
        //GetCSMItemById
        [TestMethod]
        public void StabilityTest()
        {
            List<CSMSkinsOnSale> lt = new List<CSMSkinsOnSale>();
            var a = Items.GetCSMItemById(1);
            Assert.IsNotNull(a);
            foreach (var t in a)
            {
                //db.context.Table.Add(...)
                //Console.Wwritline(t.O);
                if (t.O == 1)
                {
                    long y = t.O;
                    lt.Add(new CSMSkinsOnSale { P = t.P, O = t.O });
                    //return View(lt);
                }
            }
        }
        //GetSalesHistoryItem
        [TestMethod]
        public void StabilityTest2()
        {
            var a = Items.GetSalesHistoryItem(1);
            Assert.IsNotNull(a);
            foreach (var item in a)
            {
                var i = item.custom_price;
                var o = item.floatvalue;
                //...
            }
        }
        //FoundItem
        [TestMethod]
        public void StabilityTest3()
        {
            var a = Items.FoundItem("  ");
            Assert.IsNull(a.Name);
            var a1 = Items.FoundItem("AUG | Condemned (Field-Tested)");
            Assert.IsNotNull(a1);
            var b = Items.FoundItem(10);
            Assert.IsNotNull(b);
            Assert.AreEqual(a1.Name, b.Name);
            //Console.WriteLine(a.Name, a.Price);
            //Console.WriteLine(b.Name, b.Price);
        }
    }
}
