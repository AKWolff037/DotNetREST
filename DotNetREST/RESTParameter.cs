using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetREST
{
    public class RESTParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public RESTParameterMethod Method { get; set; }
        public Encoding StringEncoder { get; private set; }
        public RESTParameter(string name, object value, RESTParameterMethod method)
        {
            Init(name, value, method, Encoding.UTF8);
        }
        public RESTParameter(string name, object value)
        {
            Init(name, value, RESTParameterMethod.QUERY_STRING, Encoding.UTF8);
        }
        private void Init(string name, object value, RESTParameterMethod method, Encoding encoder)
        {
            Name = name;
            Value = value;
            Method = method;
            StringEncoder = encoder;
        }
    }
    public enum RESTParameterMethod
    {
        QUERY_STRING,
        REQUEST_HEADER,
        REQUEST_STREAM
    }
}
