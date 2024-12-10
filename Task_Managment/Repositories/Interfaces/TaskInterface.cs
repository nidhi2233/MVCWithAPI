using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface TaskInterface
    {
        public Task<ResponseModel<List<TaskModel>>> GetAllTaskAndById(int TaskId = 0, int EmpId = 0, int ProjectId = 0);
        public Task<ResponseModel<TaskModel>> GetTaskById(int id);
        public Task<ResponseModel<string>> AddTask(TaskModel task);
        public Task<ResponseModel<string>> UpdateTask(TaskModel task);
        public Task<ResponseModel<string>> DeleteTask(int id);
    }
}