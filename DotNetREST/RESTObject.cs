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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Dynamic;
using System.Globalization;
namespace DotNetRest
{
    /**
     * To use, call the default constructor with the type that you wish to convert the JSON to
     * For example, if you have a class Car, call with new RestObject<Car>(carJson);
     * This will only support weakly typed classes, and does not support inheritence or generics very well
     * 
     * However, if you know the exact form of the JSON coming back, it should be easy to create a class to match
     * 
     * After instantiating the class, call the statically typed object by calling RestObject.ExplicitObject
     * Or if you wish, you can call the dynamically typed RestObject.DynamicObject instead
     * 
     * .Net decimal values are only supported if they fall between double.MIN_VALUE and double.MAX_VALUE
     */ 
    public class RestObject<T> where T : class, new()
    {
        private ExpandoObject _dynamic;
        private ICollection<ExpandoObject> _dynamicList;
        private T _explicit;
        private ICollection<T> _explicitList;

        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public ExpandoObject DynamicObject { get { return _dynamic; } }
        public T ExplicitObject { get { return _explicit; } }
        public ICollection<T> ExplicitCollection { get { return _explicitList; } }
        public ICollection<ExpandoObject> DynamicCollection { get { return _dynamicList; } }
        protected RestObject()
        {
            InitRestObject();
        }
        private void InitRestObject()
        {
            _dynamicList = new List<ExpandoObject>();
            _explicitList = new List<T>();
        }
        protected RestObject(ExpandoObject expando)
        {
            ParseDynamic(expando);
        }
        public RestObject(string json)
        {
            InitRestObject();
            ParseJson(json);
        }
        public RestObject(RestWebResponse response)
        {
            InitRestObject();
            //Get the JSON from a Web Response and parse it into an ExpandoObject and typed T object
            if (response != null)
            {
                var responseString = response.ResponseStreamAsString;
                ParseJson(responseString);
            }
            else
            {
                throw new InvalidOperationException("Cannot initialize RestObject with a null RestWebResponse");
            }
        }
        private void ParseJson(string json)
        {
            var isCollection = json.StartsWith("[", StringComparison.CurrentCulture);
            if(!isCollection)
            {
                var responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(json);
                _dynamic = responseJson;
                _explicit = ParseDynamic(responseJson);
            }
            else
            {
                var responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExpandoObject>>(json);
                foreach (var dict in responseJson)
                {
                    var exp = ParseDynamic(dict);
                    _dynamicList.Add(dict);
                    _explicitList.Add(exp);
                }
            }
        }
        private static T ParseDynamic(ExpandoObject input)
        {
            //Parse when given an ExpandoObject
            T output = default(T);
            var dictionary = input as IDictionary<string, object>;
            output = ParseDictionary<T>(dictionary);
            return output;
        }
        private static object ParseDictionary(IDictionary<string, object> inputDictionary, Type explicitType)
        {
            object targetObject = default(object);
            if(inputDictionary == null)
            {
                throw new InvalidOperationException("Dictionary was null, cannot parse a null dictionary");
            }
            if (explicitType.IsArray)
            {
                var length = inputDictionary.Keys.Count();
                targetObject = (Array)Activator.CreateInstance(explicitType, new object[] { length });
            }
            else
            {
                targetObject = Activator.CreateInstance(explicitType);
            }
            foreach (var property in targetObject.GetType().GetProperties())
            {
                var propertyName = property.Name;
                if (inputDictionary.ContainsKey(propertyName) && inputDictionary[propertyName] != null)
                {
                    var val = inputDictionary[propertyName];
                    var propertyVal = explicitType.GetProperty(propertyName);
                    var expectedType = property.PropertyType;
                    var valType = val.GetType();
                    if(valType == expectedType)
                    {
                        //Hurray, we matched!
                        propertyVal.SetValue(targetObject, val);
                    }
                    else if (valType != expectedType && val is IConvertible)
                    {
                        Type safeType = Nullable.GetUnderlyingType(expectedType) ?? expectedType;
                        //Special Case - INT64 to DATETIME Conversion (UNIX Time)
                        if((valType == typeof(long) || valType == typeof(long?))
                            && (safeType == typeof(DateTime) || safeType == typeof(DateTime?)))
                        {
                            var longValue = (long)Convert.ChangeType(val, typeof(long), CultureInfo.InvariantCulture);
                            var dateValue = UNIX_EPOCH.AddSeconds(longValue);
                            val = dateValue;
                        }
                        //Convert if possible
                        var explicitVal = (val == null ? null : Convert.ChangeType(val, safeType, CultureInfo.InvariantCulture));
                        propertyVal.SetValue(targetObject, explicitVal, null);
                        
                    }
                    else if (val is IDictionary<string, object>)
                    {
                        //Parse non-simple object
                        var propType = propertyVal.PropertyType;
                        var valAsDictionary = val as IDictionary<string, object>;
                        object explicitVal = Activator.CreateInstance(propType);
                        explicitVal = ParseDictionary(valAsDictionary, propType);
                        propertyVal.SetValue(targetObject, explicitVal);
                    }
                    else if (val is IList)
                    {
                        //Parse list/enumeration/array
                        Type elementType;
                        if (expectedType.IsArray)
                        {
                            //Array type is explicitly included with GetElementType
                            elementType = expectedType.GetElementType();
                        }
                        else if (expectedType.IsGenericType)
                        {
                            //Get List type by inspecting generic argument
                            elementType = expectedType.GetGenericArguments()[0];
                        }
                        else
                        {
                            //Not sure how we'd get here if we're neither an array nor generic, but we can't really do much
                            continue;
                        }
                        //Create the necessary List implementation that we need
                        var listType = typeof(List<>);
                        var typedList = listType.MakeGenericType(elementType);
                        var explicitList = (IList)Activator.CreateInstance(typedList);
                        foreach(var element in val as IList<object>)
                        {
                            var explicitElement = ParseDictionary(element as IDictionary<string, object>, elementType);
                            explicitList.Add(explicitElement);
                        }
                        if(property.PropertyType.IsArray)
                        {
                            //Convert from list to array if necessary
                            var arrayType = elementType.MakeArrayType();
                            var array = (Array)Activator.CreateInstance(arrayType, new object[] { explicitList.Count });
                            explicitList.CopyTo(array, 0);
                            propertyVal.SetValue(targetObject, array);
                        }
                        else
                        {
                            propertyVal.SetValue(targetObject, explicitList);
                        }
                    }
                    else
                    {
                        //Attempt to set it - will error if not compatible and all other checks are bypassed
                        propertyVal.SetValue(targetObject, val);
                    }
                }
            }
            return targetObject;
        }    
        private static K ParseDictionary<K>(IDictionary<string, object> inputDictionary) where K : class, new()
        {
            var output = new K();
            var explicitType = output.GetType();
            var typedObject = ParseDictionary(inputDictionary, explicitType);
            output = typedObject as K;
            return output;
        }
    }
}
