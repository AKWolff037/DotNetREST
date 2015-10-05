using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using System.Web.Http;

namespace DotNetRESTUnitTest.Mocks
{
    public class MockTestRESTObjectController : ApiController
    {
        private IList<TestRESTObject> _testList;
        public IHttpActionResult Get()
        {
            return Ok(GetTestResults());
        }
        public IHttpActionResult Get(int id)
        {
            var results = GetTestResults();
            var result = results.First(r => r.ID == id);
            if(result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
        public IHttpActionResult Post(TestRESTObject input)
        {
            if(input == null)
            {
                return NotFound();
            }
            else
            {
                GetTestResults().Add(input);
                return Ok(input);
            }
        }
        public IHttpActionResult Put(int? id, TestRESTObject input)
        {
            if(id == null || input == null || !id.HasValue)
            {
                return NotFound();
            }
            else
            {
                var match = GetTestResults().First(r => r.ID == id);
                if(match == null)
                {
                    return NotFound();
                }
                match = input;
                return Ok(match);
            }
        }
        public IHttpActionResult Delete(int? id)
        {
            if(id == null || !id.HasValue)
            {
                return NotFound();
            }
            else
            {
                var match = GetTestResults().First(r => r.ID == id.Value);
                GetTestResults().Remove(match);
                return Ok(match);
            }
        }
        private IList<TestRESTObject> GetTestResults()
        {
            if(_testList == null)
            {
                _testList = new List<TestRESTObject>();
                var object1 = TestRESTObject.CreateTestObject(false);
                var object2 = TestRESTObject.CreateTestObject(true);
                var object3 = TestRESTObject.CreateTestObject(true);
                var object4 = TestRESTObject.CreateTestObject(false);
                _testList.Add(object1);
                _testList.Add(object2);
                _testList.Add(object3);
                _testList.Add(object4);
            }
            return _testList;
        }
    }
}
