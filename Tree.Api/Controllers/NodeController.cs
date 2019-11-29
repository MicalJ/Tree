using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tree.Model.Node;
using Tree.Service.Interfaces;

namespace Tree.Api.Controllers
{
    [Route("api/[controller]")]
    public class NodeController : Controller
    {
        private readonly INodeService _nodeService;

        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] NodeData request)
        {
            await _nodeService.AddAsync(request);

            return StatusCode(201);
        }

        [HttpPut("update/{id}")]
        public async Task Update(int id, [FromBody]NodeData request)
        {
            await _nodeService.UpdateAsync(id, request);
        }

        [HttpGet]
        public async Task<List<NodeWithId>> Get()
        {
            return await _nodeService.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<NodeSingle> GetById(int id)
        {
            return await _nodeService.GetByIdAsync(id);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _nodeService.DeleteAsync(id);
        }
    }
}
