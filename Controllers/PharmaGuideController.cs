using Microsoft.AspNetCore.Mvc;
using MongoExample.Services;
using MongoExample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PharmaGuideController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        // Inject MongoDBService and retrieve the collection from it
        public PharmaGuideController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        // GET: api/pharmaguide
        [HttpGet("HerbsList")]
        public async Task<List<PharmaGuideCollections>> Get()
        {
            return await _mongoDBService.GetAsync();
        }

        // POST: api/pharmaguide
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PharmaGuideCollections pharmaguidecollections)
        {
            await _mongoDBService.CreateAsync(pharmaguidecollections);
            return CreatedAtAction(nameof(Get), new { id = pharmaguidecollections._id }, pharmaguidecollections);
        }

        // PUT: api/pharmaguide/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> AddToPharmaGuide(string id, [FromBody] HerbalDrugUpdate update)
        {
            // Validate that the `id` in the URL matches the `DrugId` in the payload
            if (id != update.DrugId)
            {
                return BadRequest("The ID in the URL does not match the DrugId in the payload.");
            }

            // Pass the update object to the MongoDB service to perform the update
            await _mongoDBService.UpdatePharmaGuideAsync(id, update);

            return NoContent();
        }

        // DELETE: api/pharmaguide/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFromPharmaGuide(string id)
        {
            var filter = Builders<PharmaGuideCollections>.Filter.Eq(d => d._id, new ObjectId(id));
            var result = await _mongoDBService.DeleteFromPharmaGuideAsync(filter);

            if (result.DeletedCount == 0)
            {
                return NotFound(new { message = "Document not found to delete" });
            }

            return NoContent(); // Successfully deleted
        }

        // GET: api/pharmaguide/searchHerbs
        [HttpGet("SearchHerbs")]
        public async Task<IActionResult> SearchPharmaGuide([FromQuery] string? herbLatinName, [FromQuery] string? botanicalName, [FromQuery] string? englishCommonName, [FromQuery] string? use)
        {
            var result = await _mongoDBService.GetPharmaGuideBySearchAsync(herbLatinName, botanicalName, englishCommonName, use);

            if (result.Any())
            {
                return Ok(result); // Respond with 200 OK and the results if any are found
            }

            return NotFound("No records found matching the search criteria."); // Respond with 404 Not Found if no results are found
        }
    }
}
