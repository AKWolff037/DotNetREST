using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDotNetREST.TestClasses
{
    public class TestAnimal
    {
        public virtual string Name { get; set; }
        public virtual decimal Cost { get; set; }
        public virtual bool IsDomesticated { get; set; }
        public virtual int NumberOfFeet { get; set; }
        public virtual long NumberOfHairs { get; set; }
        public virtual byte SomeByteValue { get; set; }
        public virtual uint Age { get; set; }
        public virtual float FloatingPointNumber { get; set; }
        public virtual double DoublePrecisionNumber { get; set; }
        public virtual char Class { get; set; }
        public virtual void Print()
        {
            Console.WriteLine("Animal: " + this.GetType().ToString());
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Cost: " + Cost);
            Console.WriteLine("Is Domesticated: " + IsDomesticated);
            Console.WriteLine("Number of Feet: " + NumberOfFeet);
            Console.WriteLine("Number of Hairs: " + NumberOfHairs);
            Console.WriteLine("Class: " + Class);
            Console.WriteLine("Age: " + Age);
            Console.WriteLine("Floating Number: " + FloatingPointNumber);
            Console.WriteLine("Double Value: " + DoublePrecisionNumber);
            Console.WriteLine("Byte Value: " + SomeByteValue);
        }
            
    }
}
