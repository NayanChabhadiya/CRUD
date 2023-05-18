using MongoDB.Bson;

namespace Book.Models
{
    public class Employee
    {
        public ObjectId Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
    }
}
