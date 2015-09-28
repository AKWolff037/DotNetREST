using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace DotNetREST
{
    public class RESTWebResponse
    {
        private WebResponse _baseResponse;
        public WebResponse Base
        {
            get { return _baseResponse; }
        }
        private RESTWebResponse(WebResponse response)
        {
            _baseResponse = response;
        }
        public RESTWebResponse(RESTWebRequest request)
        {
            _baseResponse = request.Base.GetResponse();
        }
        public static async Task<RESTWebResponse> GetResponseAsync(RESTWebRequest request)
        {
            var response = await request.Base.GetResponseAsync();
            return new RESTWebResponse(response);
        }
    }
}
