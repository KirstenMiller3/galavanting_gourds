using Milo.Tools;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
            float xOffset = Random.Range(-0.4f, .4f);
            float zOffset = Random.Range(-0.4f, .4f);
            var x = GridManager.GridOrigin.Position.x + xOffset;
            var z = GridManager.GridOrigin.Position.z + zOffset;
            var position = new Vector3(x, GridManager.GridOrigin.Position.y, z) ;

            var o = GameObject.Instantiate(Resources.Load("Oogroot"), position, Quaternion.identity) as GameObject;

            if(o == null)
            {
                Debug.Log("o returned null");
            }
            oogroots.Add(o);
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


public class WalkingState : IOogrootState
{
    public OogrootController.OogrootState OogrootState => OogrootController.OogrootState.Walking;

    RootController _rootController;

    List<Vector3> _route;
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

        }

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (OogrootController.Instance.oogroots.Select(o => o.GetComponent<Oogroot>().Arrived).All(x => x))
        {
            Debug.Log("planting");
            OogrootController.Instance.SetState(OogrootController.OogrootState.Planting);

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

    float _timer;

    float _timeout = 3;
    public OogrootController.OogrootState OogrootState => OogrootController.OogrootState.Ready;

    private List<Vector3> StartPos = new List<Vector3>();
    public void OnEnter()
    {
        foreach (var oogroot in OogrootController.Instance.oogroots)
        {
            StartPos.Add(oogroot.transform.position);

        }

    }

    public void OnExit()
    {
        //OogrootController.Instance.oogroots[i].GetComponent<OogrootAnimator>().StartJump();
    }

    public void OnUpdate()
    {
        var origin = GameController.Instance.gridManager.GridOrigin.Position;

        for (int i = 0; i < StartPos.Count; i++)
        {
            var offset = Random.Range(-0.3f, 0.3f);
            var dest = origin + new Vector3(offset, 0, offset) + (new Vector3(0, .3f,0));
            OogrootController.Instance.oogroots[i].transform.position = Vector3.Lerp(OogrootController.Instance.oogroots[i].transform.position, dest, Time.deltaTime * 2f);


        }

        _timer += Time.deltaTime;

        if (_timer >= _timeout)
        {
            OogrootController.Instance.SetState(OogrootController.OogrootState.Walking);

        }
    }

}