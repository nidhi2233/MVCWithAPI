using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface ProjectInterface
    {
        public Task<ResponseModel<List<Project>>>  GetAllProject();
        public Task<ResponseModel<Project>> GetProjectById(int id);
        public Task<ResponseModel<string>> AddProject(Project project);
        public Task<ResponseModel<string>> UpdateProject(Project project);
        public Task<ResponseModel<string>> DeleteProject(int id);
    }
}