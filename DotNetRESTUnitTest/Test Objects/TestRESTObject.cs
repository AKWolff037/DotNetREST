using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using DotNetRest;

namespace DotNetRESTUnitTest
{
    public class TestRESTObject
    {
        public const string TEST_STRING = "Test";
        public const bool TEST_BOOL = true;
        public const int TEST_INT = int.MinValue;
        public const long TEST_LONG = long.MaxValue;
        public const byte TEST_BYTE = 0xFA;
        public const uint TEST_UINT = uint.MaxValue;
        public const float TEST_FLOAT = float.MaxValue;
        public const double TEST_DOUBLE = double.MaxValue;
        public const char TEST_CHAR = 'A';
        public static DateTime TEST_DATETIME = new DateTime(1987, 6, 3, 2, 34, 00, DateTimeKind.Utc);
        private static int _lastId = 0;

        public int ID { get; set; }
        public string TestStringValue { get; set; }
        public bool TestBoolValue { get; set; }
        public int TestIntValue { get; set; }
        public long TestLongValue { get; set; }
        public byte TestByteValue { get; set; }
        public uint TestUnsignedIntValue { get; set; }
        public float TestFloatValue { get; set; }
        public double TestDoubleValue { get; set; }
        public char TestCharValue { get; set; }
        public DateTime TestDateTimeValue { get; set; }

        public string TestNullableStringValue { get; set; }
        public bool? TestNullableBoolValue { get; set; }
        public int? TestNullableIntValue { get; set; }
        public long? TestNullableLongValue { get; set; }
        public byte? TestNullableByteValue { get; set; }
        public uint? TestNullableUIntValue { get; set; }
        public float? TestNullableFloatValue { get; set; }
        public double? TestNullableDoubleValue { get; set; }
        public char? TestNullableCharValue { get; set; }
        public DateTime? TestNullableDateTimeValue { get; set; }

        public TestRESTObject[] ChildArray { get; set; }
        public IList<TestRESTObject> ChildList { get; set; }

        public TestRESTObject()
        {

        }
        
        public static TestRESTObject CreateTestObject(bool populateNulls)
        {
            var testObj = new TestRESTObject();
            testObj.ID = ++_lastId;
            testObj.TestStringValue = TEST_STRING;
            testObj.TestBoolValue = TEST_BOOL;
            testObj.TestIntValue = TEST_INT;
            testObj.TestLongValue = TEST_LONG;
            testObj.TestByteValue = TEST_BYTE;
            testObj.TestFloatValue = TEST_FLOAT;
            testObj.TestUnsignedIntValue = TEST_UINT;
            testObj.TestDoubleValue = TEST_DOUBLE;
            testObj.TestCharValue = TEST_CHAR;
            testObj.TestDateTimeValue = TEST_DATETIME;
            if(populateNulls)
            {
                testObj.TestNullableStringValue = TEST_STRING;
                testObj.TestNullableBoolValue = TEST_BOOL;
                testObj.TestNullableIntValue = TEST_INT;
                testObj.TestNullableLongValue = TEST_LONG;
                testObj.TestNullableByteValue = TEST_BYTE;
                testObj.TestNullableFloatValue = TEST_FLOAT;
                testObj.TestNullableUIntValue = TEST_UINT;
                testObj.TestNullableDoubleValue = TEST_DOUBLE;
                testObj.TestNullableCharValue = TEST_CHAR;
                testObj.TestNullableDateTimeValue = TEST_DATETIME;
            }
            return testObj;
        }
        public static RestObject<TestRESTObject> SerializeAndParseRESTObject(TestRESTObject testObject)
        {
            var json = JsonConvert.SerializeObject(testObject);
            var restObject = new RestObject<TestRESTObject>(json);
            //Check to make sure that serialization was correct
            Assert.IsTrue(restObject.ExplicitObject != null, "Explicit Object was not correctly populated");
            return restObject;
        }
        public static void AssertValidValuesForTestClass(TestRESTObject convertedTestObject, bool checkChildArray, bool checkChildList, bool isChildCheck, bool areNullsPopulated)
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
            Assert.AreEqual(convertedTestObject.TestDateTimeValue, TEST_DATETIME);
            if(areNullsPopulated)
            {
                Assert.AreEqual(convertedTestObject.TestNullableStringValue, TEST_STRING);
                Assert.AreEqual(convertedTestObject.TestNullableBoolValue, TEST_BOOL);
                Assert.AreEqual(convertedTestObject.TestNullableIntValue, TEST_INT);
                Assert.AreEqual(convertedTestObject.TestNullableLongValue, TEST_LONG);
                Assert.AreEqual(convertedTestObject.TestNullableByteValue, TEST_BYTE);
                Assert.AreEqual(convertedTestObject.TestNullableFloatValue, TEST_FLOAT);
                Assert.AreEqual(convertedTestObject.TestNullableUIntValue, TEST_UINT);
                Assert.AreEqual(convertedTestObject.TestNullableDoubleValue, TEST_DOUBLE);
                Assert.AreEqual(convertedTestObject.TestNullableCharValue, TEST_CHAR);
                Assert.AreEqual(convertedTestObject.TestNullableDateTimeValue, TEST_DATETIME);
            }
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
                    AssertValidValuesForTestClass(childObject, false, false, true, areNullsPopulated);
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
                    AssertValidValuesForTestClass(childObject, false, false, true, areNullsPopulated);
                }
            }
        }
    }
}
