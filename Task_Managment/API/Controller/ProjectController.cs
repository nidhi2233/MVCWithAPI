using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectInterface _projectRepo;
        public ResponseModel<string> _response;

        public ProjectController(ProjectInterface projectRepo)
        {
            _projectRepo = projectRepo;
            _response = new ResponseModel<string>();
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromForm] Project project)
        {
            if (ModelState.IsValid)
            {
                var response = await _projectRepo.AddProject(project);
                if (response.success)
                {
                    return Ok(new { response.success, response.message, response.data });
                }
                else
                {
                    return BadRequest(new { response.success, response.message, response.data });
                }
            }
            else
            {
                return BadRequest(_response);
            }
        }
        [HttpGet("{id}")]
        // [Authorize]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var response = await _projectRepo.GetProjectById(id);
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }

        }

        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> GetAllProject()
        {
            var response = await _projectRepo.GetAllProject();
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }

        }

        [HttpPut("{id}")]
        // [Authorize]
        public async Task<IActionResult> UpdateProject(string id, [FromForm] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (int.TryParse(id, out var parsedId) && project.ProjectId != parsedId)
            {
                return BadRequest(new { success = false, message = "Contact ID mismatch." });
            }
            var response = await _projectRepo.UpdateProject(project);
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }
        }

        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var response = await _projectRepo.DeleteProject(id);
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }
        }

    }
}