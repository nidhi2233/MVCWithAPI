using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using repositories.Interfaces;
using repositories.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactJwtController : ControllerBase
    {
         private readonly IContactInterface _contact;
        public ContactJwtController(IConfiguration configuration, IContactInterface _contact)
        {
            this._contact = _contact;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            string userid = HttpContext.Request.Query["userid"].ToString();
            string? userId = User.FindFirst("Userid")?.Value;
            List<t_Contact> list;
            if (userid != "")
            {
                list = await _contact.GetAllByUser(userid);
            }
            else
            {
                list = await _contact.GetAll();
            }
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetContactById(string id)
        {
            var contact = await _contact.GetOne(id);
            if (contact == null)
            {
                return BadRequest(new { success = false, message = "There was no contact found" });
            }
            return Ok(contact);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddContact([FromForm] t_Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (contact.formfile != null)
            {
                var filename = contact.Email + Path.GetExtension(contact.formfile.FileName);
                var filePath = Path.Combine("../MVC/wwwroot/contact_images", filename);
                contact.Image = filename;
                System.IO.File.Delete(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    contact.formfile.CopyTo(stream);
                }
            }
            var status = await _contact.Add(contact);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Contact Inserted Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There was some error while adding the contact" });
            }

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateContact(string id, [FromForm] t_Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
 
            if (contact.formfile != null && contact.formfile.Length > 0)
            {
                var filename = contact.Email + Path.GetExtension(contact.formfile.FileName);
                var filePath = Path.Combine("../MVC/wwwroot/contact_images", filename);
                contact.Image = filename;
                // System.IO.File.Delete(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    contact.formfile.CopyTo(stream);
                }
            }
            int status = await _contact.Update(contact);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Contact Updated Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There was some error while update coontact" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteContact(string id)
        {
            int status = await _contact.Delete(id);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Contact Deleted Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There is some error while delete contact" });
            }
        }
    }
}