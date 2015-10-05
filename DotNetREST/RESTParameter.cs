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
