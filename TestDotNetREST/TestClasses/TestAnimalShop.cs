using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestDotNetREST.TestClasses
{
    public class TestAnimalShop
    {
        public List<TestAnimal> Animals { get; set; }
        public TestAnimal[] AnimalArray { get; set; }

        public TestAnimalShop()
        {
            Animals = new List<TestAnimal>();
            AnimalArray = new TestAnimal[] { };
        }
    }
}
