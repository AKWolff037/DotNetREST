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
    public class RESTObject<T> where T : class, new()
    {
        private ExpandoObject _dynamic;
        private T _explicit;

        public ExpandoObject DynamicObject { get { return _dynamic; } }
        public T ExplicitObject { get { return _explicit; } }

        public RESTObject(string json)
        {
            var jsonObject = JsonConvert.DeserializeObject<ExpandoObject>(json);
            ParseDynamic(jsonObject);
        }
        public RESTObject(RESTWebResponse response)
        {
            var responseStream = new System.IO.StreamReader(response.Base.GetResponseStream());
            var responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(responseStream.ReadToEnd());
            ParseDynamic(responseJson);
        }
        private void ParseDynamic(ExpandoObject input)
        {
            _dynamic = input;
            var dict = _dynamic as IDictionary<string, object>;
            ParseDictionary<T>(dict, out _explicit);
        }
        public static void ParseDictionary(IDictionary<string, object> Dict, out object Target, Type explicitType)
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
                    if (val is Int64)
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
                    else if (val is IList<object>)
                    {
                        var isAlreadySet = false;
                        var aryVal = (val as IList<object>).ToArray();
                        var propType = propertyVal.PropertyType;
                        IList explicitList;
                        if (propType.IsArray)
                        {
                            var instanceType = propType.GetElementType();
                            var listType = typeof(List<>);
                            var explicitListType = listType.MakeGenericType(instanceType);
                            explicitList = (IList)Activator.CreateInstance(explicitListType);
                        }
                        else
                        {
                            explicitList = (IList)Activator.CreateInstance(propType);
                        }
                        foreach (var obj in aryVal)
                        {
                            var objType = obj.GetType();
                            if (obj is T)
                            {
                                explicitList.Add(obj as T);
                            }
                            else if (obj is IDictionary<string, object> && propType.IsArray)
                            {
                                object explicitVal;
                                Type instanceType = propType.GetElementType();
                                ParseDictionary(obj as IDictionary<string, object>, out explicitVal, instanceType);
                                explicitList.Add(explicitVal);
                            }
                            else if (obj is IDictionary<string, object>)
                            {
                                object explicitVal;
                                ParseDictionary(obj as IDictionary<string, object>, out explicitVal, propType);
                                propertyVal.SetValue(Target, explicitVal);
                                isAlreadySet = true;
                            }
                        }
                        if (!isAlreadySet)
                        {
                            if (propType.IsArray)
                            {
                                var instanceType = propType.GetElementType();
                                var array = (Array)Activator.CreateInstance(propType, new object[] { explicitList.Count });
                                explicitList.CopyTo(array, 0);
                                propertyVal.SetValue(Target, array);
                            }
                            else
                            {
                                propertyVal.SetValue(Target, explicitList);
                            }
                        }
                    }
                    else
                    {
                        propertyVal.SetValue(Target, val);
                    }
                }
            }
        }    
        public static void ParseDictionary<K>(IDictionary<string, object> Dict, out K Target) where K : class, new()
        {
            Target = new K();
            var explicitType = Target.GetType();
            var outObject = new object();
            ParseDictionary(Dict, out outObject, explicitType);
            Target = outObject as K;
        //    foreach (var property in Target.GetType().GetProperties())
        //    {
        //        var propertyName = property.Name;
                
        //        if (Dict.ContainsKey(propertyName))
        //        {
        //            var val = Dict[propertyName];
        //            var propertyVal = explicitType.GetProperty(propertyName);
        //            if (val is Int64)
        //            {
        //                var explicitVal = Convert.ChangeType(val, propertyVal.PropertyType);
        //                propertyVal.SetValue(Target, explicitVal);
        //            }
        //            else if (val is IDictionary<string, object>)
        //            {
        //                K obj = new K();
        //                ParseDictionary(val as IDictionary<string, object>, out obj);
        //            }
        //            else if (val is K)
        //            {
        //                propertyVal.SetValue(Target, val as K);
        //            }
        //            else if (val is T)
        //            {
        //                propertyVal.SetValue(Target, val as T);
        //            }
        //            else if (val is IList<K>)
        //            {
        //                var Klist = val as IList<K>;
        //                propertyVal.SetValue(Target, Klist);
        //            }
        //            else if (val is IList<T>)
        //            {
        //                var Tlist = val as IList<T>;
        //                propertyVal.SetValue(Target, Tlist);
        //            }
        //            else if (val is IList<object>)
        //            {
        //                var isAlreadySet = false;
        //                var aryVal = (val as IList<object>).ToArray();
        //                var propType = propertyVal.PropertyType;
        //                IList explicitList;
        //                if (propType.IsArray)
        //                {
        //                    var instanceType = propType.GetElementType();
        //                    var listType = typeof(List<>);
        //                    var explicitListType = listType.MakeGenericType(instanceType);
        //                    explicitList = (IList)Activator.CreateInstance(explicitListType);
        //                }
        //                else
        //                {
        //                    explicitList = (IList)Activator.CreateInstance(propType);
        //                }
        //                foreach(var obj in aryVal)
        //                {
        //                    var objType = obj.GetType();
        //                    if(obj is K)
        //                    {
        //                        explicitList.Add(obj as K);
        //                    }
        //                    else if (obj is T)
        //                    {
        //                        explicitList.Add(obj as T);
        //                    }
        //                    else if(obj is IDictionary<string, object> && propType.IsArray)
        //                    {
        //                        object explicitVal;
        //                        Type instanceType = propType.GetElementType();
        //                        ParseDictionary(obj as IDictionary<string, object>, out explicitVal, instanceType);
        //                        explicitList.Add(explicitVal);
        //                    }
        //                    else if(obj is IDictionary<string, object>)
        //                    {
        //                        object explicitVal;
        //                        ParseDictionary(obj as IDictionary<string, object>, out explicitVal, propType);
        //                        propertyVal.SetValue(Target, explicitVal);
        //                        isAlreadySet = true;
        //                    }
        //                }
        //                if (!isAlreadySet)
        //                {
        //                    if (propType.IsArray)
        //                    {
        //                        var instanceType = propType.GetElementType();
        //                        var array = (Array)Activator.CreateInstance(propType, new object[] { explicitList.Count });
        //                        explicitList.CopyTo(array, 0);
        //                        propertyVal.SetValue(Target, array);
        //                    }
        //                    else
        //                    {
        //                        propertyVal.SetValue(Target, explicitList);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                propertyVal.SetValue(Target, val);
        //            }
        //        }
        //    }
        }
    }
}
