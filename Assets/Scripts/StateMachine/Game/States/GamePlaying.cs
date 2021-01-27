namespace NextOne
{
    class GamePlaying : State<GameContext>
    {
        // Constructor taking the state machine + the state id
        public GamePlaying(StateMachine<GameContext> stateMachine) : base(stateMachine, (int)GameStates.GamePlaying) { }

    }
}