using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightMoves
{
    struct DataSet
    {
        public int numberOfBlockedSquares;
        public string[] blockedSquares;
        public string initialSquare;
        public string finalSquare;
    }

    class KnightMoves
    {
        int[] dx = { 2, 1, -1, -2, -2, -1, 1, 2 };
        int[] dy = { 1, 2, 2, 1, -1, -2, -2, -1 };

        DataSet dataSet;
        int[] initialPosition;
        int[] finalPosition;

        int moves = int.MaxValue;
        int[,] board;
        int[,] solutionBoard;


        public KnightMoves(DataSet dataSet)
        {
            this.dataSet = dataSet;
            initialPosition = GetMatrixPosition(dataSet.initialSquare);
            finalPosition = GetMatrixPosition(dataSet.finalSquare);

            board = new int[8, 8];
            solutionBoard = new int[8, 8];

            for (int i = 0; i < dataSet.blockedSquares.Length; i++)
            {
                int[] position = GetMatrixPosition(dataSet.blockedSquares[i]);
                board[position[0], position[1]] = -1;
            }
        }

        private int[] GetMatrixPosition(string square)
        {
            string yPossibilities = "abcdefgh";

            int y = yPossibilities.IndexOf(square[0].ToString());
            int x = Math.Abs(int.Parse(square[1].ToString()) - 8);

            return new int[] { x, y };
        }

        private void PrintMatrix(int [,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(board[i, j].ToString().PadLeft(2, ' ') + " ");
                }
                Console.WriteLine();
            }
        }

        public int countMovements()
        {
            // Console.WriteLine("Initial state of the board:");
            // PrintMatrix(board);

            board[initialPosition[0], initialPosition[1]] = 1;
            tryMovement(2, initialPosition[0], initialPosition[1]);
            
            if (moves == int.MaxValue) return 0;

            // Console.WriteLine("Solution:");
            // PrintMatrix(solutionBoard);

            return moves;
        }

        private bool tryMovement(int move, int x, int y)
        {
            bool found = IsFinalPosition(x, y);
            int k = 0, u, v;

            if (found)
            {
                moves = board[x, y] - 1;
                solutionBoard = (int[,]) board.Clone();
            }

            while (!found && move < moves  &&  k < 8)
            {
                u = x + dx[k];
                v = y + dy[k];

                if (IsValidMovement(u, v))
                {
                    board[u, v] = move;
                    tryMovement(move + 1, u, v);
                    board[u, v] = 0;
                }

                k++;
            }

            return found;
        }

        private bool IsValidMovement(int x, int y)
        {
            return (x >= 0 && x <= 7) && (y >= 0 && y <= 7) && board[x, y] == 0;
        }

        private bool IsFinalPosition(int x, int y)
        {
            return finalPosition[0] == x && finalPosition[1] == y;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<DataSet> dataSets = LoadStaticDataSets(); // This method laods the input based on use cases in memory, which makes it easier to debug
            // List<DataSet> dataSets = LoadDataSetsFromConsole(); // This method loads the input as specified by the problem

            for (int i = 0; i < dataSets.Count; i++)
            {
                KnightMoves instance = new KnightMoves(dataSets[i]);
                int moves = instance.countMovements();
                if (moves > 0)
                {
                    Console.WriteLine("Board {0}: {1} moves", i + 1, moves);
                }
                else
                {
                    Console.WriteLine("Board {0}: not reacheable", i + 1);
                }
            }

            Console.ReadKey();
        }

        static List<DataSet> LoadDataSetsFromConsole()
        {
            List<DataSet> dataSets = new List<DataSet>();

            int numberOfBlockedSquares = 0;

            do
            {
                Console.WriteLine("Number of blocked squares");
                numberOfBlockedSquares = int.Parse(Console.ReadLine());
                if (numberOfBlockedSquares != -1)
                {
                    Console.WriteLine("Blocked squares");
                    string blockedSquares = Console.ReadLine();

                    Console.WriteLine("Knight positions");
                    string[] knightPositions = Console.ReadLine().Split(' ');

                    dataSets.Add(new DataSet
                    {
                        numberOfBlockedSquares = numberOfBlockedSquares,
                        blockedSquares = blockedSquares.Split(' '),
                        initialSquare = knightPositions[0],
                        finalSquare = knightPositions[1]
                    });
                }
            } while (numberOfBlockedSquares != -1);

            return dataSets;
        }

        static List<DataSet> LoadStaticDataSets()
        {
            List<DataSet> dataSets = new List<DataSet>();

            dataSets.Add(new DataSet
            {
                numberOfBlockedSquares = 10,
                blockedSquares = "c1 d1 d5 c2 c3 c4 d2 d3 d4 c5".Split(' '),
                initialSquare = "a1 f1".Split(' ')[0],
                finalSquare = "a1 f1".Split(' ')[1]
            });

            dataSets.Add(new DataSet
            {
                numberOfBlockedSquares = 0,
                blockedSquares = new string[0],
                initialSquare = "c1 b3".Split(' ')[0],
                finalSquare = "c1 b3".Split(' ')[1]
            });
            
            dataSets.Add(new DataSet
            {
                numberOfBlockedSquares = 2,
                blockedSquares = "b3 c2".Split(' '),
                initialSquare = "a1 b2".Split(' ')[0],
                finalSquare = "a1 b2".Split(' ')[1]
            });
            
            return dataSets;
        }
    }
}
