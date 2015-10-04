using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace DotNetREST
{
    public class RESTWebResponse
    {
        private IResponse _baseResponse;
        public IResponse Base
        {
            get { return _baseResponse; }
        }
        private RESTWebResponse(IResponse response)
        {
            _baseResponse = response;
        }
        public RESTWebResponse(IRequest request)
        {
            _baseResponse = request.GetResponse();
        }
        public static RESTWebResponse GetResponse(IRequest request)
        {
            var response = request.GetResponse();
            return new RESTWebResponse(response);
        }
        public static async Task<RESTWebResponse> GetResponseAsync(IRequest request)
        {
            var response = await request.GetResponseAsync();
            return new RESTWebResponse(response);
        }
        public string ResponseStreamAsString
        {
            get
            {
                var responseStream = new System.IO.StreamReader(Base.GetStream());
                var responseString = responseStream.ReadToEnd();
                return responseString;
            }
        }

    }
    public interface IResponse
    {
        Uri ResponseUri { get; }
        bool IsMutuallyAuthenticated { get; }
        bool IsFromCache { get; }
        bool SupportsHeaders { get; }
        WebHeaderCollection Headers { get; }
        string ContentType { get; set; }
        long ContentLength { get; }
        Stream GetStream();
        void Close();
    }
    public class RESTResponse : IResponse
    {
        private WebResponse _baseResponse;
        public RESTResponse(WebResponse response)
        {
            _baseResponse = response;
        }

        public Uri ResponseUri
        {
            get { return _baseResponse.ResponseUri; }
        }

        public bool IsMutuallyAuthenticated
        {
            get { return _baseResponse.IsMutuallyAuthenticated; }
        }

        public bool IsFromCache
        {
            get { return _baseResponse.IsFromCache; }
        }

        public bool SupportsHeaders
        {
            get { return _baseResponse.SupportsHeaders; }
        }

        public WebHeaderCollection Headers
        {
            get { return _baseResponse.Headers; }
        }

        public string ContentType
        {
            get
            {
                return _baseResponse.ContentType;
            }
            set
            {
                _baseResponse.ContentType = value;
            }
        }

        public long ContentLength
        {
            get { return _baseResponse.ContentLength; }
        }

        public Stream GetStream()
        {
            return _baseResponse.GetResponseStream();
        }

        public void Close()
        {
            _baseResponse.Close();
        }
    }
}
