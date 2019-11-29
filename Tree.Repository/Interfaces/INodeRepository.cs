using System.Collections.Generic;
using System.Threading.Tasks;
using Tree.Model.Node;

namespace Tree.Repository.Interfaces
{
    public interface INodeRepository
    {
        Task AddAsync(NodeData request);
        Task UpdateAsync(int id, NodeData request);
        Task<NodeSingle> GetByIdAsync(int id);
        Task<List<NodeWithId>> GetAsync();
        Task DeleteAsync(int id);
        Task<bool> IsExistAsync(int? parentId = null);
        Task AddRootAsync();
    }
}
