using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace DotNetREST
{
    public class RESTWebRequest
    {
        public Dictionary<string, string> HeaderParameters { get; set; }
        public IRequest Base
        {
            get { return _baseRequest; }
        }
        private IRequest _baseRequest;
        private RESTWebRequest()
        {
            _baseRequest = (IRequest)HttpWebRequest.Create("http://localhost:8080");
            InitRequest();
        }
        public RESTWebRequest(IRequest request)
        {
            _baseRequest = request;
            InitRequest();
        }
        public RESTWebRequest(Uri uri, HttpVerb verb)
        {
            _baseRequest = (IRequest)WebRequest.Create(uri);
            _baseRequest.Method = verb.ToString();
            InitRequest();
        }
        public RESTWebRequest(string uri, HttpVerb verb)
        {
            _baseRequest = (IRequest)WebRequest.Create(uri);
            _baseRequest.Method = verb.ToString();
            InitRequest();
        }
        private void InitRequest()
        {
            HeaderParameters = new Dictionary<string, string>();
        }
        public RESTWebResponse GetRESTResponse()
        {
            //Add parameters into Headers
            foreach(var pair in HeaderParameters)
            {
                _baseRequest.Headers.Add(pair.Key, pair.Value);
            }
            var restResponse = new RESTWebResponse(_baseRequest);
            return restResponse;
        }
    }
    public enum HttpVerb
    {
        GET,
        PUT,
        POST,
        DELETE
    }
    public class RESTRequest : IRequest
    {
        private WebRequest _baseRequest;
        public RESTRequest(WebRequest request)
        {
            _baseRequest = request;
        }
        public System.Net.Security.AuthenticationLevel AuthenticationLevel
        {
            get
            {
                return _baseRequest.AuthenticationLevel;
            }
            set
            {
                _baseRequest.AuthenticationLevel = value;
            }
        }

        public ICredentials Credentials
        {
            get
            {
                return _baseRequest.Credentials;
            }
            set
            {
                _baseRequest.Credentials = value;
            }
        }

        public int Timeout
        {
            get
            {
                return _baseRequest.Timeout;
            }
            set
            {
                _baseRequest.Timeout = value;
            }
        }

        public WebHeaderCollection Headers
        {
            get { return _baseRequest.Headers; }
        }

        public IWebProxy Proxy
        {
            get
            {
                return _baseRequest.Proxy;
            }
            set
            {
                _baseRequest.Proxy = value;
            }
        }

        public IResponse GetResponse()
        {
            return new RESTResponse(_baseRequest.GetResponse());
        }

        public async Task<IResponse> GetResponseAsync()
        {
            return new RESTResponse(await _baseRequest.GetResponseAsync());
        }

        public string Method
        {
            get
            {
                return _baseRequest.Method;
            }
            set
            {
                _baseRequest.Method = value;
            }
        }
    }
    public interface IRequest
    {
        System.Net.Security.AuthenticationLevel AuthenticationLevel { get; set; }
        ICredentials Credentials { get; set; }
        int Timeout { get; set; }
        WebHeaderCollection Headers { get; }
        IWebProxy Proxy { get; set; }
        IResponse GetResponse();
        Task<IResponse> GetResponseAsync();
        string Method { get; set; }
    }
}
