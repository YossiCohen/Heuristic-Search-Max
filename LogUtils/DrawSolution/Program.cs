using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DrawSolution
{
    class Program
    {
        private const char START_CHAR = '@';
        private const char END_CHAR = '*';
        private const char BLOCK_CHAR = '#'; 
        
        private const char UP = '↑';
        private const char DOWN = '↓';
        private const char RIGHT = '→';
        private const char LEFT = '←';

        private static char[][] _mazeFile;
        private static char[][] _resultMaze;
        private static List<string> _steps;

        private static Location _current;
        private static Location _start; //@
        private static Location _end; //*
        private static string _mazeFileName;
        private static string _algorithm;
        private static string _solutionLength;
        private static int _numberOfFree;
        private static int _numberOfBlocked;
        private static int _numberOfStepped;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Arguments: Draw solution <SolutionFile>");
                return;
            }
            InitValues(args);

            ProcessStepsinMaze();

            SaveResultsToFile(args);
        }

        private static void ProcessStepsinMaze()
        {
            _current = new Location(_start);
            Location next;
            Location.Direction dir;
            char dirChar;
            
            foreach (var step in _steps)
            {
                next = new Location(step);
                dir = _current.getDirectionTo(next);
                switch (dir)
                {
                    case Location.Direction.Up:
                        dirChar = UP;
                        break;
                    case Location.Direction.Down:
                        dirChar = DOWN;
                        break;
                    case Location.Direction.Right:
                        dirChar = RIGHT;
                        break;
                    case Location.Direction.Left:
                        dirChar = LEFT;
                        break;
                    default:
                        dirChar = '?';
                        break;
                }
                _resultMaze[_current.Y][_current.X] = dirChar;
                _current = next;
                _numberOfStepped++;
            }
            _numberOfStepped++; //start point is stepped
        }

        private static void SaveResultsToFile(string[] args)
        {
            StringBuilder all = new StringBuilder();
            all.Append(_mazeFileName);
            all.Append(Environment.NewLine);
            all.Append("Maze:");
            all.Append(Environment.NewLine);
            all.Append(mazeToString(_mazeFile));
            all.Append(Environment.NewLine);
            all.Append("Route:");
            all.Append(Environment.NewLine);
            all.Append(mazeToString(_resultMaze));
            all.Append(Environment.NewLine);
            all.Append("Algorithm:");
            all.Append(_algorithm);
            all.Append(Environment.NewLine);
            all.Append("Length:");
            all.Append(_solutionLength);
            all.Append(Environment.NewLine);
            all.Append("Number Of Free:");
            all.Append(_numberOfFree);
            all.Append(Environment.NewLine);
            all.Append("Number Of Blocked:");
            all.Append(_numberOfBlocked);
            all.Append(Environment.NewLine);
            all.Append("Number Of Stepped:");
            all.Append(_numberOfStepped);
            all.Append(Environment.NewLine);
            all.Append("Number Of UnStepped:");
            all.Append(_numberOfFree - _numberOfStepped);
            System.IO.File.WriteAllText(args[0] + ".visualSolution", all.ToString(), Encoding.UTF8);
        }

        private static string mazeToString(char[][] maze)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            foreach (var row in maze)
            {
                foreach (var cell in row)
                {
                    sb.Append(cell);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private static void InitValues(string[] args)
        {
            _numberOfFree = 0;
            _numberOfBlocked = 0;
            _numberOfStepped = 0;

            _steps = ReadStepsAndParams(args[0]);
            _mazeFileName = args[0].Split(new char[] { '_' },2)[1];
            _mazeFileName = _mazeFileName.Substring(0, _mazeFileName.Length - 20);
            _mazeFile = ReadMaze(_mazeFileName);
            _resultMaze = ReadMaze(_mazeFileName);

            for (int i = 0; i < _mazeFile.Length; i++)
            {
                for (int j = 0; j < _mazeFile[i].Length; j++)
                {
                    if (_mazeFile[i][j] == START_CHAR)
                    {
                        _start = new Location(i, j);
                    }
                    else if (_mazeFile[i][j] == END_CHAR)
                    {
                        _end = new Location(i, j);
                    }
                    else if (_mazeFile[i][j] == BLOCK_CHAR)
                    {
                        _numberOfBlocked++;
                        continue; //skip num of free count
                    }
                    _numberOfFree++;
                }
            }
        }

        private static List<string> ReadStepsAndParams(string filename)
        {
            List<string> lines = new List<string>();
            using (var sr = File.OpenText(filename))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("~node::"))
                    {
                        lines.Add(line.Substring(8));
                    }
                    else if (line.StartsWith("#pair \"solution length\""))
                    {
                        _solutionLength = line.Split('\t')[1].Split('\"')[1];
                    }
                    else if (line.StartsWith("#pair \"algorithm\""))
                    {
                        _algorithm = line.Split('\t')[1].Split('\"')[1];
                    }
                }
            }

            lines.Reverse();
            return lines;
        }

        private static char[][] ReadMaze(string filename)
        {
            return File.ReadLines(filename).Select(l => l.ToArray()).ToArray();
        }
    }
}
