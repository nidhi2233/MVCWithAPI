using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
       private readonly TaskInterface _taskRepo;
       public ResponseModel<string> _response;
        public TaskController(TaskInterface taskRepo)
        {
            _taskRepo = taskRepo;
            _response = new ResponseModel<string>();
        }

        [HttpPost("AddTask")]
        public async Task<IActionResult> AddTask([FromForm] TaskModel task)
        {
            var response= await _taskRepo.AddTask(task);
            if(ModelState.IsValid)
            {
                
                if(response.success)
                {
                    return Ok(new{response.success,response.message,response.data});
                }
                else
                {
                    return BadRequest(new{response.success,response.message,response.data});
                }
            }
            else
            {
                return BadRequest(_response);
            }
        }
        [HttpGet("GetTaskById/{id}")]
        // [Authorize]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var response = await _taskRepo.GetTaskById(id); 
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }

        }

         [HttpGet("GetAllTaskById/{id}")]
        // [Authorize]
        public async Task<IActionResult> GetAllTaskById(int id)
        {
            var response = await _taskRepo.GetAllTaskAndById(id, 0, 0);
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }
        }


        [HttpGet("GetAllTaskByProjectId/{id}")]
        // [Authorize]
        public async Task<IActionResult> GetAllTaskByProjectId(int id)
        {
            var response = await _taskRepo.GetAllTaskAndById(0, id, 0);
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }
        }

        [HttpGet("GetAllTaskByEmpId/{id}")]
        // [Authorize]
        public async Task<IActionResult> GetAllTaskByEmpId(int id)
        {
            var response = await _taskRepo.GetAllTaskAndById(0, 0, id);
            if (response.success)
            {
                return Ok(new { response.success, response.message, response.data });
            }
            else
            {
                return BadRequest(new { response.success, response.message, response.data });
            }
        }

        [HttpPut("UpdateTask")]
        // [Authorize]
        public async Task<IActionResult> UpdateProject([FromForm] TaskModel task)
        {
            if (ModelState.IsValid)
            {
                var response = await _taskRepo.UpdateTask(task);
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

        [HttpDelete("DeleteProject/{id}")]
        // [Authorize]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var response = await _taskRepo.DeleteTask(id);
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