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
        Task<Stream> GetStreamAsync();
        void Close();
    }
}
