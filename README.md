# DotNetRest
.Net library for easy RESTful calls with JSON parsing

# Limitations/Warnings
* The **decimal** type is not supported for values outside of the range of **double.MIN_VALUE** and **double.MAX_VALUE**
* Does not work with interfaces, generics (Other than Lists), or abstracts
* Requires you to know the format of the JSON being returned and create a wrapper class around it
* All fields in instantiated class must be public and have gets/sets
* Uses a lot of reflection, not the fastest thing in the world
* Currently only supports JSON return type
 

# Standard Usage

```
public class Foo
{
  public string Bar {get; set;}
  public int Baz {get; set;}
  public DateTime Timestamp {get; set;}
}
public void Main()
{
  //Initialize the request
  var uri = new Uri("http://localhost:8080/foo/1");
  var verb = HttpVerbs.GET;
  var apiKey = "H3LL0_W0RLD";
  var request = new RestWebRequest(uri, verb);
  //Add any header/parameter information into the request
  request.Parameters.add(new RestParameter("apikey", apiKey, RestParameterMethod.QUERY_STRING));
  var response = request.GetRestResponse();
  var myRestFoo = new RestObject<Foo>(response);
  // Also available, myRestFoo = await RestObject<Foo>.GetRestObjectAsync(response);
  var myFoo = myRestFoo.ExplicitObject;
  //Enjoy using myFoo as a statically typed object elsewhere
  ...
}
```

# Why ?
Normally the easy way to do this is to take your response, shove it into a **dynamic** and then set fields manually by doing
```
void SetFields(Foo myFoo, dynamic json)
{
  myFoo.Foo = json.Foo;
  myFoo.Bar = json.Bar;
  myFoo.Baz = json.Baz;
}
```
This lets you declare the type up front, then make the call and begin using it without having to parse the **dynamic** yourself!
