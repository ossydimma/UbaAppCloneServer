using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UbaClone.WebApi.Data;
using UbaClone.WebApi.Models;
using UbaClone.WebApi.Repositories;

namespace UbaClone.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbaCloneController(IUsersRepository repo) : ControllerBase
    {
        private readonly IUsersRepository _repo = repo;

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.UbaClone>) )]
        public async Task<IEnumerable<Models.UbaClone>> GetAll()
        {
            //var clones = await _db.ubaClones.ToListAsync();
            //return Ok(clones);
            return await  _repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetUser) )]
        [ProducesResponseType(200, Type = typeof(Models.UbaClone))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(int id)
        {

            Models.UbaClone? user = await _repo.RetrieveAsync(id);
            if (user == null) return NotFound("user Not found");

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(200, Type =typeof(Models.UbaClone) )]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateUser([FromBody] Models.UbaClone user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            Models.UbaClone? addedUser = await _repo.CreateUserAsync(user);

            if (addedUser == null) return BadRequest("Repository failed to create user");

            return Ok(user);

        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Models.UbaClone user)
        {
            if (user is null || user.Id != id) return BadRequest();

            Models.UbaClone? existing = await _repo.RetrieveAsync(id);
            if (existing == null) return NotFound();

            await _repo.UpdateUserAsync(user);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            Models.UbaClone? user = await _repo.RetrieveAsync(id);
            if (user == null) return NotFound();

            bool? deleted = await _repo.DeleteUserAsync(id);

            if (deleted.HasValue && deleted.Value) return new NoContentResult();

            return BadRequest($"User {id} was found but failed to delete");
        }

    }
}
