using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using PathFinder;
using GameEntity;

namespace PathFindTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFindAPath()
        {
            Program p = new Program();
            Cell s = new EmptyCell(7, 1);
            Cell e = new EmptyCell(7, 3);
            int node = p.FindAPath(s, e, true).Count;
            int result = 3;
            Assert.AreEqual(result, node);
        }

        [TestMethod]
        public void TestHeuristicValue()
        {
            Program p = new Program();
            Cell curr = new EmptyCell(3, 9);
            Cell end = new EmptyCell(15, 19);
            int expected = (19 - 9) * 10 + (15 - 3) * 10;
            int res = p.GetHeuristicValue(curr, end);
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void TestAdjacentElement()
        {
            Program p = new Program();
            var nodes = p.AdjacentElements(p.GetGrid(), 3, 10);
            Cell node = (Cell)nodes[0];
            int exp = 0;
            Assert.AreEqual(exp, node.priority);
        }

        [TestMethod]
        public void TestFindPath()
        {
            Program p = new Program();
            Cell curr = new EmptyCell(9, 2);
            Cell end = new EmptyCell(9, 2);
            int result = 1;
            int next = p.FindPath(curr, end).Count;
            Assert.AreEqual(result, next);
        }

        [TestMethod]
        public void TestFindCoinPile()
        {
            TreasureFinder cf = new TreasureFinder();
            Program p=new Program();
            Cell curr = new EmptyCell(10, 10);
            int[] exp = new int[] { 9, 5 };
            int[] res = cf.FindNextTreasure(curr, p.GetGrid().GetGrid(), 5);
            Assert.AreEqual(exp[1], res[1]);
        }
    }
}
