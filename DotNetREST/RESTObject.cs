using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Dynamic;
namespace DotNetREST
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
     */ 
    public class RESTObject<T> where T : class, new()
    {
        private ExpandoObject _dynamic;
        private T _explicit;

        public ExpandoObject DynamicObject { get { return _dynamic; } }
        public T ExplicitObject { get { return _explicit; } }

        public RESTObject(string json)
        {
            //Deserialize the json string into an ExpandoObject
            var jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(json);
            ParseDynamic(jsonObject);
        }
        public RESTObject(RESTWebResponse response)
        {
            //Get the JSON from a Web Response and parse it into an ExpandoObject and typed T object
            var responseStream = new System.IO.StreamReader(response.Base.GetResponseStream());
            var responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(responseStream.ReadToEnd());
            ParseDynamic(responseJson);
        }
        private void ParseDynamic(ExpandoObject input)
        {
            //Parse when given an ExpandoObject
            _dynamic = input;
            var dict = _dynamic as IDictionary<string, object>;
            ParseDictionary<T>(dict, out _explicit);
        }
        protected static void ParseDictionary(IDictionary<string, object> Dict, out object Target, Type explicitType)
        {
            if (explicitType.IsArray)
            {
                var length = Dict.Keys.Count();
                Target = (Array)Activator.CreateInstance(explicitType, new object[] { length });
            }
            else
            {
                Target = Activator.CreateInstance(explicitType);
            }
            foreach (var property in Target.GetType().GetProperties())
            {
                var propertyName = property.Name;
                if (Dict.ContainsKey(propertyName))
                {
                    var val = Dict[propertyName];
                    var propertyVal = explicitType.GetProperty(propertyName);
                    var expectedType = property.PropertyType;
                    if (val.GetType() != propertyVal.GetType() && val is IConvertible)
                    {
                        var explicitVal = Convert.ChangeType(val, propertyVal.PropertyType);
                        propertyVal.SetValue(Target, explicitVal);
                    }
                    else if (val is IDictionary<string, object>)
                    {
                        var propType = propertyVal.PropertyType;
                        object explicitVal = Activator.CreateInstance(propType);
                        ParseDictionary(val as IDictionary<string, object>, out explicitVal, propType);
                        propertyVal.SetValue(Target, explicitVal);
                    }
                    else if (val is T)
                    {
                        propertyVal.SetValue(Target, val as T);
                    }
                    else if (val is IList<T>)
                    {
                        var Tlist = val as IList<T>;
                        propertyVal.SetValue(Target, Tlist);
                    }
                    else if (val is IList)
                    {
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
                        var listType = typeof(List<>);
                        var typedList = listType.MakeGenericType(elementType);
                        var explicitList = (IList)Activator.CreateInstance(typedList);
                        foreach(var element in val as IList<object>)
                        {
                            object explicitElement;
                            ParseDictionary(element as IDictionary<string, object>, out explicitElement, elementType);
                            explicitList.Add(explicitElement);
                        }
                        if(property.PropertyType.IsArray)
                        {
                            var arrayType = elementType.MakeArrayType();
                            var array = (Array)Activator.CreateInstance(arrayType, new object[] { explicitList.Count });
                            explicitList.CopyTo(array, 0);
                            propertyVal.SetValue(Target, array);
                        }
                        else
                        {
                            propertyVal.SetValue(Target, explicitList);
                        }
                    }
                    else
                    {
                        propertyVal.SetValue(Target, val);
                    }
                }
            }
        }    
        protected static void ParseDictionary<K>(IDictionary<string, object> Dict, out K Target) where K : class, new()
        {
            Target = new K();
            var explicitType = Target.GetType();
            var outObject = new object();
            ParseDictionary(Dict, out outObject, explicitType);
            Target = outObject as K;
        }
    }
}
