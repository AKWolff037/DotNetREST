using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetRESTUnitTest.Tests
{
    [TestClass]
    public class UnitTestRESTObject
    {
        [TestMethod]
        public void TestDefaultValues()
        {
            var defaultTestObject = TestRESTObject.CreateTestObject(false);
            var restObject = TestRESTObject.SerializeAndParseRESTObject(defaultTestObject);
            var convertedTestObject = restObject.ExplicitObject;
            TestRESTObject.AssertValidValuesForTestClass(convertedTestObject, false, false, false, false);

            var defaultTestObjectWithNulls = TestRESTObject.CreateTestObject(true);
            restObject = TestRESTObject.SerializeAndParseRESTObject(defaultTestObjectWithNulls);
            convertedTestObject = restObject.ExplicitObject;
            TestRESTObject.AssertValidValuesForTestClass(convertedTestObject, false, false, false, true);
        }

        [TestMethod]
        public void TestObjectWithArray()
        {
            var defaultTestObject = TestRESTObject.CreateTestObject(false);
            defaultTestObject.ChildArray = new TestRESTObject[] { TestRESTObject.CreateTestObject(false), TestRESTObject.CreateTestObject(false) };
            var restObject = TestRESTObject.SerializeAndParseRESTObject(defaultTestObject);
            var convertedTestObject = restObject.ExplicitObject;
            TestRESTObject.AssertValidValuesForTestClass(convertedTestObject, true, false, false, false);
        }
        [TestMethod]
        public void TestObjectWithList()
        {
            var defaultTestObject = TestRESTObject.CreateTestObject(true);
            defaultTestObject.ChildList = new List<TestRESTObject>() { TestRESTObject.CreateTestObject(true), TestRESTObject.CreateTestObject(true) };
            var restObject = TestRESTObject.SerializeAndParseRESTObject(defaultTestObject);
            var convertedTestObject = restObject.ExplicitObject;
            TestRESTObject.AssertValidValuesForTestClass(convertedTestObject, false, true, false, true);
        }
        [TestMethod]
        public void TestObjectWithListAndArray()
        {
            var defaultTestObject = TestRESTObject.CreateTestObject(false);
            defaultTestObject.ChildArray = new TestRESTObject[] { TestRESTObject.CreateTestObject(false), TestRESTObject.CreateTestObject(false) };
            defaultTestObject.ChildList = new List<TestRESTObject>() { TestRESTObject.CreateTestObject(false), TestRESTObject.CreateTestObject(false) };
            var restObject = TestRESTObject.SerializeAndParseRESTObject(defaultTestObject);
            var convertedTestObject = restObject.ExplicitObject;
            TestRESTObject.AssertValidValuesForTestClass(convertedTestObject, true, true, false, false);
        }
    }
}
