using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine<TOwner>
{
    public StateMachine(TOwner owner)
    {
        Owner = owner;
    }

    private LinkedList<State> _states = new();

    // 理解メモ　間違っていた場合指摘してください。
    // Stateがステートマシーンを持つのは、StateでOwnerを扱いたいから。

    public T Add<T>() where T : State, new()
    {
        var state = new T();
        state._sm = this;
        _states.AddLast(state);
        return state;
    }

    public void AddOrTransition<TFrom, TTo>(int eventId)
    where TFrom : State, new()
    where TTo : State, new()
    {
        var from = GetOrAddState<TFrom>();
        var to = GetOrAddState<TTo>();
        if (from._transitionPair.TryGetValue(eventId, out var result))
        {
            from.Enter(result);
        }

        Debug.Log($"{to}は推移先として定義されていません。/n {to}を推移先として定義します");
        from._transitionPair.Add(eventId, to);
    }

    public void Dispatch(int eventId)
    {
        State to;
        if (!CurrentState._transitionPair.TryGetValue(eventId, out to))
        {
            if (!GetOrAddState<AnyState>()._transitionPair.TryGetValue(eventId, out to))
            {
                return;
            }
        }

        Change(to);
    }

    public void AnyTransition<TTo>(int eventId) where TTo : State, new()
    {
        AddOrTransition<AnyState, TTo>(eventId);
    }
    public void Start<TFirst>() where TFirst : State, new()
    {
        Start(GetOrAddState<TFirst>());
    }

    public void Start(State firstState)
    {
        CurrentState = firstState;
        CurrentState.Enter(null);
    }

    public void Update()
    {
        CurrentState.Update();
    }


    public void Change(State nextState)
    {
        // 推移前のステートを受け取り、推移後のアクションをステートに応じたものを行う。
        CurrentState.Exit(nextState);
        nextState.Enter(CurrentState);
        CurrentState = nextState;
    }

    public T GetOrAddState<T>()
    where T : State, new()
    {
        foreach (var state in _states)
        {
            if (state is T result)
            {
                return result;
            }
        }

        return Add<T>();
    }

    public class AnyState : State { }
    public State CurrentState { get; private set; }
    public TOwner Owner { get; private set; }
    public abstract class State
    {
        protected StateMachine<TOwner> Sm => _sm;
        internal StateMachine<TOwner> _sm;
        protected TOwner Owner => _sm.Owner;
        internal Dictionary<int, State> _transitionPair = new();

        // 理解メモ　間違っていた場合指摘してください。
        // Sm　はOwnerクラスの機能を使用するためのもの。
        // internalを使用するのは、同じアセンブリのステートマシーンから扱うため。
        // Enter,Exit,Update はStateの共通処理
        // OnEnter,OnExit,OnUpdate　は各ステートの独自処理
        public void Enter(State prevState)
        {
            OnEnter(prevState);
        }
        protected virtual void OnEnter(State prevState)
        {

        }

        public void Exit(State nextState)
        {
            OnExit(nextState);
        }

        protected virtual void OnExit(State nextState)
        {

        }

        public void Update()
        {
            OnUpdate();
        }
        protected virtual void OnUpdate()
        {

        }
    }
}
