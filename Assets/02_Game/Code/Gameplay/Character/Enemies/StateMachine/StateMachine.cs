using System;
using System.Collections.Generic;
namespace BlobbInvasion.Gameplay.Character.Enemies.StateMachine
{
    //S: Manages states and their transitions
    //      Specifically the transitions from one state to another
    public class StateMachine
    {
        //###############
        //##  MEMBERS  ##
        //###############

        private IState mCurrentState;

        //fixme: why does it hold a list of transitions?
        private Dictionary<Type, List<Transition>> mTransitions = new Dictionary<Type,List<Transition>>();
        private List<Transition> mCurrentTransitions = new List<Transition>();
        private List<Transition> mPriorityTransitions = new List<Transition>();

        private static List<Transition> sEmptyTransitions = new List<Transition>(0);

        //#################
        //##  INTERFACE  ##
        //#################

        public void Tick()
        {
            //fixme it sounds pretty expensive to test every condition every single tick ...
            var transition = GetTransition();
            if(transition != null) SetState(transition.To);

            mCurrentState.Tick();
        }

        public void SetState(IState state)
        {
            if(state == mCurrentState) return;

            mCurrentState?.OnExit();
            mCurrentState = state;

            //fixme questonable 
            mTransitions.TryGetValue(mCurrentState.GetType(), out mCurrentTransitions);
            if(mCurrentTransitions == null) mCurrentTransitions = sEmptyTransitions;

            mCurrentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if(mTransitions.TryGetValue(from.GetType(), out var transitions) == false)
            {   
                transitions = new List<Transition>();
                mTransitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to,predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            mPriorityTransitions.Add(new Transition(state,predicate));
        }

        //###############
        //##  METHODS  ##
        //###############

        private Transition GetTransition()
        {
            foreach(var t in mPriorityTransitions)
            {
                if(t.Condition()) return t;
            }

            foreach(var t in mCurrentTransitions)
            {
                if(t.Condition()) return t;
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