using Microsoft.AspNetCore.Mvc;
using ReportService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMemoryReportStorage memoryReportStorage;

        public ReportController(IMemoryReportStorage memoryReportStorage)
        {
            this.memoryReportStorage = memoryReportStorage;
        }

        // GET: api/<ReportController>
        [HttpGet]
        public IEnumerable<Report> Get()
        {
            return memoryReportStorage.Get();
        }

        // GET api/<ReportController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReportController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReportController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReportController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
