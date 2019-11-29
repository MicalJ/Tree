using System.ComponentModel.DataAnnotations;
using Tree.Common.Consts;

namespace Tree.Model.Node
{
    public class NodeData
    {
        [Required(ErrorMessage = ErrorCodes.EmptyField)]
        [MaxLength(500, ErrorMessage = ErrorCodes.MaxLength500)]
        public string Name { get; set; }

        [Required(ErrorMessage = ErrorCodes.EmptyField)]
        public int ParentId { get; set; }
    }
}
