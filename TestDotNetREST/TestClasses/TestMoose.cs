using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDotNetREST.TestClasses
{
    public class TestMoose : TestAnimal
    {
        public TestMoose()
            : base()
        {
            Name = "Moose";
            Age = 18;
            Cost = 100000000.00m;
            IsDomesticated = false;
            NumberOfFeet = 4;
            NumberOfHairs = 7234567890123;
            SomeByteValue = 0xDE;
            FloatingPointNumber = 18.341f;
            DoublePrecisionNumber = 0.125d;
            Class = 'M';
        }
    }

}
