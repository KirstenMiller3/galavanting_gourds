using UnityEngine;
using DG.Tweening;

public enum GridType
{
    None,
    Gap,
    Button,
    Hazard,
    End,
    Poison,
    Push
}

public class GridSquare : MonoBehaviour
{
    [SerializeField] private GameObject[] _tileTypes;
    [SerializeField] private GameObject _particles;

    public GridType GridType;
    public bool IsOccupied;
    public bool IsElectrified;


    public string InteractId;

    private bool _isElectrified;

    public void Start() {

        _isElectrified = IsElectrified;
        if (_tileTypes.Length > 0)
        {
            foreach (var tileType in _tileTypes)
            {
                tileType.SetActive(false);
            }

            _tileTypes[Random.Range(0, _tileTypes.Length)].SetActive(true);
        }


        transform.DOPunchScale(Vector3.up, 1f);
    }

    public bool DeElectrify()
    {
        _isElectrified = false;
        _particles.SetActive(_isElectrified);
        return _isElectrified;
    }


    private void OnDrawGizmos()
    {
        switch (GridType)
        {
            case GridType.None:
                break;
            case GridType.Gap:
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(transform.position, Vector3.one * 0.2f);
                break;
            case GridType.Button:
                Gizmos.color = Color.green;
                Gizmos.DrawCube(transform.position + (Vector3.up * 1f), Vector3.one * 0.2f);
                break;
            case GridType.Hazard:
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.position + (Vector3.up * 1f), Vector3.one * 0.2f);
                break;
            case GridType.End:
                break;
            case GridType.Poison:
                break;
            case GridType.Push:
                break;
            default:
                break;
        }
    }
}