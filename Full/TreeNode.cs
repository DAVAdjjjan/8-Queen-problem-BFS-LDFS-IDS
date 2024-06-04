using System.Collections.Generic;

namespace Full
{
    public class TreeNode
    {
        public Queen[,] Board { get; }
        public List<TreeNode> Children { get; }
        public TreeNode Parent { get; }
        public string Move { get; }

        public TreeNode(Queen[,] board, TreeNode parent = null, string move = null)
        {
            Board = (Queen[,])board.Clone();
            Children = new List<TreeNode>();
            Parent = parent;
            Move = move;
        }
    }
}
