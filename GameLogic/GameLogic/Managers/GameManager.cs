using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEntity;
using Core;
using System.Threading;

namespace GameLogic.Managers
{
    public class GameManager
    {
        private static GameManager gamemanager;
        private MessageParser parser;
        private NetworkListener listener;
        private Grid grid;
        private PlayerManager playermanager;

        private GameManager()
        {
            parser = MessageParser.getInstance();
            listener = NetworkListener.GetInstance();
            grid = Grid.getInstance();
            playermanager = PlayerManager.getInstance();

            //Thread t = new Thread(new ThreadStart(parser.listenToKeyboard));
            //t.Start();

            parser.GameInitiation += mp_initializeMap;
            parser.AcceptMessage += mp_initializePlayers;
            parser.GlobalBroadcast += mp_globalBroadcast;
        }

        public static GameManager getInstance()
        {
            if (gamemanager == null)
                gamemanager = new GameManager();

            return gamemanager;
        }

        public void start()
        {
            parser.sendMessage("JOIN#");
            parser.getMessage(); // start to listen to the game server
        }

        public void draw()
        {
            grid.draw();
        }

        private void mp_initializeMap(List<Position> bricks, List<Position> stone, List<Position> water, Player me)
        {
            playermanager.init(me); // Setup the own-player
            grid.initializeGrid(bricks, stone, water);
            grid.printGrid();
        }

        private void mp_initializePlayers(List<string[]> players)
        {
            playermanager.setupPlayers(players);
            playermanager.printPlayers();
        }

        private void mp_globalBroadcast(List<string[]> players, int[,] brickdamage)
        {
            playermanager.updatePlayers(players);
            grid.updateBrickDamages(brickdamage);
            playermanager.printPlayers();
        }
    }
}
