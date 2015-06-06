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
                    grid[i, j].x = i;
                    grid[i, j].y = j;
                }
            }
        }

        public static Grid getInstance()
        {
            if (obj == null)
                obj = new Grid();
            
            return obj;
        }

        public Cell[,] GetGrid()
        {
            return grid;
        }

        public void initializeGrid(List<Position> bricks, List<Position> stone, List<Position> water)
        {
            for (int i = 0; i < bricks.Count; i++)
            {
                grid[bricks[i].x, bricks[i].y] = new BrickCell();
                grid[bricks[i].x, bricks[i].y].x = bricks[i].x;
                grid[bricks[i].x, bricks[i].y].y = bricks[i].y;
            }

            for (int i = 0; i < stone.Count; i++)
            {
                grid[stone[i].x, stone[i].y] = new StoneCell();
                grid[stone[i].x, stone[i].y].x = stone[i].x;
                grid[stone[i].x, stone[i].y].y = stone[i].y;
            }

            for (int i = 0; i < water.Count; i++)
            {
                grid[water[i].x, water[i].y] = new WaterCell();
                grid[water[i].x, water[i].y].x = water[i].x;
                grid[water[i].x, water[i].y].y = water[i].y;
            }
        }

        public void updateBrickDamages(int[,] damages)
        {
            for (int i = 0; i < damages.GetLength(0); i++)
            {
                if (grid[damages[i, 0], damages[i, 1]] is BrickCell)
                    ((BrickCell)grid[damages[i, 0], damages[i, 1]]).damageLevel = (DamageLevel)damages[i, 2];
                else
                    Console.WriteLine("ERROR: Invalid Brick coordinate when trying to update damage levels.");

                printBrickDamages(damages[i, 0], damages[i, 1], (DamageLevel)damages[i, 2]);
            }
        }

        public void draw()
        {
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    grid[i, j].Draw();
                }
            }
        }

        public void printBrickDamages(int x, int y, DamageLevel damage)
        {
            Console.Write("{ " + x + ", " + y + " : " + damage + " }, ");
        }

        public void printGrid()
        {
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (grid[i, j] is EmptyCell)
                        Console.Write(" - ");
                    else if (grid[i, j] is BrickCell)
                        Console.Write(" B ");
                    else if (grid[i, j] is StoneCell)
                        Console.Write(" S ");
                    else if (grid[i, j] is WaterCell)
                        Console.Write(" W ");
                }
                Console.WriteLine();
            }
        }
    }
}
