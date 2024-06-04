using System.Collections.Generic;

namespace Full
{
    public class SearchAlgorithms
    {
        public static SearchResult BFS(TreeNode root)
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();
           
            int nodesProcessed = 0;

            queue.Enqueue(root);
          
            nodesProcessed++;

            while (queue.Count > 0)
            {
                TreeNode node = queue.Dequeue();
                if (IsValidBoard(node.Board))
                {
                    return new SearchResult(node, nodesProcessed);
                }

                foreach (TreeNode child in node.Children)
                {
                   queue.Enqueue(child);
                   nodesProcessed++;
                }
            }
            return new SearchResult(null, nodesProcessed);
        }

        public static SearchResult IDDFS(TreeNode root)
        {
            int depth = 0;
            int nodesProcessed = 0;
            int lastNodesProcessed = -1;

            while (nodesProcessed != lastNodesProcessed)
            {
                lastNodesProcessed = nodesProcessed;
               
                var result = DLS(root, depth, ref nodesProcessed);
                if (result.Node != null)
                {
                    return new SearchResult(result.Node, nodesProcessed);
                }
                depth++;
            }

            return new SearchResult(null, nodesProcessed);
        }

        public static SearchResult LDFS(TreeNode root, int depthLimit)
        {
            int nodesProcessed = 0;
           
            return DLS(root, depthLimit, ref nodesProcessed);
        }

        public static SearchResult DLS(TreeNode node, int depth, ref int nodesProcessed)
        {
            nodesProcessed++;
            if (IsValidBoard(node.Board))
            {
                return new SearchResult(node, nodesProcessed);
            }
            if (depth == 0)
            {
                return new SearchResult(null, nodesProcessed);
            }


            foreach (TreeNode child in node.Children)
            {
                var result = DLS(child, depth - 1, ref nodesProcessed);

                if (result.Node != null)
                {
                    return new SearchResult(result.Node, nodesProcessed);
                }
            }

            return new SearchResult(null, nodesProcessed);
        }

        private static bool IsValidBoard(Queen[,] board)
        {
            for (int row = 0; row < ChessBoard.Size; row++)
            {
                for (int col = 0; col < ChessBoard.Size; col++)
                {
                    if (board[row, col] != null)
                    {
                        if (IsThreatened(board, row, col))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static bool IsThreatened(Queen[,] board, int row, int col)
        {
            for (int i = 0; i < ChessBoard.Size; i++)
            {
                if (i != col && board[row, i] != null)
                    return true;
                if (i != row && board[i, col] != null)
                    return true;
            }

            for (int i = -ChessBoard.Size; i < ChessBoard.Size; i++)
            {
                if (row + i >= 0 && row + i < ChessBoard.Size && col + i >= 0 && col + i < ChessBoard.Size)
                {
                    if (i != 0 && board[row + i, col + i] != null)
                        return true;
                }
                if (row + i >= 0 && row + i < ChessBoard.Size && col - i >= 0 && col - i < ChessBoard.Size)
                {
                    if (i != 0 && board[row + i, col - i] != null)
                        return true;
                }
            }

            return false;
        }
    }
}
