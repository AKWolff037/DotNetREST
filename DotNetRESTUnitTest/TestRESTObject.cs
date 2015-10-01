using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using DotNetREST;

namespace DotNetRESTUnitTest
{
    [TestClass]
    public class TestRESTObject
    {
        const string TEST_STRING = "Test";
        const bool TEST_BOOL = true;
        const int TEST_INT = int.MinValue;
        const long TEST_LONG = long.MaxValue;
        const byte TEST_BYTE = 0xFA;
        const uint TEST_UINT = uint.MaxValue;
        const float TEST_FLOAT = float.MaxValue;
        const double TEST_DOUBLE = double.MaxValue;
        const char TEST_CHAR = 'A';

        public string TestStringValue { get; set; }
        public bool TestBoolValue { get; set; }
        public int TestIntValue { get; set; }
        public long TestLongValue { get; set; }
        public byte TestByteValue { get; set; }
        public uint TestUnsignedIntValue { get; set; }
        public float TestFloatValue { get; set; }
        public double TestDoubleValue { get; set; }
        public char TestCharValue { get; set; }
        public TestRESTObject[] ChildArray { get; set; }
        public IList<TestRESTObject> ChildList { get; set; }

        public TestRESTObject()
        {

        }
        

        [TestMethod]
        public void TestDefaultValues()
        {
            var defaultTestObject = CreateTestObject();
            var restObject = SerializeAndParseRESTObject(defaultTestObject);
            var convertedTestObject = restObject.ExplicitObject;
            AssertValidValuesForTestClass(convertedTestObject, false, false, false);
        }

        [TestMethod]
        public void TestObjectWithArray()
        {
            var defaultTestObject = CreateTestObject();
            defaultTestObject.ChildArray = new TestRESTObject[] { CreateTestObject(), CreateTestObject() };
            var restObject = SerializeAndParseRESTObject(defaultTestObject);
            var convertedTestObject = restObject.ExplicitObject;
            AssertValidValuesForTestClass(convertedTestObject, true, false, false);
        }
        [TestMethod]
        public void TestObjectWithList()
        {
            var defaultTestObject = CreateTestObject();
            defaultTestObject.ChildList = new List<TestRESTObject>() { CreateTestObject(), CreateTestObject() };
            var restObject = SerializeAndParseRESTObject(defaultTestObject);
            var convertedTestObject = restObject.ExplicitObject;
            AssertValidValuesForTestClass(convertedTestObject, false, true, false);
        }
        [TestMethod]
        public void TestObjectWithListAndArray()
        {
            var defaultTestObject = CreateTestObject();
            defaultTestObject.ChildArray = new TestRESTObject[] { CreateTestObject(), CreateTestObject() };
            defaultTestObject.ChildList = new List<TestRESTObject>() { CreateTestObject(), CreateTestObject() };
            var restObject = SerializeAndParseRESTObject(defaultTestObject);
            var convertedTestObject = restObject.ExplicitObject;
            AssertValidValuesForTestClass(convertedTestObject, true, true, false);
        }
        private static TestRESTObject CreateTestObject()
        {
            var testObj = new TestRESTObject();
            testObj.TestStringValue = TEST_STRING;
            testObj.TestBoolValue = TEST_BOOL;
            testObj.TestIntValue = TEST_INT;
            testObj.TestLongValue = TEST_LONG;
            testObj.TestByteValue = TEST_BYTE;
            testObj.TestFloatValue = TEST_FLOAT;
            testObj.TestUnsignedIntValue = TEST_UINT;
            testObj.TestDoubleValue = TEST_DOUBLE;
            testObj.TestCharValue = TEST_CHAR;
            return testObj;
        }
        private static RESTObject<TestRESTObject> SerializeAndParseRESTObject(TestRESTObject testObject)
        {
            var json = JsonConvert.SerializeObject(testObject);
            var restObject = new RESTObject<TestRESTObject>(json);
            //Check to make sure that serialization was correct
            Assert.IsTrue(restObject.ExplicitObject != null, "Explicit Object was not correctly populated");
            return restObject;
        }
        public static void AssertValidValuesForTestClass(TestRESTObject convertedTestObject, bool checkChildArray, bool checkChildList, bool isChildCheck)
        {
            //Check to make sure that all values are correct
            Assert.AreEqual(convertedTestObject.TestStringValue, TEST_STRING);
            Assert.AreEqual(convertedTestObject.TestBoolValue, TEST_BOOL);
            Assert.AreEqual(convertedTestObject.TestIntValue, TEST_INT);
            Assert.AreEqual(convertedTestObject.TestLongValue, TEST_LONG);
            Assert.AreEqual(convertedTestObject.TestByteValue, TEST_BYTE);
            Assert.AreEqual(convertedTestObject.TestUnsignedIntValue, TEST_UINT);
            Assert.AreEqual(convertedTestObject.TestFloatValue, TEST_FLOAT);
            Assert.AreEqual(convertedTestObject.TestDoubleValue, TEST_DOUBLE);
            Assert.AreEqual(convertedTestObject.TestCharValue, TEST_CHAR);
            if (!checkChildArray || isChildCheck)
            {
                Assert.IsNull(convertedTestObject.ChildArray);
            }
            else
            {
                Assert.IsNotNull(convertedTestObject.ChildArray);
                Assert.IsTrue(convertedTestObject.ChildArray.Length > 0);
                for (int i = 0; i < convertedTestObject.ChildArray.Length; i++)
                {
                    var childObject = convertedTestObject.ChildArray[i];
                    AssertValidValuesForTestClass(childObject, false, false, true);
                }
            }
            if (!checkChildList || isChildCheck)
            {
                Assert.IsNull(convertedTestObject.ChildList);
            }
            else
            {
                Assert.IsNotNull(convertedTestObject.ChildList);
                Assert.IsTrue(convertedTestObject.ChildList.Count > 0);
                foreach (var childObject in convertedTestObject.ChildList)
                {
                    AssertValidValuesForTestClass(childObject, false, false, true);
                }
            }
        }
    }
}
