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



public class OogrootController : Singleton<OogrootController>
{
    private const int MaxOogroots = 10;
    public enum OogrootState
    {
        Idle, Ready, Walking, Planting, Dying
    }

    [SerializeField] private GridManager _gridManager;

    public OogrootState State => _state != null ? _state.OogrootState : OogrootState.Idle;
    public GridManager GridManager => _gridManager;

    public bool PauseControls => State != OogrootState.Idle;

    private IOogrootState _state;
    public UnityEvent<OogrootState> OnStateChanged;

    private Dictionary<OogrootState, IOogrootState> _states = new Dictionary<OogrootState, IOogrootState>();


    protected override void Awake()
    {
        base.Awake();
        _states.Add(OogrootState.Idle, new IdleState());
    }

    private void Start() { 
    
        for(int i = 0; i < MaxOogroots; i++) { 
            float xOffset = Random.Range(-0.4f, .4f);
            float zOffset = Random.Range(-0.4f, .4f);
            var x = GridManager.GridOrigin.Position.x + xOffset;
            var z = GridManager.GridOrigin.Position.z + zOffset;
            var position = new Vector3(x, GridManager.GridOrigin.Position.y, z);

            Instantiate(Resources.Load("oogroot"), position, Quaternion.identity);
        }
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