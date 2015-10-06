using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetRest;
using System.IO;
using System.Net;
namespace DotNetRESTUnitTest
{
    public class MockHttpResponse : IResponse
    {
        public string MockJsonResponse { get; set; }
        public Uri ResponseUri { get { return new Uri("http://localhost:8080"); } }
        public bool IsMutuallyAuthenticated { get { return true; } }
        public bool IsFromCache { get { return true; } }
        public bool SupportsHeaders { get { return false; } }
        public WebHeaderCollection Headers { get { return null; } }
        public string ContentType { get; set; }
        public long ContentLength { get { return MockJsonResponse.Length; } }

        public MockHttpResponse()
        {
            MockJsonResponse = "";
        }
        public MockHttpResponse(string json)
        {
            MockJsonResponse = json;
        }
        private void InitResponse()
        {
            ContentType = "text/json";
        }
        public Stream GetStream()
        {
            var bytes = Encoding.UTF8.GetBytes(MockJsonResponse);
            var mockStream = new MemoryStream(bytes);
            return mockStream;
        }
        public async Task<Stream> GetStreamAsync()
        {
            return GetStream();
        }
        public void Close()
        {
            return;
        }
    }

}
