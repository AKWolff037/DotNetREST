using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDotNetREST.TestClasses
{
    public class TestDog : TestAnimal
    {
        public TestDog() : base()
        {
            Name = "Dog";
            Age = 7;
            Cost = 100.00m;
            IsDomesticated = true;
            NumberOfFeet = 4;
            NumberOfHairs = 1234567890123;
            SomeByteValue = 0xab;
            FloatingPointNumber = 3f;
            DoublePrecisionNumber = 18d;
            Class = 'A';
        }
    }
}