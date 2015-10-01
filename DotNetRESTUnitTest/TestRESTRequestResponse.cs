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
                                    quote, "TestStringValue", quote, colon, quote, "Test", quote, comma, crlf,
                                    quote, "TestBoolValue", quote, colon, "true", comma, crlf,
                                    quote, "TestIntValue", quote, colon, "-2147483648", comma, crlf,
                                    quote, "TestLongValue", quote, colon, "9223372036854775807", comma, crlf,
                                    quote, "TestByteValue", quote, colon, "250", comma, crlf,
                                    quote, "TestUnsignedIntValue", quote, colon, "4294967295", comma, crlf,
                                    quote, "TestFloatValue", quote, colon, "3.40282347E+38", comma, crlf,
                                    quote, "TestDoubleValue", quote, colon, "1.7976931348623157E+308", comma, crlf,
                                    quote, "TestCharValue", quote, colon, quote, "A", quote, comma, crlf,
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
