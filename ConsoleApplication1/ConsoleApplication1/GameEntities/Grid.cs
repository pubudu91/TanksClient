using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace GameEntity
{
    enum GameObjects
    {
        Blank, Brick, Stone, Water
    }
    public class Grid
    {
        public const int SIZE = 20;
        private static Grid obj;

        private Cell[,] grid;

        private Grid()
        {
            grid = new Cell[SIZE, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    grid[i, j] = new EmptyCell();
                }
            }
        }

        public static Grid getInstance()
        {
            if (obj == null)
                obj = new Grid();
            
            return obj;
        }

        public void initializeGrid(ArrayList bricks, ArrayList stone, ArrayList water)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                int[] temp = (int[])bricks[i];
                grid[temp[0], temp[1]] = new BrickCell();
            }

            for (int i = 0; i < stone.Count; i++)
            {
                int[] temp = (int[])stone[i];
                grid[temp[0], temp[1]] = new StoneCell();
            }

            for (int i = 0; i < water.Count; i++)
            {
                int[] temp = (int[])water[i];
                grid[temp[0], temp[1]] = new WaterCell();
            }
        }
    }
}
