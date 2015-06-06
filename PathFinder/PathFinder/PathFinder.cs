using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using GameEntity;

namespace PathFinder
{   

    public class PathFinder
    {
        List<Cell> openList;// The set of tentative nodes to be evaluated, initially containing the start node
        List<Cell> closedList;// The set of nodes already evaluated.
        private Grid grid;

        public PathFinder()
        {
            grid = Grid.getInstance();
        }

        //this method calculates 2 paths: path with bricks, path without bricks and selects the optimum one
        public List<Cell> FindPath(Cell start, Cell end)
        {
            List<Cell> withBricks = FindAPath(start, end, true);//whether there is a path with bricks
            List<Cell> withoutBricks = FindAPath(start, end, false);  //whether there is a path without bricks  
            int countWithBricks = 0;
            int countWithoutBricks = 0;

            if (withBricks != null && withoutBricks != null)//if we have both paths
            {
                countWithBricks = withBricks.Count - 1;
                countWithoutBricks = withoutBricks.Count - 1;
                foreach (Cell n in withBricks)
                {
                    if (n.priority == 1)//path has a brick cell
                    {
                        countWithBricks += 3; //every brick wall counts 4 moves
                    }
                }
            }

            if (countWithBricks < countWithoutBricks || withoutBricks == null)//either path with brick is less than path without brick or no path without brick
            {
                return withBricks;
            }
            else//either path with brick is greater than path without brick or no path with brick
            {
                return withoutBricks;
            }
        }

        //return the the estimated movement cost to move from that given square on the grid to the final destination
        public int GetHeuristicValue(Cell node, Cell end)
        {
            return (Math.Abs(end.y - node.y) + Math.Abs(end.x - node.x)) * 10;//using Manhattan method
        }

        //given the locations of a node, this method return the horizontally and vertically adjacent nodes of the former node.
        public List<Cell> AdjacentElements(Grid grid, int row, int column)
        {
            Cell[,] cells = grid.GetGrid();
            int rows = cells.GetLength(0);
            int columns = cells.GetLength(1);            
            List<Cell> nodes = new List<Cell>();

            for (int j = row - 1; j <= row + 1; j++)
            {
                for (int i = column - 1; i <= column + 1; i++)
                {
                    if (i >= 0 && j >= 0 && i < columns && j < rows && !(j == row && i == column) && ((j == row) || (i == column)))
                    {                        
                        nodes.Add(cells[j, i]);
                    }
                }
            }
            return nodes;
        }

        //Find the optimum path with bricks or without bricks as specified by wantBrick parameter
        public List<Cell> FindAPath(Cell start, Cell end, bool wantBrick)
        {
            openList = new List<Cell>();
            closedList = new List<Cell>();
            openList.Add(start);
            start.G = 0; // Cost from start along best known path.
            start.F = start.G + GetHeuristicValue(start, end);// Estimated total cost from start to goal (using Manhattan method)
            Cell current; //keeps the current node
            
            List<Cell> totalPath = new List<Cell>();//optimum path from start to end
            Cell tempCurr = start;//keeps the previously visited Cell

            while (openList.Count != 0)
            {                
                current = openList.Last(node => node.F == Cell.Min(openList));   //select the last Cell added to the list with minimum F value as current node             
                if (!current.Equals(start))
                {
                    //check whether the selected current cell can be travelled horizontally or verticall. i.e. without moving diagonally
                    while (!(current.x == tempCurr.x || current.y == tempCurr.y) || !(Math.Abs(current.x - tempCurr.x) < 2 && Math.Abs(current.y - tempCurr.y) <2))
                    {
                        openList.Remove(current);
                        closedList.Add(current);
                        try//if there is no path with brick or no path without brick
                        {
                            current = openList.Last(node => node.F == Cell.Min(openList));
                        }
                        catch (InvalidOperationException e)
                        {
                            return null;
                        }
                    }
                    tempCurr = current;
                }

                totalPath.Add(current);//current node is in the path

                if (current.Equals(end))//if we reach the destination
                {
                    return totalPath;
                }

                openList.Remove(current);//no intention of traversing current node again
                closedList.Add(current);//no intention of traversing current node again

                List<Cell> adjNodes = AdjacentElements(grid, current.x, current.y);
                foreach (Cell n in adjNodes)
                {
                    //traverse either by ignoring bricks or with bricks.
                    bool condition = wantBrick ? (n.priority == 3 || n.priority == 2) : (n.priority == 3 || n.priority == 2 || n.priority == 1);
                    if (!condition)
                    {
                        if (closedList.Contains(n))//we do not traverse nodes in closedList again
                            continue;

                        int newG = current.G + 10;//calculate new G value

                        if (!openList.Contains(n) || newG < n.G)//n is either not traversed before or has a high G value than newG
                        {                            
                            n.G = newG;//change G to newG
                            n.H = GetHeuristicValue(n, end);//calculate new H
                            n.F = n.G + n.H;//calculate new F
                            if (!openList.Contains(n))//put n in openList
                            {                                
                                openList.Add(n);
                            }
                        }
                    }
                }
            }
            return null;
        }

        public Grid GetGrid()
        {
            return grid;
        }        
    }
}
