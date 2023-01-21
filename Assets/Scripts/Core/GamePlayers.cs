using System.Collections;
using System.Collections.Generic;
using MathFighter.GamePlay;
using UnityEngine;
namespace MathFighter.Core
{
    public class GamePlayers : MonoBehaviour
    {
        private Player player1;
        private Player player2;

        private static GamePlayers gamePlayers;

        public static Player Player1
        {
            get { return gamePlayers.player1; }
            set { gamePlayers.player1 = value; }
        }


        public static Player Player2
        {
            get { return gamePlayers.player2; }
            set { gamePlayers.player2 = value; }
        }

        public static GamePlayers instance
        {
            get
            {
                if (!gamePlayers)
                {
                    gamePlayers = FindObjectOfType(typeof(GamePlayers)) as GamePlayers;

                    if (!gamePlayers)
                        Debug.LogError("There needs to be one active GamePlayer script on a GameObject in your scene.");
                    else
                    {
                        gamePlayers.player1 = new Player();
                        gamePlayers.player2 = new Player();
                        //  Sets this to not be destroyed when reloading scene
                        DontDestroyOnLoad(gamePlayers);
                    }
                }
                return gamePlayers;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
