using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDotNetREST.TestClasses
{
    public abstract class TestAnimal
    {
        public abstract string Name { get; }
        public abstract double Cost { get; }
        public abstract bool IsDomesticated { get; }
        public virtual void Print()
        {
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Cost: " + Cost);
            Console.WriteLine("Is Domesticated: ");
        }
            
    }
}
