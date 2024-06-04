using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System;
using System.IO;

namespace Full
{
    public partial class Form1 : Form
    {
        private const int BoardSize = 8;
        private const int MaxQueens = 8;
        private readonly Button[,] queensPlacementButtons = new Button[BoardSize, BoardSize];
        private readonly Queen[,] queens = new Queen[BoardSize, BoardSize];
        private int queenCount = 0;
        private Button simplifyButton;
        private Button clearButton;
        private Button randomPlaceButton;
        private Button bfsButton;
        private Button iddfsButton;
        private Button ldfsButton;
        private ListBox stepsListBox;
        private Button exitButton;
        private Label waitLabel;
        private bool simplified = false;
        private Label nodesProcessedLabel;
        private Button saveButton;
        private Queen[,] initialBoard;
        
        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
            InitializeSimplifyButton();
            InitializeClearButton();
            InitializeStepsListBox();
            InitializeCoordinates();
            InitializeRandomPlaceButton();
            InitializeExitButton();
            InitializeBFSButton();
            InitializeIDDFSButton();
            InitializeLDFSButton();
            InitializeNodesProcessedLabel();
            InitializeSaveButton();
        }



        private void InitializeBoard()
        {
            int buttonSize = ClientSize.Width / 2 / BoardSize;
            ClientSize = new Size(BoardSize * buttonSize * 2, BoardSize * buttonSize + 250);

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    queensPlacementButtons[row, col] = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(col * buttonSize + 30, row * buttonSize + 30),
                        Font = new Font("Arial", 24, FontStyle.Bold),
                        Tag = new Point(row, col)
                    };

                    queensPlacementButtons[row, col].Click += queensPlacementButtons_Click;

                    queensPlacementButtons[row, col].BackColor = (row + col) % 2 == 0 ? Color.Tan : Color.Sienna;

                    Controls.Add(queensPlacementButtons[row, col]);
                }
            }
        }

        private void InitializeCoordinates()
        {
            int buttonSize = ClientSize.Width / 2 / BoardSize;

            for (int col = 0; col < BoardSize; col++)
            {
                Label label = new Label
                {
                    Text = ((char)('a' + col)).ToString(),
                    Size = new Size(buttonSize, 20),
                    Location = new Point(col * buttonSize + 30 + buttonSize / 2 - 5, 10),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Controls.Add(label);
            }

            for (int row = 0; row < BoardSize; row++)
            {
                Label label = new Label
                {
                    Text = (BoardSize - row).ToString(),
                    Size = new Size(20, buttonSize),
                    Location = new Point(10, row * buttonSize + 30 + buttonSize / 2 - 10),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Controls.Add(label);
            }
        }

        private void InitializeSimplifyButton()
        {
            simplifyButton = new Button
            {
                Text = "Simplify problem",
                Size = new Size(150, 50),
                Location = new Point((BoardSize + 1) * queensPlacementButtons[0, 0].Width + 30,
                           queensPlacementButtons[0, 0].Height / 2),
                Visible = false
            };
            simplifyButton.Click += RearrangeButton_Click;
            Controls.Add(simplifyButton);
        }

        private void InitializeClearButton()
        {
            clearButton = new Button
            {
                Text = "Clear Board and Steps",
                Size = new Size(200, 50),
                Location = new Point(ClientSize.Width / 2 - 100, this.ClientSize.Height - 70)
            };
            clearButton.Click += ClearButton_Click;
            Controls.Add(clearButton);
        }

        private void InitializeStepsListBox()
        {
            stepsListBox = new ListBox
            {
                Location = new Point(0, BoardSize * queensPlacementButtons[0, 0].Height + 40),
                Size = new Size(this.ClientSize.Width, 100)
            };
            Controls.Add(stepsListBox);
        }

        private void InitializeRandomPlaceButton()
        {
            randomPlaceButton = new Button
            {
                Text = "Random Place Queens",
                Size = new Size(150, 50),
                Location = new Point((BoardSize + 1) * queensPlacementButtons[0, 0].Width + 30, queensPlacementButtons[0, 0].Height * 3 / 2)
            };
            randomPlaceButton.Click += RandomPlaceButton_Click;
            Controls.Add(randomPlaceButton);
        }

        private void InitializeExitButton()
        {
            exitButton = new Button
            {
                Text = "Exit",
                Size = new Size(100, 50),
                Location = new Point(clearButton.Location.X + clearButton.Width + 10, clearButton.Location.Y)
            };
            exitButton.Click += ExitButton_Click;
            Controls.Add(exitButton);
        }

        private void InitializeBFSButton()
        {
            bfsButton = new Button
            {
                Text = "Run BFS",
                Size = new Size(150, 50),
                Location = new Point((BoardSize + 1) * queensPlacementButtons[0, 0].Width + 30, queensPlacementButtons[0, 0].Height * 5 / 2),
                Visible = false
            };
            bfsButton.Click += BFSButton_Click;
            Controls.Add(bfsButton);

            waitLabel = new Label
            {
                Text = "Please wait...",
                Size = new Size(150, 50),
                Location = new Point(bfsButton.Location.X, bfsButton.Location.Y + 60),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            Controls.Add(waitLabel);
        }

        private void InitializeIDDFSButton()
        {
            iddfsButton = new Button
            {
                Text = "Run IDDFS",
                Size = new Size(150, 50),
                Location = new Point((BoardSize + 1) * queensPlacementButtons[0, 0].Width + 30, queensPlacementButtons[0, 0].Height * 7 / 2),
                Visible = false
            };
            iddfsButton.Click += IDDFSButton_Click;
            Controls.Add(iddfsButton);

            waitLabel = new Label
            {
                Text = "Please wait...",
                Size = new Size(150, 50),
                Location = new Point(iddfsButton.Location.X, iddfsButton.Location.Y + 60),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            Controls.Add(waitLabel);
        }

        private void InitializeLDFSButton()
        {
            ldfsButton = new Button
            {
                Text = "Run LDFS",
                Size = new Size(150, 50),
                Location = new Point((BoardSize + 1) * queensPlacementButtons[0, 0].Width + 30, queensPlacementButtons[0, 0].Height * 9 / 2),
                Visible = false
            };
            ldfsButton.Click += LDFSButton_Click;
            Controls.Add(ldfsButton);

            waitLabel = new Label
            {
                Text = "Please wait...",
                Size = new Size(150, 50),
                Location = new Point(ldfsButton.Location.X, ldfsButton.Location.Y + 60),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            Controls.Add(waitLabel);
        }

        private void InitializeNodesProcessedLabel()
        {
            nodesProcessedLabel = new Label
            {
                Size = new Size(200, 50),
                Location = new Point((BoardSize + 1) * queensPlacementButtons[0, 0].Width + 30, queensPlacementButtons[0, 0].Height * 11 / 2),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            Controls.Add(nodesProcessedLabel);
        }



        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void queensPlacementButtons_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Point position = (Point)button.Tag;

            if (!simplified)
            {
                if (queens[position.X, position.Y] == null)
                {
                    PlaceQueen(position.X, position.Y);
                }
                else
                {
                    RemoveQueen(position.X, position.Y);
                }

                simplifyButton.Visible = queenCount == MaxQueens;
                CheckQueensPlacement();
            }
        }

        private void PlaceQueen(int row, int col)
        {
            if (queenCount < MaxQueens)
            {
                if (queens[row, col] == null)
                {
                    queens[row, col] = new Queen(row, col);
                    queensPlacementButtons[row, col].Text = "♕";
                    queensPlacementButtons[row, col].ForeColor = Color.Red;
                    queenCount++;
                }
            }
            else
            {
                MessageBox.Show("You cannot place more than 8 queens on the board.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveQueen(int row, int col)
        {
            if (queens[row, col] != null)
            {
                queens[row, col] = null;
                queensPlacementButtons[row, col].Text = "";
                queenCount--;
            }
        }

        private void InitializeSaveButton()
        {
            saveButton = new Button
            {
                Text = "Save to File",
                Size = new Size(150, 50),
                Location = new Point(nodesProcessedLabel.Location.X, nodesProcessedLabel.Location.Y + 60),
                Visible = false
            };
            saveButton.Click += SaveButton_Click;
            Controls.Add(saveButton);
        }

        private void SaveInitialBoard()
        {
            initialBoard = new Queen[BoardSize, BoardSize];
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (queens[row, col] != null)
                    {
                        initialBoard[row, col] = new Queen(queens[row, col].Row, queens[row, col].Col);
                    }
                }
            }
        }

        private void RearrangeButton_Click(object sender, EventArgs e)
        {
            if (initialBoard == null)
            {
                SaveInitialBoard();
            }

            SimplifyProblem rearrangement = new SimplifyProblem(queens);
            List<string> steps = new List<string>();
            rearrangement.Execute(step => steps.Add(step));
            UpdateBoard(rearrangement.GetBoard());

            foreach (var step in steps)
            {
                stepsListBox.Items.Add(step);
            }

            simplified = true;
            randomPlaceButton.Visible = false;
        }

        private void RandomPlaceButton_Click(object sender, EventArgs e)
        {
            ChessBoard chessBoard = new ChessBoard();
            chessBoard.PlaceQueensRandomly();
            Queen[,] randomBoard = chessBoard.GetBoardCopy();
            UpdateBoard(randomBoard);

            CheckQueensPlacement();
        }

        private void UpdateBoard(Queen[,] newBoard)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (queens[row, col] == null && newBoard[row, col] != null)
                    {
                        queensPlacementButtons[row, col].ForeColor = Color.Red;
                    }
                    queens[row, col] = newBoard[row, col];
                    queensPlacementButtons[row, col].Text = queens[row, col] != null ? "♕" : "";
                }
            }

            queenCount = newBoard.Cast<Queen>().Count(queen => queen != null);
            simplifyButton.Visible = queenCount == MaxQueens;
            CheckQueensPlacement();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    queens[row, col] = null;
                    queensPlacementButtons[row, col].Text = "";
                }
            }

            stepsListBox.Items.Clear();
            simplified = false;
            queenCount = 0;
            simplifyButton.Visible = false;
            randomPlaceButton.Visible = true;
            bfsButton.Visible = false;
            iddfsButton.Visible = false;
            ldfsButton.Visible = false;
            nodesProcessedLabel.Visible = false;
            nodesProcessedLabel.Text = "";
            saveButton.Visible = false;
            initialBoard = null;
        }

        private void CheckQueensPlacement()
        {
            bool allRows = true;
            bool allCols = true;

            for (int i = 0; i < BoardSize; i++)
            {
                bool rowHasQueen = false;
                bool colHasQueen = false;

                for (int j = 0; j < BoardSize; j++)
                {
                    if (queens[i, j] != null)
                    {
                        rowHasQueen = true;
                    }
                    if (queens[j, i] != null)
                    {
                        colHasQueen = true;
                    }
                }

                if (!rowHasQueen)
                {
                    allRows = false;
                }
                if (!colHasQueen)
                {
                    allCols = false;
                }
            }

            if (allRows && allCols)
            {
                bfsButton.Visible = true;
                iddfsButton.Visible = true;
                ldfsButton.Visible = true;
                simplifyButton.Visible = false;
            }
            else
            {
                bfsButton.Visible = false;
                iddfsButton.Visible = false;
                ldfsButton.Visible = false;
            }
        }


        private void BFSButton_Click(object sender, EventArgs e)
        {
            RunSearchAlgorithm(SearchAlgorithms.BFS);
        }

        private void IDDFSButton_Click(object sender, EventArgs e)
        {
            RunSearchAlgorithm(SearchAlgorithms.IDDFS);
        }

        private void LDFSButton_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the maximum depth(1-9):", "LDFS Maximum Depth", "9");
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            if (!int.TryParse(input, out int depthLimit) || depthLimit <= 0 || depthLimit > 9 )
            {
                MessageBox.Show("Invalid input. Please enter a positive integer(1-9) .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RunSearchAlgorithm(root => SearchAlgorithms.LDFS(root, depthLimit));
        }

        private void RunSearchAlgorithm(Func<TreeNode, SearchResult> searchAlgorithm)
        {
            if (initialBoard == null)   
            {
                SaveInitialBoard();
            }

            ldfsButton.Visible = false;
            iddfsButton.Visible = false;
            bfsButton.Visible = false;
            randomPlaceButton.Visible = false;
            waitLabel.Visible = true;
            nodesProcessedLabel.Visible = false;
            Refresh();

            ChessBoard chessBoard = new ChessBoard();
            Queen[,] currentBoard = new Queen[BoardSize, BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (queens[row, col] != null)
                    {
                        currentBoard[row, col] = new Queen(queens[row, col].Row, queens[row, col].Col);
                    }
                }
            }

            TreeNode root = new TreeNode(currentBoard);
            chessBoard.GenerateMoveTreeRecursive(root, 0);

            SearchResult result = searchAlgorithm(root);

            if (result.Node != null)
            {
                Stack<string> steps = new Stack<string>();
                TreeNode currentNode = result.Node;
                while (currentNode.Parent != null)
                {
                    steps.Push(currentNode.Move);
                    currentNode = currentNode.Parent;
                }

                while (steps.Count > 0)
                {
                    stepsListBox.Items.Add(steps.Pop());
                }

                UpdateBoard(result.Node.Board);

                nodesProcessedLabel.Text = $"Nodes processed: {result.NodesProcessed}";
                nodesProcessedLabel.Visible = true;
                saveButton.Visible = true;

                bfsButton.Visible = false;
                iddfsButton.Visible = false;
                ldfsButton.Visible = false;
            }
            else
            {
                nodesProcessedLabel.Text = $"No solution found! Nodes processed: {result.NodesProcessed}";
                nodesProcessedLabel.Visible = true;

                bfsButton.Visible = true;
                iddfsButton.Visible = true;
                ldfsButton.Visible = true;
            }

            waitLabel.Visible = false;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filePath = Path.Combine(desktopPath, $"chess_solution_{timestamp}.txt");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Initial Board:");
                WriteBoardToFile(writer, initialBoard);

                writer.WriteLine("\nSteps:");
                foreach (var step in stepsListBox.Items)
                {
                    writer.WriteLine(step.ToString());
                }

                writer.WriteLine("\nFinal Board:");
                WriteBoardToFile(writer, queens);

                writer.WriteLine($"\n {nodesProcessedLabel.Text}");
            }

            MessageBox.Show($"Solution saved to {filePath}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            saveButton.Visible = false;
        }


        private void WriteBoardToFile(StreamWriter writer, Queen[,] board)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    writer.Write(board[row, col] != null ? "♕ " : ". ");
                }
                writer.WriteLine();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}