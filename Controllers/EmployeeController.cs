using Book.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.IO;
using System.Linq;

namespace Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dbList = dbclient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int LastEmployeeId = dbclient.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable().Count();
            emp.EmployeeId = LastEmployeeId + 1;

            dbclient.GetDatabase("testdb").GetCollection<Employee>("Employee").InsertOne(emp);

            return new JsonResult("Added Succesfully..");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var fillter = Builders<Employee>.Filter.Eq("EmployeeId", emp.EmployeeId);
            var update = Builders<Employee>.Update.Set("EmployeeName", emp.EmployeeName)
                                                .Set("DepartmentName", emp.DepartmentName)
                                                .Set("DateOfJoining", emp.DateOfJoining)
                                                .Set("PhotoFileName", emp.PhotoFileName);

            dbclient.GetDatabase("testdb").GetCollection<Employee>("Employee").UpdateOne(fillter, update);

            return new JsonResult("Updated Succesfully..");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbclient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var fillter = Builders<Employee>.Filter.Eq("EmployeeId", id);


            dbclient.GetDatabase("testdb").GetCollection<Employee>("Employee").DeleteOne(fillter);

            return new JsonResult("Deleted Succesfully..");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using(var strem = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(strem);
                }

                return new JsonResult(fileName);

            }
            catch
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}
