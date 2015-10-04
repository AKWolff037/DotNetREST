using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DotNetREST;
using System.Net;
namespace DotNetRESTUnitTest
{
    public class MockHttpRequest : IRequest
    {
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
        public IResponse TestResponse { get; set; }
        public string Method { get; set; }
        public System.Net.Security.AuthenticationLevel AuthenticationLevel { get; set; }
        public ICredentials Credentials { get; set; }
        public int Timeout { get; set; }
        private WebHeaderCollection _headers;
        public WebHeaderCollection Headers { get { return _headers; } }
        public IWebProxy Proxy { get; set; }
        public Stream GetRequestStream()
        {
            return new MemoryStream();
        }
        public async Task<Stream> GetRequestStreamAsync()
        {
            return GetRequestStream();
        }
        public MockHttpRequest()
        {
            TestResponse = new MockHttpResponse("\"value\":\"string\"");
            InitDefaults();
        }
        public MockHttpRequest(IResponse response)
        {
            TestResponse = response;
            InitDefaults();
        }
        private void InitDefaults()
        {
            AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
            Credentials = new System.Net.NetworkCredential("Test", "Test");
            Timeout = 1000;
            _headers = new WebHeaderCollection();
            Proxy = null;
            ContentType = "application/json";
            ContentLength = 0;
        }
        public IResponse GetResponse()
        {
            return TestResponse;
        }
        public async Task<IResponse> GetResponseAsync()
        {
            return GetResponse();
        }

        public Uri RequestUri
        {
            get { return new Uri("http://locahost:8080/api/"); }
        }
    }
}
