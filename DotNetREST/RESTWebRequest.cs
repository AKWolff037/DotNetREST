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
        public ICollection<RESTParameter> Parameters { get; set; }

        public IRequest Base
        {
            get { return _baseRequest; }
        }
        private IRequest _baseRequest;
        private RESTWebRequest()
        {
            _baseRequest = new RESTRequest(HttpWebRequest.Create("http://localhost:8080"));
            InitRequest();
        }
        public RESTWebRequest(IRequest request)
        {
            _baseRequest = request;
            InitRequest();
        }
        public RESTWebRequest(Uri uri, HttpVerb verb)
        {
            _baseRequest = new RESTRequest(HttpWebRequest.Create(uri));
            _baseRequest.Method = verb.ToString();
            InitRequest();
        }

        private void InitRequest()
        {
            Parameters = new List<RESTParameter>();
        }
        public RESTWebResponse GetRESTResponse()
        {
            if (_baseRequest.Method.ToUpper() != "GET")
            {
                AddParameters();
            }
            var restResponse = new RESTWebResponse(_baseRequest);
            return restResponse;
        }
        private void AddParameters()
        {
            bool isUriChanged = false;
            bool isFirstQueryParam = true;
            var parameterString = "";
            var requestStream = _baseRequest.GetRequestStream();
            var currentPosition = 0;
            foreach (var restParameter in Parameters)
            {                
                switch (restParameter.Method)
                {
                    case RESTParameterMethod.REQUEST_STREAM:
                        var parameterBytes = restParameter.StringEncoder.GetBytes(restParameter.Value.ToString());
                        requestStream.Write(parameterBytes, currentPosition, parameterBytes.Length);
                        currentPosition += parameterBytes.Length;
                        break;
                    case RESTParameterMethod.REQUEST_HEADER:
                        _baseRequest.Headers.Add(restParameter.Name, restParameter.Value.ToString());
                        break;
                    case RESTParameterMethod.QUERY_STRING:
                    default:                                                
                        if (isFirstQueryParam)
                        {
                            parameterString += "?" + restParameter.Name + "=" + restParameter.Value;
                            isFirstQueryParam = false;
                        }
                        else
                        {
                            parameterString += "&" + restParameter.Name + "=" + restParameter.Value;
                        }
                        isUriChanged = true;
                        break;
                }
            }            
            if(isUriChanged)
            {
                var newUri = _baseRequest.RequestUri + parameterString;
                var originalRequest = _baseRequest;
                _baseRequest = new RESTRequest(HttpWebRequest.Create(newUri), originalRequest);
            }
            _baseRequest.ContentType = "application/json";
        }
    }
    public enum ParameterType
    {
        URI,
        HEADER
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
        public RESTRequest(WebRequest newRequest, IRequest oldRequest)
        {
            //Probably really slow, but makes sure we get the same 
            foreach(var property in oldRequest.GetType().GetProperties())
            {
                //Make sure property exists in new request in case they are different
                var propExists = newRequest.GetType().GetProperties().Contains(property);
                if (propExists)
                {
                    if (property.Name != "RequestUri")
                    {
                        property.SetValue(newRequest, property.GetValue(oldRequest));
                    }
                }
            }
            _baseRequest = newRequest;
        }
        public Uri RequestUri
        {
            get { return _baseRequest.RequestUri; }            
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

        public long ContentLength { get { return _baseRequest.ContentLength; } set { _baseRequest.ContentLength = value; } }
        public string ContentType { get { return _baseRequest.ContentType; } set { _baseRequest.ContentType = value; } }
        public Stream GetRequestStream()
        {
            return _baseRequest.GetRequestStream();
        }
        public async Task<Stream> GetRequestStreamAsync()
        {
            return await _baseRequest.GetRequestStreamAsync();
        }
    }
    public interface IRequest
    {
        long ContentLength { get; set; }
        string ContentType { get; set; }
        Uri RequestUri { get; }
        System.Net.Security.AuthenticationLevel AuthenticationLevel { get; set; }
        ICredentials Credentials { get; set; }
        int Timeout { get; set; }
        WebHeaderCollection Headers { get; }
        IWebProxy Proxy { get; set; }
        IResponse GetResponse();
        Task<IResponse> GetResponseAsync();
        string Method { get; set; }
        Stream GetRequestStream();
        Task<Stream> GetRequestStreamAsync();
    }
}
