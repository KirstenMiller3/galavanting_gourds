using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IOogrootState
{
    void OnUpdate();
    void OnEnter();
    void OnExit();
    OogrootController.OogrootState OogrootState { get; }
}


// empty gameobject with cubes inside
// spawn lots at 0,0 in random pos
//move them up above root
// follow route

public class OogrootController : Singleton<OogrootController>
{
    public enum OogrootState
    {
       Idle, Ready, Walking, Planting, Dying
    }

    [SerializeField] private RootController _rootController;

    public OogrootState State => _state != null ? _state.OogrootState : OogrootState.Idle;
    public RootController RootController => _rootController;

    public bool PauseControls => State != OogrootState.Idle;

    private IOogrootState _state;
    public UnityEvent<OogrootState> OnStateChanged;

    private Dictionary<OogrootState, IOogrootState> _states = new Dictionary<OogrootState, IOogrootState>();


    protected override void Awake()
    {
        base.Awake();
        _states.Add(OogrootState.Idle, new IdleState());
    }

    private void Start()
    {
        SetState(OogrootState.Idle);
    }

    private void Update()
    {
        if (_state != null)
        {
            _state.OnUpdate();
        }
    }

    public void SetState(OogrootState state)
    {
        if (_state != null && _state.OogrootState == state)
        {
            return;
        }

        if (_state != null)
        {
            _state.OnExit();
        }

        _state = _states[state];
        Debug.Log($"<color=yellow>Entering state: {_state.OogrootState}</color>");
        _state.OnEnter();

        OnStateChanged?.Invoke(state);
    }
}

public class IdleState : IOogrootState
{
    public OogrootController.OogrootState OogrootState => OogrootController.OogrootState.Idle;

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}


public class ReadyState : IOogrootState
{
    public OogrootController.OogrootState OogrootState => OogrootController.OogrootState.Ready;

   
    public void OnEnter()
    {
       
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
      
    }
}