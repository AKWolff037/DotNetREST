using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetRest;

namespace DotNetRESTUnitTest.Tests
{
    [TestClass]
    public class UnitTestRESTRequestResponse
    {
        [TestMethod]
        public void TestRequestAndResponseWithNulls()
        {
            //Just to make it more readable
            var quote = "\"";
            var openBracket = "{";
            var closeBracket = "}";
            var comma = ",";
            var colon = ":";
            var crlf = "";
            var json = TestRESTRequestResponse.CreateString(openBracket, crlf,
                                    quote, "TestStringValue", quote, colon, quote, TestRESTObject.TEST_STRING, quote, comma, crlf,
                                    quote, "TestBoolValue", quote, colon, TestRESTObject.TEST_BOOL.ToString().ToLower(), comma, crlf,
                                    quote, "TestIntValue", quote, colon, TestRESTObject.TEST_INT.ToString(), comma, crlf,
                                    quote, "TestLongValue", quote, colon, TestRESTObject.TEST_LONG.ToString(), comma, crlf,
                                    quote, "TestByteValue", quote, colon, TestRESTObject.TEST_BYTE.ToString(), comma, crlf,
                                    quote, "TestUnsignedIntValue", quote, colon, TestRESTObject.TEST_UINT.ToString(), comma, crlf,
                                    quote, "TestFloatValue", quote, colon, TestRESTObject.TEST_FLOAT.ToString("R"), comma, crlf,
                                    quote, "TestDoubleValue", quote, colon, TestRESTObject.TEST_DOUBLE.ToString("R"), comma, crlf,
                                    quote, "TestCharValue", quote, colon, quote, TestRESTObject.TEST_CHAR.ToString(), quote, comma, crlf,
                                    quote, "TestDateTimeValue", quote, colon, TestRESTObject.TEST_DATETIME.Subtract(TestRESTRequestResponse.UNIX_EPOCH).TotalSeconds.ToString(), comma, crlf,
                                    quote, "ChildArray", quote, colon, "null", comma, crlf,
                                    quote, "ChildList", quote, colon, "null", crlf,
                                    closeBracket
                                   );

            Assert.IsTrue(json != null);
            // {"TestStringValue":"Test","TestBoolValue":true,"TestIntValue":-2147483648,"TestLongValue":9223372036854775807,"TestByteValue":250,"TestUnsignedIntValue":4294967295,"TestFloatValue":3.40282347E+38,"TestDoubleValue":1.7976931348623157E+308,"TestCharValue":"A","TestDateTime":6/3/1987 2:34:00 AM,"ChildArray":null,"ChildList":null}
            // {"TestStringValue":"Test","TestBoolValue":true,"TestIntValue":-2147483648,"TestLongValue":9223372036854775807,"TestByteValue":250,"TestUnsignedIntValue":4294967295,"TestFloatValue":3.40282347E+38,"TestDoubleValue":1.7976931348623157E+308,"TestCharValue":"A","TestDateTimeValue":"1987-06-03T02:34:00","TestNullableStringValue":"Test","TestNullableBoolValue":true,"TestNullableIntValue":-2147483648,"TestNullableLongValue":9223372036854775807,"TestNullableByteValue":250,"TestNullableUIntValue":4294967295,"TestNullableFloatValue":3.40282347E+38,"TestNullableDoubleValue":1.7976931348623157E+308,"TestNullableCharValue":"A","TestNullableDateTimeValue":"1987-06-03T02:34:00","ChildArray":null,"ChildList":null}
            var request = TestRESTRequestResponse.CreateTestHttpRequest(TestRESTRequestResponse.CreateTestHttpResponse(json));
            request.Method = "GET";
            Assert.IsNotNull(request);
            var testRequest = new RestWebRequest(request);
            Assert.IsNotNull(testRequest);
            var testResponse = testRequest.GetRestResponse();
            Assert.IsNotNull(testResponse);

            var testObject = new RestObject<TestRESTObject>(testResponse);
            Assert.IsNotNull(testObject);
            var testExplicit = testObject.ExplicitObject;
            Assert.IsNotNull(testExplicit);
            TestRESTObject.AssertValidValuesForTestClass(testExplicit, false, false, false, false);
        }
        [TestMethod]
        public void TestRequestAndResponseWithoutNulls()
        {
            //Just to make it more readable
            var quote = "\"";
            var openBracket = "{";
            var closeBracket = "}";
            var comma = ",";
            var colon = ":";
            var crlf = "";
            // "R" formatting preserves the most precision for float/double
            var json = TestRESTRequestResponse.CreateString(openBracket, crlf,
                                    quote, "TestStringValue", quote, colon, quote, TestRESTObject.TEST_STRING, quote, comma, crlf,
                                    quote, "TestBoolValue", quote, colon, TestRESTObject.TEST_BOOL.ToString().ToLower(), comma, crlf,
                                    quote, "TestIntValue", quote, colon, TestRESTObject.TEST_INT.ToString(), comma, crlf,
                                    quote, "TestLongValue", quote, colon, TestRESTObject.TEST_LONG.ToString(), comma, crlf,
                                    quote, "TestByteValue", quote, colon, TestRESTObject.TEST_BYTE.ToString(), comma, crlf,
                                    quote, "TestUnsignedIntValue", quote, colon, TestRESTObject.TEST_UINT.ToString(), comma, crlf,
                                    quote, "TestFloatValue", quote, colon, TestRESTObject.TEST_FLOAT.ToString("R"), comma, crlf,
                                    quote, "TestDoubleValue", quote, colon, TestRESTObject.TEST_DOUBLE.ToString("R"), comma, crlf,
                                    quote, "TestCharValue", quote, colon, quote, TestRESTObject.TEST_CHAR.ToString(), quote, comma, crlf,
                                    quote, "TestDateTimeValue", quote, colon, TestRESTObject.TEST_DATETIME.Subtract(TestRESTRequestResponse.UNIX_EPOCH).TotalSeconds.ToString(), comma, crlf,
                                    quote, "TestNullableStringValue", quote, colon, quote, TestRESTObject.TEST_STRING, quote, comma, crlf,
                                    quote, "TestNullableBoolValue", quote, colon, TestRESTObject.TEST_BOOL.ToString().ToLower(), comma, crlf,
                                    quote, "TestNullableIntValue", quote, colon, TestRESTObject.TEST_INT.ToString(), comma, crlf,
                                    quote, "TestNullableLongValue", quote, colon, TestRESTObject.TEST_LONG.ToString(), comma, crlf,
                                    quote, "TestNullableByteValue", quote, colon, TestRESTObject.TEST_BYTE.ToString(), comma, crlf,
                                    quote, "TestNullableUIntValue", quote, colon, TestRESTObject.TEST_UINT.ToString(), comma, crlf,
                                    quote, "TestNullableFloatValue", quote, colon, TestRESTObject.TEST_FLOAT.ToString("R"), comma, crlf,
                                    quote, "TestNullableDoubleValue", quote, colon, TestRESTObject.TEST_DOUBLE.ToString("R"), comma, crlf,
                                    quote, "TestNullableCharValue", quote, colon, quote, TestRESTObject.TEST_CHAR.ToString(), quote, comma, crlf,
                                    quote, "TestNullableDateTimeValue", quote, colon, TestRESTObject.TEST_DATETIME.Subtract(TestRESTRequestResponse.UNIX_EPOCH).TotalSeconds.ToString(), comma, crlf,
                                    quote, "ChildArray", quote, colon, "null", comma, crlf,
                                    quote, "ChildList", quote, colon, "null", crlf,
                                    closeBracket
                                   );

            Assert.IsTrue(json != null);
            var request = TestRESTRequestResponse.CreateTestHttpRequest(TestRESTRequestResponse.CreateTestHttpResponse(json));
            request.Method = "GET";
            Assert.IsNotNull(request);
            var testRequest = new RestWebRequest(request);
            Assert.IsNotNull(testRequest);
            var testResponse = testRequest.GetRestResponse();
            Assert.IsNotNull(testResponse);

            var testObject = new RestObject<TestRESTObject>(testResponse);
            Assert.IsNotNull(testObject);
            var testExplicit = testObject.ExplicitObject;
            Assert.IsNotNull(testExplicit);
            TestRESTObject.AssertValidValuesForTestClass(testExplicit, false, false, false, true);
        }
    }
}
