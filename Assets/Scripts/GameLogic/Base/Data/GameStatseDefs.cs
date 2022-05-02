using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

namespace Assets.Scripts
{
public class Game
    {
        public enum State
        {
            idle, 
            loading,
            loaded, 
            gameStarting,
            gameStarted,
            gamePlaying, 
            gameEnded,
            gamePausing, 
            gameUnPausing, 
            restarting,
        }
    }
}
