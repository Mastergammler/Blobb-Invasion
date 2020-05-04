using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine
{
    //S: Manages states and their transitions
    //      Specifically the transitions from one state to another
    public class StateMachine
    {
        //###############
        //##  MEMBERS  ##
        //###############

        private IState mCurState;

        //fixme: why does it hold a list of transitions?
        private Dictionary<Type, List<Transition>> mTransitionsPerState = new Dictionary<Type,List<Transition>>();
        private List<Transition> mCurPossibleTransitions = new List<Transition>();
        private List<Transition> mOverrideTransitions = new List<Transition>();

        private static List<Transition> sEmptyTransitions = new List<Transition>(0);

        private bool mLogging = false;


        //#################
        //##  INTERFACE  ##
        //#################

        public void Tick()
        {
            //fixme it sounds pretty expensive to test every condition every single tick ...
            var transition = GetTransition();
            if(transition != null) SetState(transition.To);

            mCurState.Tick();
        }

        public void SetState(IState state)
        {
            if(state == mCurState) return;

            if(mLogging) Debug.Log($"Changing state to: {state.GetType().ToString()}");

            mCurState?.OnExit();
            mCurState = state;

            //fixme questonable 
            mTransitionsPerState.TryGetValue(mCurState.GetType(), out mCurPossibleTransitions);
            if(mCurPossibleTransitions == null) mCurPossibleTransitions = sEmptyTransitions;

            mCurState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if(mTransitionsPerState.TryGetValue(from.GetType(), out var transitions) == false)
            {   
                transitions = new List<Transition>();
                mTransitionsPerState[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to,predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            mOverrideTransitions.Add(new Transition(state,predicate));
        }

        public void EnableLogging(bool enable)
        {
            mLogging = enable;
        }

        //###############
        //##  METHODS  ##
        //###############

        // !!! the list of possible transitions are DEPENDANT on the state were currently in
        // That means that the current state will not stay and 'overrule' a second transition
        // Because the state itself is not in the transitions
        private Transition GetTransition()
        {
            foreach(var t in mOverrideTransitions)
            {
                if(t.Condition()) return t;
            }

            foreach(var t in mCurPossibleTransitions)
            {
                if(mLogging) Debug.Log($"Checking condition for state change to:{t.To.GetType().ToString()}");
                if(t.Condition()) 
                {
                    if(mLogging) Debug.Log($"Switching to state: {t.To.GetType().ToString()}");
                    return t;

                }
            }

            //fixme shouldn't it return the current transition then?
            return null;
            //throw new EntryPointNotFoundException("No condition for a state has been met!");
        }

        //#################
        //##  AUXILIARY  ##
        //#################

        //S: DataObject for a Transition
        private class Transition
        {
            public IState To { get; }
            public Func<bool> Condition { get; }

            public Transition(IState to, Func<bool> condition)
            {
                Condition = condition;
                To = to;
            }
        }
    }
}