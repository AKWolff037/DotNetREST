#region LICENSE/NOTICE
/*
Copyright 2015 Alex Wolff
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace DotNetRest
{
    public class RestWebResponse
    {
        private IResponse _baseResponse;
        public IResponse Base
        {
            get { return _baseResponse; }
        }
        private RestWebResponse(IResponse response)
        {
            _baseResponse = response;
        }
        public RestWebResponse(IRequest request)
        {
            if(request == null)
            {
                throw new InvalidOperationException("Cannot create an instance of RestWebResponse with a null request");
            }
            _baseResponse = request.GetResponse();
        }
        public static RestWebResponse GetResponse(IRequest request)
        {
            if (request == null)
            {
                throw new InvalidOperationException("Cannot create an instance of RestWebResponse with a null request");
            }
            var response = request.GetResponse();
            return new RestWebResponse(response);
        }
        public static async Task<RestWebResponse> GetResponseAsync(IRequest request)
        {
            var response = await request.GetResponseAsync();
            return new RestWebResponse(response);
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
