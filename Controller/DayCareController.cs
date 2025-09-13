using System.Threading.Tasks;
using DayCareApi.Models;
using DayCareApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DayCareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DayCareController : ControllerBase
    {
        private readonly IDayCareRepository _dayCareRepository;

        public DayCareController(IDayCareRepository dayCareRepository)
        {
            _dayCareRepository = dayCareRepository;
        }

        // GET: api/daycare
        [HttpGet]
        public async Task<IActionResult> GetReimbursements()
        {
            var initiatorEmpId = 12345; 
            var data = await _dayCareRepository.GetByEmployeeIdAsync(initiatorEmpId);
            return Ok(data);
        }

        [HttpPost("child")]
        public async Task<IActionResult> AddChild([FromBody] DayCareReimbursement model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var initiatorEmpId = 12345; 
            var result = await _dayCareRepository.AddChildAsync(model, initiatorEmpId);
            return Ok(new { message = "Child data added successfully.", rowsAffected = result });
        }

        [HttpPut("child/{rid}")]
        public async Task<IActionResult> UpdateChild(int rid, [FromBody] DayCareReimbursement model)
        {
            if (!ModelState.IsValid || model.RID != rid)
                return BadRequest("Model is invalid or the RID does not match.");
            var result = await _dayCareRepository.UpdateChildAsync(model);
            if (result == 0) return NotFound();
            return Ok(new { message = "Child data updated successfully.", rowsAffected = result });
        }

        [HttpDelete("child/{rid}/{dcid}")]
        public async Task<IActionResult> DeleteChild(int rid, int dcid)
        {
            var result = await _dayCareRepository.DeleteChildAsync(rid, dcid);
            if (result == 0) return NotFound();
            return Ok(new { message = "Child data deleted successfully.", rowsAffected = result });
        }

        [HttpPost("submit/{rid}/{dcid}")]
        public async Task<IActionResult> SubmitReimbursement(int rid, int dcid)
        {
            var result = await _dayCareRepository.UpdateDraftStatusAsync(rid, dcid, 1);
            if (result == 0) return NotFound();
            return Ok(new { message = "Reimbursement submitted successfully.", rowsAffected = result });
        }
    }
}