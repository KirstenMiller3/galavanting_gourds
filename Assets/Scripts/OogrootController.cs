using DG.Tweening;
using Milo.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static UnityEngine.UI.Image;

public interface IOogrootState
{
    void OnUpdate();
    void OnEnter();
    void OnExit();
    OogrootController.OogrootState OogrootState { get; }
}



public class OogrootController : Milo.Tools.Singleton<OogrootController>
{
    private const int MaxOogroots = 10;

    public List<GameObject> oogroots;
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
        _states.Add(OogrootState.Ready, new ReadyState());
        _states.Add(OogrootState.Walking, new WalkingState());
        _states.Add(OogrootState.Planting, new PlantingState());


    }

    private void Start() { 
    
        for(int i = 0; i < MaxOogroots; i++) { 
            float xOffset = UnityEngine.Random.Range(-0.4f, .4f);
            float zOffset = UnityEngine.Random.Range(-0.4f, .4f);
            var x = GridManager.GridOrigin.Position.x + xOffset;
            var z = GridManager.GridOrigin.Position.z + zOffset;
            var position = new Vector3(x, 0f, z); 

            var o = GameObject.Instantiate(Resources.Load("Oogroot"), position, Quaternion.identity) as GameObject;
            oogroots.Add(o);
        }
        SetState(OogrootState.Idle);

        GameController.Instance.OnTakePoisonDamage += KillOogroot;


    }

    public void KillOogroot(int numToDie)
    {
        if (numToDie <= oogroots.Count)
        {
            for (int i = 0; i < numToDie; i++)
            {
                oogroots[i].GetComponent<OogrootAnimator>().StartDeath();

            }

            oogroots.RemoveRange(0, numToDie);
        }
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


public class WalkingState : IOogrootState
{
    public OogrootController.OogrootState OogrootState => OogrootController.OogrootState.Walking;

    RootController _rootController;

    List<Vector3> _route;

    float _chirpTimer; 

    public void OnEnter()
    {
        _rootController = GameController.Instance.RootController;
        _route = _rootController.Route.Select(r => r.position).ToList();
        _route.Reverse();
        _route = _route.Select(s => s += new Vector3(0, 0.3f, 0)).ToList();
        _route.ForEach(p => Debug.Log("route " + p));

        var oogrootsArrived = 0;

        foreach (var oogroot in OogrootController.Instance.oogroots)
        {
            oogroot.GetComponent<Oogroot>().SetTargetPositions(_route.ToArray());
            oogroot.transform.GetComponent<OogrootAnimator>().StartRun();

        }

    }


    public void OnExit()
    {
        foreach(var oogroot in OogrootController.Instance.oogroots)
        {
            oogroot.transform.GetComponent<OogrootAnimator>().EndRun();
        }
    }

    public void OnUpdate()
    {
        if (OogrootController.Instance.oogroots.Select(o => o.GetComponent<Oogroot>().Arrived).All(x => x))
        {
            Debug.Log("planting");
            OogrootController.Instance.SetState(OogrootController.OogrootState.Planting);
            GameController.Instance.SetState(GameController.GameState.End);
        }
        else
        {
            _chirpTimer += Time.deltaTime;
            if (_chirpTimer >= 0.4f && AudioManager.instance.sounds.Any(s => s.sName.Contains("chirp") && !s.source.isPlaying)) {
                _chirpTimer = 0;
                int num = UnityEngine.Random.Range(1, 15);
                AudioManager.instance.Play($"chirp_{num}");
                    }
        }
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

public class PlantingState : IOogrootState
{
    public OogrootController.OogrootState OogrootState => OogrootController.OogrootState.Planting;

    private List<Vector3> positionsToPlant = new List<Vector3>();

    float _timer;

    float _timeout = 1.5f;

    public void OnEnter()
    {

        GameController.Instance.SetState(GameController.GameState.Seeding);

        foreach (var oogroot in OogrootController.Instance.oogroots)
        {
            var isOccupied = false;
            var dest = Vector3.zero;

            while(!isOccupied)
            {
                oogroot.transform.GetComponent<OogrootAnimator>().StartJump();
                var x = UnityEngine.Random.Range(0, GameController.Instance.gridManager._rowSize);
                var y = UnityEngine.Random.Range(0, GameController.Instance.gridManager._colSize);
                var tile = GameController.Instance.gridManager.grid[x, y];
                dest = tile.TilePosition;
                isOccupied = tile.GridType != GridType.Gap;
            }

            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(oogroot.transform.DOJump(dest, 0.2f, 1, 1f))
                .Insert(0f, oogroot.transform.DOPunchScale(Vector3.up * 0.005f, 1f, 2, 0.2f));


            positionsToPlant.Add(dest);
       

        }

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        _timer += Time.deltaTime;

        if (_timer >= _timeout)
        {
            foreach(var pos in positionsToPlant)
            {
                var index = UnityEngine.Random.Range(1, 3);
                GameObject.Instantiate(Resources.Load($"Flowers_0{index}"), pos, Quaternion.identity);
            }
   
        }
    }
}
public class ReadyState : IOogrootState
{

    float _timer;

    float _timeout = 3f;
    public OogrootController.OogrootState OogrootState => OogrootController.OogrootState.Ready;

    public void OnEnter()
    {
       

        var origin = new Vector3( GameController.Instance.gridManager.GridOrigin.Position.x, 0f, GameController.Instance.gridManager.GridOrigin.Position.z);

        for (int i = 0; i < OogrootController.Instance.oogroots.Count; i++)
        {
            var offset = UnityEngine.Random.Range(-0.3f, 0.3f);
            var dest = origin + new Vector3(offset, 0, offset) + (new Vector3(0, .1f, 0));
            OogrootController.Instance.oogroots[i].transform.GetComponent<OogrootAnimator>().StartJump();

            DG.Tweening.Sequence sequence = DOTween.Sequence();
            sequence.Append(OogrootController.Instance.oogroots[i].transform.DOJump(dest, 0.2f, 1, 1f))
                .Insert(0f, OogrootController.Instance.oogroots[i].transform.DOPunchScale(Vector3.up * 0.005f, 1f, 2, 0.2f));
        }
    }

    public void OnExit()
    {
        for (int i = 0; i < OogrootController.Instance.oogroots.Count; i++)
        {
            OogrootController.Instance.oogroots[i].transform.GetComponent<OogrootAnimator>().EndJump();
        }
    }

    public void OnUpdate()
    {


        _timer += Time.deltaTime;

        if (_timer >= _timeout)
        {
            OogrootController.Instance.SetState(OogrootController.OogrootState.Walking);

        }
    }

}