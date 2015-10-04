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
        public string Value { get; set; }
        public RESTParameterMethod Method { get; set; }

        public RESTParameter(string name, string value, RESTParameterMethod method)
        {
            Init(name, value, method);
        }
        public RESTParameter(string name, string value)
        {
            Init(name, value, RESTParameterMethod.QUERY_STRING);
        }
        private void Init(string name, string value, RESTParameterMethod method)
        {
            Name = name;
            Value = value;
            Method = method;
        }
    }
    public enum RESTParameterMethod
    {
        QUERY_STRING,
        REQUEST_HEADER
    }
}
