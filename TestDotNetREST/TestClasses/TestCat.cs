using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDotNetREST.TestClasses
{
    public class TestCat : TestAnimal
    {
        public TestCat() : base()
        {
            Name = "Cat";
            Age = 8;
            Cost = 50.00m;
            IsDomesticated = true;
            NumberOfFeet = 4;
            NumberOfHairs = 54567890123;
            SomeByteValue = 0xfb;
            FloatingPointNumber = 18f;
            DoublePrecisionNumber = 2678.22412d;
            Class = 'C';
        }
    }
}
