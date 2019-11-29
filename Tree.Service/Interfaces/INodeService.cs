using System.Collections.Generic;
using System.Threading.Tasks;
using Tree.Model.Node;

namespace Tree.Service.Interfaces
{
    public interface INodeService
    {
        Task AddAsync(NodeData request);
        Task UpdateAsync(int id,NodeData request);
        Task<NodeSingle> GetByIdAsync(int id);
        Task<List<NodeWithId>> GetAsync();
        Task DeleteAsync(int id);
        Task AddRootAsync();
    }
}
