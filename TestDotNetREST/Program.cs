using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetREST;
using Newtonsoft.Json;
using TestDotNetREST.TestClasses;
namespace TestDotNetREST
{
    class Program
    {
        static void Main(string[] args)
        {
            var testShop = new TestAnimalShop();
            var moose = new TestMoose();
            moose.HornLength = 10;
            var moose2 = new TestMoose();
            moose.HornLength = 20;
            var cat = new TestCat();
            cat.Color = "Orange";
            var cat2 = new TestCat();
            cat2.Color = "Black";
            var dog = new TestDog();
            dog.Breed = "Black Labrador";
            var dog2 = new TestDog();
            dog2.Breed = "Border Collie";

            testShop.Animals.Add(moose);
            testShop.Animals.Add(moose2);
            testShop.Animals.Add(cat);
            testShop.Animals.Add(cat2);
            testShop.Animals.Add(dog);
            testShop.Animals.Add(dog2);

            var json = JsonConvert.SerializeObject(testShop);
            var obj = new RESTObject<TestAnimalShop>(json);
            foreach(var animal in obj.ExplicitObject.Animals)
            {
                animal.Print();
            }
            //var testobj = new TestObject()
            //{
            //    name = "Test",
            //    date = DateTime.Now,
            //    integer = 8,
            //    longvalue = 1292,
            //    children = new TestObject[]
            //    {
            //        new TestObject() 
            //        {
            //            name = "Child 1",
            //            date = DateTime.Now,
            //            integer = 2,
            //            longvalue = 1,
            //            children = null
            //        },
            //        new TestObject()
            //        {
            //            name = "Child 2",
            //            date = DateTime.Now,
            //            integer = 1191,
            //            longvalue = -111,
            //            children = new TestObject[]
            //            {
            //                new TestObject()
            //                {
            //                    name = "Child of Child 2",
            //                    date = DateTime.Now.AddYears(20),
            //                    integer = 22,
            //                    longvalue = -37,
            //                    children = null
            //                }
            //            }
            //        }
            //    }
            //};
            //var testobj2 = new TestObject2()
            //{
            //    department = "Department of Testportation",
            //    owner = testobj,
            //    children = new TestObject[] 
            //    { 
            //        new TestObject()
            //        {
            //            name = "Child 3",
            //            date = DateTime.Now.AddDays(-5),
            //            integer = -3,
            //            longvalue = 1234567890,
            //            children = null
            //        }
            //    },
            //    subchildren = new TestObject2[] 
            //    { 
            //        new TestObject2()
            //        {
            //            department = "Dept of motor testing",
            //            owner = testobj,
            //            children = null,
            //            subchildren = null
            //        }

            //    },
            //    idiot = new TestObject3()
            //    {
            //        insult = "fuck you"
            //    }
            //};
            //var serial = JsonConvert.SerializeObject(testobj);
            //var serial2 = JsonConvert.SerializeObject(testobj2);

            //var restobj = new RESTObject<TestObject>(serial);
            //var restobj2 = new RESTObject<TestObject2>(serial2);

            //restobj.ExplicitObject.print("");
            //restobj2.ExplicitObject.print("");

            Console.ReadLine();
        }
    }
    public class TestObject2
    {
        public string department { get; set; }
        public TestObject owner { get; set; }
        public TestObject[] children { get; set; }
        public TestObject2[] subchildren { get; set; }
        public TestObject3 idiot { get; set; }
        public void print(string leadingString)
        {
            Console.WriteLine(leadingString + "My department is : " + department);
            if (owner != null)
            {
                Console.WriteLine(leadingString + "My owner is : ");
                owner.print(leadingString);
            }            
            Console.WriteLine(leadingString + "My children are: ");
            if (children != null && children.Count() > 0)
            {
                foreach (var child in children)
                {
                    child.print(leadingString + "\t");
                }
            }
            else
            {
                Console.WriteLine(leadingString + "Just kidding, no children...");
            }
            Console.WriteLine(leadingString + "My subchildren are: ");
            if (subchildren != null && subchildren.Count() > 0)
            {
                foreach (var child in subchildren)
                {
                    child.print(leadingString + "\t");
                }
            }
            else
            {
                Console.WriteLine(leadingString + "Just kidding, no subchildren...");
            }
            if(idiot != null)
            {
                Console.WriteLine(leadingString + "The idiot says: ");
                idiot.print(leadingString);
            }
        }
    }
    public class TestObject
    {
        public string name { get; set; }
        public DateTime date { get; set; }
        public int integer { get; set; }
        public long longvalue { get; set; }
        public TestObject[] children { get; set; }        

        public TestObject()
        {

        }

        public void print(string leadingString)
        {
            Console.WriteLine(leadingString + "My name is : " + name);
            Console.WriteLine(leadingString + "My date is : " + date.ToLongDateString());
            Console.WriteLine(leadingString + "My int is : " + integer.ToString());
            Console.WriteLine(leadingString + "My long is : " + longvalue.ToString());
            Console.WriteLine(leadingString + "My children are: ");
            if (children != null && children.Count() > 0)
            {
                foreach (var child in children)
                {
                    child.print(leadingString + "\t");
                }
            }
            else
            {
                Console.WriteLine(leadingString + "Just kidding, no children...");
            }
        }
    }
    public class TestObject3
    {
        public string insult { get; set; }
        public void print(string leadingString)
        {
            Console.WriteLine(leadingString + insult);
        }
    }
}
