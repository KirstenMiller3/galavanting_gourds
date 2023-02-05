using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IGameState
{
    void OnUpdate();
    void OnEnter();
    void OnExit();
    GameController.GameState GameState { get; }
}

public class GameController : Singleton<GameController> 
{
    public enum GameState
    {
        None,
        Rooting,
        Hazard,
        Pikmining,
        Death,
        Success,
        End,
        Seeding
    }

    [SerializeField] private RootController _rootController;
    [SerializeField] private float _poisonDamageInterval = 1f;

    public GridManager gridManager;


    public GameState State => _state != null ? _state.GameState : GameState.None;
    public RootController RootController => _rootController;

    public bool PauseControls => State != GameState.Rooting;

    private IGameState _state;
    public UnityEvent<GameState> OnStateChanged;

    private Dictionary<GameState, IGameState> _states = new Dictionary<GameState, IGameState>();

    public System.Action<int> OnTakePoisonDamage;

    private int _poisonDamageCounter = 0;

    private float _poisonDamageTimer = 0f;


    protected override void Awake()
    {
        base.Awake();
        _states.Add(GameState.Rooting, new RootingState());
        _states.Add(GameState.Hazard, new HazardState());
        _states.Add(GameState.Pikmining, new PikminingState());
        _states.Add(GameState.Success, new SuccessState());
        _states.Add(GameState.End, new EndState());
        _states.Add(GameState.Seeding, new SeedingState());
    }

    private void Start()
    {
        SetState(GameState.Rooting);
    }

    private void Update()
    {
        if(_state != null)
        {
            _state.OnUpdate();
        }

        if(_poisonDamageCounter == 0)
        {
            _poisonDamageTimer = 0f;
            return;
        }

        _poisonDamageTimer += Time.deltaTime;

        if (_poisonDamageTimer >= _poisonDamageInterval)
        {
            _poisonDamageTimer = 0f;
            OnTakePoisonDamage?.Invoke(_poisonDamageCounter);
        }
    }

    public void AddPoisonDamage()
    {
        _poisonDamageCounter++;
        UIController.Instance.UpdatePoisoned(_poisonDamageCounter);
    }

    public void RemovePoisonDamage()
    {
        _poisonDamageCounter--;
        UIController.Instance.UpdatePoisoned(_poisonDamageCounter);
    }

    public void SetState(GameState state)
    {
        if(_state != null && _state.GameState == state)
        {
            return;
        }

        if(_state != null)
        {
            _state.OnExit();
        }

        _state = _states[state];
        Debug.Log($"<color=yellow>Entering state: {_state.GameState}</color>");
        _state.OnEnter();

        OnStateChanged?.Invoke(state);
    }

    public void OnClickStart()
    {
        SetState(GameState.Pikmining);
    }
}

public class RootingState : IGameState
{
    public GameController.GameState GameState => GameController.GameState.Rooting;

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


public class HazardState : IGameState
{
    public GameController.GameState GameState => GameController.GameState.Hazard;

    private const int _undoAmmount = 3;
    public const float _undoRootTime = 1f;

    private float _timer = 0f;
    private int _undoCount = 0;

    public void OnEnter()
    {
        _undoCount = 0;
        _timer = 0f;
        UIController.Instance.ShowHazardWarning(true);
    }

    public void OnExit()
    {
        UIController.Instance.ShowHazardWarning(false);
    }

    public void OnUpdate()
    {
        if(_undoCount >= _undoAmmount) {
            GameController.Instance.SetState(GameController.GameState.Rooting);
            return;
        }

        _timer += Time.deltaTime;

        if(_timer >= _undoRootTime ) {
            _timer = 0f;
            _undoCount++;

            if (!GameController.Instance.RootController.Undo())
            {
                GameController.Instance.SetState(GameController.GameState.Rooting);
            }
        }
    }
}


public class PikminingState : IGameState
{
    private const float _testTime = 3f;

    private float _tempTimer = 0f;
    public GameController.GameState GameState => GameController.GameState.Pikmining;

    public void OnEnter()
    {
        //To do: Make pikmin hop onto root
        OogrootController.Instance.SetState(OogrootController.OogrootState.Ready);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
    }
}


public class SuccessState : IGameState
{
    public GameController.GameState GameState => GameController.GameState.Success;

    public void OnEnter()
    {
        UIController.Instance.SetPlayButton(true);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}

public class EndState : IGameState
{
    public GameController.GameState GameState => GameController.GameState.End;
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

public class SeedingState : IGameState
{
    public GameController.GameState GameState => GameController.GameState.Seeding;
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