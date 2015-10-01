using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetREST;
using System.Text;
using Newtonsoft.Json;
namespace DotNetRESTUnitTest
{
    [TestClass]
    public class TestRESTWebRequest
    {
        [TestMethod]
        public void TestRequestAndResponse()
        {
            //Just to make it more readable
            var quote = "\"";
            var openBracket = "{";
            var closeBracket = "}";
            var comma = ",";
            var colon = ":";
            var crlf = "";
            var json = CreateString(openBracket, crlf,
                                    quote, "TestStringValue", quote, colon, quote, TestRESTObject.TEST_STRING, quote, comma, crlf,
                                    quote, "TestBoolValue", quote, colon, TestRESTObject.TEST_BOOL.ToString().ToLower(), comma, crlf,
                                    quote, "TestIntValue", quote, colon, TestRESTObject.TEST_INT.ToString(), comma, crlf,
                                    quote, "TestLongValue", quote, colon, TestRESTObject.TEST_LONG.ToString(), comma, crlf,
                                    quote, "TestByteValue", quote, colon, TestRESTObject.TEST_BYTE.ToString(), comma, crlf,
                                    quote, "TestUnsignedIntValue", quote, colon, TestRESTObject.TEST_UINT.ToString(), comma, crlf,
                                    quote, "TestFloatValue", quote, colon, TestRESTObject.TEST_FLOAT.ToString(), comma, crlf,
                                    quote, "TestDoubleValue", quote, colon, TestRESTObject.TEST_DOUBLE.ToString(), comma, crlf,
                                    quote, "TestCharValue", quote, colon, quote, TestRESTObject.TEST_CHAR.ToString(), quote, comma, crlf,
                                    quote, "TestDateTime", quote, colon, TestRESTObject.TEST_DATETIME.ToString(), comma, crlf,
                                    quote, "ChildArray", quote, colon, "null", comma, crlf,
                                    quote, "ChildList", quote, colon, "null", crlf,
                                    closeBracket
                                   );

            Assert.IsTrue(json != null);
            
            var request = CreateTestHttpRequest(CreateTestHttpResponse(json));
            request.Method = "GET";
            Assert.IsNotNull(request);
            var testRequest = new RESTWebRequest(request);
            Assert.IsNotNull(testRequest);
            var testResponse = testRequest.GetRESTResponse();
            Assert.IsNotNull(testResponse);
            
            var testObject = new RESTObject<TestRESTObject>(testResponse);
            Assert.IsNotNull(testObject);            
            var testExplicit = testObject.ExplicitObject;
            Assert.IsNotNull(testExplicit);
            TestRESTObject.AssertValidValuesForTestClass(testExplicit, false, false, false);
        }
        private static string CreateString(params string[] parms)
        {
            var stringBuilder = new StringBuilder();
            foreach(string s in parms)
            {
                stringBuilder.Append(s);
            }
            return stringBuilder.ToString();
        }
        private static IRequest CreateTestHttpRequest(IResponse testResponse)
        {
            var request = new MockHttpRequest(testResponse);
            return request;
        }
        private static IResponse CreateTestHttpResponse(string json)
        {
            var request = new MockHttpResponse(json);
            return request;
        }
    }
}
