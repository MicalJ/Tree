using System.Collections.Generic;
using System.Threading.Tasks;
using Tree.Model.Node;
using Tree.Repository.Interfaces;
using Tree.Service.Interfaces;

namespace Tree.Service.Services
{
    public class FrameworkService : INodeService
    {
        private readonly INodeRepository _nodeRepository;

        public FrameworkService(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        public async Task AddAsync(NodeData request)
        {
            await _nodeRepository.AddAsync(request);
        }

        public async Task UpdateAsync(int id, NodeData request)
        {
            await _nodeRepository.UpdateAsync(id, request);
        }

        public async Task<NodeSingle> GetByIdAsync(int id)
        {
            return await _nodeRepository.GetByIdAsync(id);
        }

        public async Task<List<NodeWithId>> GetAsync()
        {
            return await _nodeRepository.GetAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _nodeRepository.DeleteAsync(id);
        }

        public async Task AddRootAsync()
        {
            if (await _nodeRepository.IsExistAsync())
            {
                return;
            }

            await _nodeRepository.AddRootAsync();
        }
    }
}
