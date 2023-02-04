using UnityEngine;

public class GridSquare : MonoBehaviour
{
    public bool IsHazard;
    public bool IsButton;
    public bool IsGap;

    private void OnDrawGizmosSelected()
    {
        if (IsHazard)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + (Vector3.up * 1f), Vector3.one * 0.2f);
        }

        if(IsButton)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position + (Vector3.up * 1f), Vector3.one * 0.2f);
        }
    }
}