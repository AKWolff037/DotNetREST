using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDotNetREST.TestClasses
{
    public class TestCat : TestAnimal
    {
        public override string Name { get { return "Cat"; } }
        public string Color { get; set; }
        public override double Cost { get { return 200.00; } }
        public override bool IsDomesticated
        {
            get { return true; }
        }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Color: " + Color);
        }
    }
}
