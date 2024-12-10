using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface EmployeeInterface
    {
        public Task<ResponseModel<List<Employee>>> GetAllEmployee();
        public Task<ResponseModel<Employee>> GetEmployeeById(int id);
        public Task<ResponseModel<Employee>> Login(Login login);
        public Task<ResponseModel<string>>  AddEmployee(Employee employee);
        public Task<ResponseModel<string>>  UpdateEmployee(Employee employee);
        public Task<ResponseModel<string>>  DeleteEmployee(int id);
        
    }
}