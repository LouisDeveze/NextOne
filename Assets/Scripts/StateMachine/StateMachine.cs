
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NextOne
{
    /// <summary>
    /// This class describes a state doing absolutly nothing
    /// It has access to the state Machine
    /// </summary>
    abstract class State<Context> where Context : MonoBehaviour
    {
        protected StateMachine<Context> sm;
        protected int id;

        public State(StateMachine<Context> stateMachine, int id)
        {
            this.sm = stateMachine;
            this.id = id;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
        
    }

    /// <summary>
    /// This class is the state machine.
    /// It handles the different states of the game and control the switch between the states
    /// It holds reference to the Context data of the state machine with the reference to the 
    /// game objects of the scene used by the state 
    /// </summary>
    abstract class StateMachine<Context> : MonoBehaviour where Context : MonoBehaviour
    {
        /// <summary> Map of the States </summary>
        public Dictionary<int, State<Context>> states;

        /// <summary> this is the current state </summary>
        public State<Context> currentState;

        public Context ctx = null;
        
        
        /// <summary> Constructor for the State Machine </summary>
        public StateMachine()
        {
            this.states = new Dictionary<int, State<Context>>();
            this.currentState = null;
        }

        /// <summary>
        /// On Start the Machine Starts with the Initial State
        /// </summary>
        void Start()
        {

        }


        /// <summary>
        /// On Update callback updates the current state of the state machine
        /// </summary>
        void Update()
        {
            if (this.currentState != null)
                this.currentState.OnUpdate();
        }

        /// <summary>
        /// Exit the current state and enter the new one
        /// </summary>
        /// <param name="stateID"></param>
        public void SwitchState(int stateID)
        {
            if(this.currentState != null)
                this.currentState.OnExit();
            if (states.ContainsKey(stateID))
            {
                // Switch current state
                states.TryGetValue(stateID, out this.currentState);
                this.currentState.OnEnter();
            }
        }

        
    }
}
