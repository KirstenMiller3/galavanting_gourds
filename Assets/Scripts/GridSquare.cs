using UnityEngine;
using DG.Tweening;

public enum GridType
{
    None,
    Gap,
    Button,
    Hazard,
    End
}

public class GridSquare : MonoBehaviour
{
    public GridType GridType;
    public bool IsOccupied;


    public string ButtonId;


    public void Start()
    {
        transform.DOPunchScale(Vector3.up, 1f);
    }


    private void OnDrawGizmosSelected()
    {
        if (GridType == GridType.Hazard)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + (Vector3.up * 1f), Vector3.one * 0.2f);
        }

        if(GridType == GridType.Button)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position + (Vector3.up * 1f), Vector3.one * 0.2f);
        }
    }
}