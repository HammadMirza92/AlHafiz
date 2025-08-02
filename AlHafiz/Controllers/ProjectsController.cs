using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using AlHafiz.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlHafiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjects()
        {
            try
            {
                var projects = await _projectRepository.GetAllAsync();
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Projects>> GetProject(int id)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(id);

                if (project == null)
                {
                    return NotFound($"Project with ID {id} not found");
                }

                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<ActionResult<Projects>> CreateProject([FromBody] Projects project)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Set creation timestamp
                project.CreatedAt = DateTime.Now;

                await _projectRepository.AddAsync(project);

                // Assuming the repository returns the created entity with its ID
                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] Projects project)
        {
            try
            {
                if (id != project.Id)
                    return BadRequest("Project ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingProject = await _projectRepository.GetByIdAsync(id);
                if (existingProject == null)
                    return NotFound($"Project with ID {id} not found");

                // Update fields manually
                existingProject.ProjectName = project.ProjectName;
                existingProject.Description = project.Description;
                existingProject.Status = project.Status;
                existingProject.UpdatedAt = DateTime.Now;

                await _projectRepository.UpdateAsync(existingProject);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            
            try
            {
                var result = await _projectRepository.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Example of additional endpoint with custom repository method
        // GET: api/Projects/status/active
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjectsByStatus(string status)
        {
            try
            {
                // This would use a custom repository method if implemented
                var projects = await _projectRepository.GetAllAsync();
                var filteredProjects = projects.Where(p => p.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

                return Ok(filteredProjects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}