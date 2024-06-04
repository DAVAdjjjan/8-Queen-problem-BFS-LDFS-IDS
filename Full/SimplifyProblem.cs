using System;

namespace Full
{
    public class SimplifyProblem
    {
        private readonly Queen[,] _board;

        public SimplifyProblem(Queen[,] board)
        {
            _board = (Queen[,])board.Clone();
        }

        public void Execute(Action<string> updateMethod)
        {
            int[] queenCountPerRow = new int[ChessBoard.Size];
            int[] queenCountPerColumn = new int[ChessBoard.Size];

            for (int row = 0; row < ChessBoard.Size; row++)
            {
                for (int col = 0; col < ChessBoard.Size; col++)
                {
                    if (_board[row, col] != null)
                    {
                        queenCountPerRow[row]++;
                        queenCountPerColumn[col]++;
                    }
                }
            }

            for (int row = 0; row < ChessBoard.Size; row++)
            {
                if (queenCountPerRow[row] <= 1)
                    continue;

                for (int col = 0; col < ChessBoard.Size; col++)
                {
                    if (_board[row, col] == null)
                        continue;

                    for (int newRow = 0; newRow < ChessBoard.Size; newRow++)
                    {
                        if (queenCountPerRow[newRow] == 0
                            && !IsQueenInRow(newRow)
                            && !IsQueenOnPath(row, col, newRow, col))
                        {
                            updateMethod(
                                $"Queen {ChessBoard.ConvertToChessCoordinate(row, col)} - {ChessBoard.ConvertToChessCoordinate(newRow, col)}");
                            _board[newRow, col] = _board[row, col];
                            _board[row, col] = null;
                            queenCountPerRow[row]--;
                            queenCountPerRow[newRow]++;
                            break;
                        }
                    }
                }
            }

            for (int col = 0; col < ChessBoard.Size; col++)
            {
                if (queenCountPerColumn[col] <= 1)
                    continue;

                for (int row = 0; row < ChessBoard.Size; row++)
                {
                    if (_board[row, col] == null)
                        continue;

                    for (int newCol = 0; newCol < ChessBoard.Size; newCol++)
                    {
                        if (queenCountPerColumn[newCol] == 0
                            && !IsQueenInColumn(newCol)
                            && !IsQueenOnPath(row, col, row, newCol))
                        {
                            updateMethod(
                                $"Queen {ChessBoard.ConvertToChessCoordinate(row, col)} - {ChessBoard.ConvertToChessCoordinate(row, newCol)}");
                            _board[row, newCol] = _board[row, col];
                            _board[row, col] = null;
                            queenCountPerColumn[col]--;
                            queenCountPerColumn[newCol]++;
                            break;
                        }
                    }
                }
            }
        }

        public Queen[,] GetBoard()
        {
            return _board;
        }

        private bool IsQueenInRow(int row)
        {
            for (int col = 0; col < ChessBoard.Size; col++)
            {
                if (_board[row, col] != null)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsQueenInColumn(int col)
        {
            for (int row = 0; row < ChessBoard.Size; row++)
            {
                if (_board[row, col] != null)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsQueenOnPath(int startRow, int startCol, int endRow, int endCol)
        {
            int rowIncrement = Math.Sign(endRow - startRow);
            int colIncrement = Math.Sign(endCol - startCol);

            int currentRow = startRow + rowIncrement;
            int currentCol = startCol + colIncrement;

            while (currentRow != endRow || currentCol != endCol)
            {
                if (_board[currentRow, currentCol] != null)
                {
                    return true;
                }

                currentRow += rowIncrement;
                currentCol += colIncrement;
            }

            return false;
        }
    }
}
