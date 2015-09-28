using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace DotNetREST
{
    public class RESTWebRequest
    {
        public WebRequest Base
        {
            get { return _baseRequest; }
        }
        private WebRequest _baseRequest;
        private RESTWebRequest() : base()
        {
            _baseRequest = HttpWebRequest.Create("http://localhost");
        }
        public RESTWebRequest(Uri uri, HttpVerb verb)
        {
            _baseRequest = WebRequest.Create(uri);
            _baseRequest.Method = verb.ToString();
        }
        public RESTWebRequest(string uri, HttpVerb verb)
        {
            _baseRequest = WebRequest.Create(uri);
            _baseRequest.Method = verb.ToString();
        }

    }
    public enum HttpVerb
    {
        GET,
        PUT,
        POST,
        DELETE
    }
}
