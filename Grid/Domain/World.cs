
namespace Grid.Domain
{
    public class World
    {
        
        public World(string gridString)
        {
            
        }

//        void readGrid(string filename)
//        {
//            string line;
//            ifstream myfile(filename);
//            if (myfile.is_open())
//            {
//                while (getline(myfile, line))
//                {
//                    grid.push_back(line);
//                    for (unsigned int i = 0; i < line.length(); i++)
//                    {
//                        if (line[i] == '@')
//                        {
//                            agent.y = grid.size() - 1;
//                            agent.x = i;
//                        }
//                        else if (line[i] == '*')
//                        {
//                            goal.y = grid.size() - 1;
//                            goal.x = i;
//                        }
//                    }
//                }
//                myfile.close();
//            }
//            height = grid.size();
//            width = grid[0].length();
//
//            for (unsigned int i = 0; i < height; i++)
//            {
//                assert(width == grid[i].length());
//            }
//            binaryGridSize = ceil((double)(width * height) / 8);
//        }
    }
}
