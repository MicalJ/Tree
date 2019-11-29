using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tree.Database;
using Tree.Database.DbModels;
using Tree.Model.Node;
using Tree.Repository.Interfaces;

namespace Tree.Repository.Repositories
{
    public class NodeRepository : INodeRepository
    {
        private readonly TreeContext _context;

        public NodeRepository(TreeContext treeContext)
        {
            _context = treeContext;
        }

        public async Task AddAsync(NodeData request)
        {
            await _context.AddAsync(new Node
            {
                Name = request.Name,
                ParentId = request.ParentId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, NodeData request)
        {
            var nodeEntity = await GetEntityByIdAsync(id);

            nodeEntity.Name = request.Name;
            nodeEntity.ParentId = request.ParentId;
            nodeEntity.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<NodeSingle> GetByIdAsync(int id)
        {
            return await _context.Set<Node>()
                .AsNoTracking()
                .Where(w => w.Id == id && !w.IsDeleted)
                .Select(s => new NodeSingle
                {
                    Name = s.Name,
                    ParentId = s.ParentId
                }).FirstOrDefaultAsync();
        }

        public async Task<List<NodeWithId>> GetAsync()
        {
            return await _context.Set<Node>()
                .AsNoTracking()
                .Where(w => !w.IsDeleted)
                .Select(s => new NodeWithId
                {
                    Id = s.Id,
                    Name = s.Name,
                    ParentId = s.ParentId
                }).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nodeEntity = await GetEntityByIdAsync(id);

            nodeEntity.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(int? parentId = null)
        {
            return await _context.Set<Node>()
                .AnyAsync(a => a.ParentId == parentId);
        }

        private async Task<Node> GetEntityByIdAsync(int id)
        {
            var nodeEntity = await _context.Set<Node>()
               .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

            if (nodeEntity == null)
            {
                throw new Exception("Cannot find node");
            }

            return nodeEntity;
        }
    }
}
