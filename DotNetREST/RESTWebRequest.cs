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
