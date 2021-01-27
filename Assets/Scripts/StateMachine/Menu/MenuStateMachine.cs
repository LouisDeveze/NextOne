using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    enum MenuStates{
        FadeInTitle = 0,
        MainTitle = 1,
        OptionState = 2,
        GameSelection = 3,
        CharacterSelection = 4,
        SaveSelection = 5
    }

    class MenuStateMachine : StateMachine<MenuContext>
    {

        public MenuStateMachine() : base()
        {
            // Add the states tot the state machine
            this.states.Add((int)MenuStates.FadeInTitle, new FadeInTitle(this));
            this.states.Add((int)MenuStates.MainTitle, new MainTitle(this));
            this.states.Add((int)MenuStates.OptionState, new OptionState(this));
            this.states.Add((int)MenuStates.GameSelection, new GameSelection(this));

        }

        public void Start()
        {

            this.SwitchState((int)MenuStates.FadeInTitle);
        }
    }

}
