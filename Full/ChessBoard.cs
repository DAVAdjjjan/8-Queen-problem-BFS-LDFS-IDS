using System;

namespace Full
{
    public class ChessBoard
    {
        public const int Size = 8;
        private readonly Queen[,] _board;
        private readonly Random _random;

        public ChessBoard()
        {
            _board = new Queen[Size, Size];
            _random = new Random();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    _board[row, col] = null;
                }
            }
        }

        public void PlaceQueensRandomly()
        {
            InitializeBoard();
            for (int i = 0; i < Size; i++)
            {
                int randomRow, randomCol;
                do
                {
                    randomRow = _random.Next(0, Size);
                    randomCol = _random.Next(0, Size);
                } while (_board[randomRow, randomCol] != null);
                _board[randomRow, randomCol] = new Queen(randomRow, randomCol);
            }
        }

        public TreeNode GenerateMoveTree()
        {
            Queen[,] currentBoard = GetBoardCopy();
            TreeNode root = new TreeNode(currentBoard);
            GenerateMoveTreeRecursive(root, 0);
            return root;
        }

        public void GenerateMoveTreeRecursive(TreeNode node, int queenIndex)
        {
            if (queenIndex >= Size) return;

            Queen[,] currentBoard = node.Board;
            int row = queenIndex;
            int originalCol = -1;

            for (int col = 0; col < Size; col++)
            {
                if (currentBoard[row, col] != null)
                {
                    originalCol = col;
                    break;
                }
            }

            if (originalCol == -1) return;

            for (int newCol = 0; newCol < Size; newCol++)
            {
                if (newCol == originalCol) continue;
                Queen[,] newBoard = (Queen[,])currentBoard.Clone();
                newBoard[row, originalCol] = null;
                newBoard[row, newCol] = new Queen(row, newCol);

                string move = $"Queen {ConvertToChessCoordinate(row, originalCol)} - {ConvertToChessCoordinate(row, newCol)}";

                TreeNode childNode = new TreeNode(newBoard, node, move);
                node.Children.Add(childNode);

                GenerateMoveTreeRecursive(childNode, queenIndex + 1);
            }
        }

        public Queen[,] GetBoardCopy()
        {
            Queen[,] copy = new Queen[Size, Size];
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (_board[row, col] != null)
                    {
                        copy[row, col] = new Queen(_board[row, col].Row, _board[row, col].Col);
                    }
                }
            }
            return copy;
        }

        public static string ConvertToChessCoordinate(int row, int col)
        {
            char columnChar = (char)('a' + col);
            int rowNumber = Size - row;
            return $"{columnChar}{rowNumber}";
        }
    }
}
