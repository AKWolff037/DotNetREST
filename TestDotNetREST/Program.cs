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
            moose.Name = "Moose 1";
            var moose2 = new TestMoose();
            moose2.Name = "Moose 2";
            var cat = new TestCat();
            cat.Name = "Skyler";
            var cat2 = new TestCat();
            cat2.Name = "Fluffy Butt";
            var dog = new TestDog();
            dog.Name = "Daisy";
            var dog2 = new TestDog();
            dog2.Name = "Shadow";

            testShop.Animals.Add(moose);
            testShop.Animals.Add(moose2);
            testShop.Animals.Add(cat);
            testShop.Animals.Add(cat2);
            testShop.Animals.Add(dog);
            testShop.Animals.Add(dog2);
            testShop.AnimalArray = new TestAnimal[] { moose, cat, dog };
            var json = JsonConvert.SerializeObject(testShop);
            var obj = new RESTObject<TestAnimalShop>(json);
            foreach(var animal in obj.ExplicitObject.Animals)
            {
                animal.Print();
            }
            for (int i = 0; i < obj.ExplicitObject.AnimalArray.Count(); i++)
            {
                obj.ExplicitObject.AnimalArray[i].Print();
            }

            Console.ReadLine();
        }
    }
}
