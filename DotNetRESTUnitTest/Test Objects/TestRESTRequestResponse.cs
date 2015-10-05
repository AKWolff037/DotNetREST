using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetREST;
using System.Text;
using Newtonsoft.Json;
namespace DotNetRESTUnitTest
{    
    public class TestRESTRequestResponse
    {
        public static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static string CreateString(params string[] parms)
        {
            var stringBuilder = new StringBuilder();
            foreach(string s in parms)
            {
                stringBuilder.Append(s);
            }
            return stringBuilder.ToString();
        }
        public static IRequest CreateTestHttpRequest(IResponse testResponse)
        {
            var request = new MockHttpRequest(testResponse);
            return request;
        }
        public static IResponse CreateTestHttpResponse(string json)
        {
            var request = new MockHttpResponse(json);
            return request;
        }
    }
}
