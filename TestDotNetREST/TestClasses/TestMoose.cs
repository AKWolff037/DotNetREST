using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDotNetREST.TestClasses
{
    public class TestMoose : TestAnimal
    {
        public override string Name { get { return "Moose"; } }
        public int HornLength { get; set; }
        public override double Cost { get { return 0.00; } }
        public override bool IsDomesticated
        {
            get { return false; }
        }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Horn Length: " + HornLength.ToString());
        }
    }

}
