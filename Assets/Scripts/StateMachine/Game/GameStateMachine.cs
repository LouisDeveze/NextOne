using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    enum GameStates
    {
        GameLoadingScreen = 0,
        GameLevelGeneration = 1,
        GameFadeIn = 2,
        GamePlaying = 3,
        GameDeath = 4,
        GamePause = 5,
    }

    class GameStateMachine : StateMachine<GameContext>
    {

        public GameStateMachine() : base()
        {
            // Add the states tot the state machine
            this.states.Add((int)GameStates.GameLoadingScreen, new GameLoadingScreen(this));
            this.states.Add((int)GameStates.GameLevelGeneration, new GameLevelGeneration(this));
            this.states.Add((int)GameStates.GameFadeIn, new GameFadeIn(this));
            this.states.Add((int)GameStates.GamePlaying, new GamePlaying(this));
            this.states.Add((int)GameStates.GameDeath, new GameDeath(this));
            this.states.Add((int)GameStates.GamePause, new GamePause(this));

        }

        public void Start()
        {
            this.SwitchState((int)GameStates.GameLoadingScreen);
        }
    }

}
