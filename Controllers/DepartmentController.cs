using Book.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get() {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dbList = dbclient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int LastDepartmentId = dbclient.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable().Count();
            dep.DepartmentId = LastDepartmentId + 1;

            dbclient.GetDatabase("testdb").GetCollection<Department>("Department").InsertOne(dep);

            return new JsonResult("Added Succesfully..");
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var fillter = Builders<Department>.Filter.Eq("DepartmentId", dep.DepartmentId);
            var update = Builders<Department>.Update.Set("DepartmentName", dep.DepartmentName);

            dbclient.GetDatabase("testdb").GetCollection<Department>("Department").UpdateOne(fillter, update);

            return new JsonResult("Updated Succesfully..");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var fillter = Builders<Department>.Filter.Eq("DepartmentId", id);
           

            dbclient.GetDatabase("testdb").GetCollection<Department>("Department").DeleteOne(fillter);

            return new JsonResult("Deleted Succesfully..");
        }
    }
}
