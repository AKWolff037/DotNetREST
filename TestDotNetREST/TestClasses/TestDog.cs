using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDotNetREST.TestClasses
{
    public class TestDog : TestAnimal
    {
        public override string Name { get { return "Dog"; } }
        public string Breed { get; set; }
        public override double Cost { get { return 350.00; } }
        public override bool IsDomesticated
        {
            get { return true; }
        }
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Breed: " + Breed);
        }
    }
}
