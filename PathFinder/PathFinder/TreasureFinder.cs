using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEntity;

namespace PathFinder
{
    public class TreasureFinder
    {
        //this method would return the coordinates of the nearest treasure. treasure can be either a coin pile or a life pack
        public int[] FindNextTreasure(Cell current, Cell[,] grid, int treasure)
        {
            int[] variables = new int[4];
            variables[0] = current.x;
            variables[1] = current.y;
            variables[2] = current.y;
            variables[3] = current.x;
            int loop = 1;
            int gridSize = 20;
            int[,] checkGrid = new int[gridSize, gridSize];

            while (variables[0] > 0 || variables[1] > 0 || variables[2] < gridSize || variables[3] < gridSize) //traverse within the grid
            {
                if (variables[0] > 0) //traverse the upper rows adjacent to a cell
                {
                    variables[0]--;
                    for (int j = current.y - loop; j < loop + 1 + current.y; j++)
                    {
                        if (j < 0)
                            j = 0;

                        if (j > (gridSize - 1))
                            break;

                        if (checkGrid[variables[0], j] != 1 && grid[variables[0], j].priority == treasure)//if it has treasure and has not traversed yet
                        {
                            checkGrid[variables[0], j] = 1;
                            return new int[] { variables[0], j };
                        }
                    }                    
                }

                if (variables[1] > 0) //traverse the left columns adjacent to a cell
                {
                    variables[1]--;
                    for (int j = current.x - loop; j < loop + 1 + current.x; j++)
                    {
                        if (j < 0)
                            j = 0;

                        if (j > (gridSize - 1))
                            break;

                        if (checkGrid[j, variables[1]] != 1 && grid[j, variables[1]].priority == treasure)//if it has treasure and has not traversed yet
                        {
                            checkGrid[j, variables[1]] = 1;
                            return new int[] { j, variables[1] };
                        }
                    }    
                }

                if (variables[2] < (gridSize - 1))//traverse the right columns adjacent to a cell
                {
                    variables[2]++;
                    for (int j = current.x - loop; j < loop + 1 + current.x; j++)
                    {
                        if (j < 0)
                            j = 0;

                        if (j > (gridSize - 1))
                            break;

                        if (checkGrid[j, variables[2]] != 1 && grid[j, variables[2]].priority == treasure)//if it has treasure and has not traversed yet
                        {
                            checkGrid[j, variables[2]] = 1;
                            return new int[] { j, variables[2] };
                        }
                    }                    
                }

                if (variables[3] < (gridSize - 1))//traverse the lower rows adjacent to a cell
                {
                    variables[3]++;
                    for (int j = current.y - loop; j < loop + 1 + current.y; j++)
                    {
                        if (j < 0)
                            j = 0;

                        if (j > (gridSize - 1))
                            break;

                        if (checkGrid[variables[3], j] != 1 && grid[variables[3], j].priority == treasure)//if it has treasure and has not traversed yet
                        {
                            checkGrid[variables[3], j] = 1;
                            return new int[] { variables[3], j };
                        }
                    }    
                }

                loop++;
            }
            return null;
        }
    }
}
