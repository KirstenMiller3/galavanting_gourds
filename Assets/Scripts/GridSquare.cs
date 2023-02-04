using UnityEngine;

public class GridSquare : MonoBehaviour
{
    public bool IsHazard;

    private void OnDrawGizmosSelected()
    {
        if (IsHazard)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + (Vector3.up * 1f), Vector3.one * 0.2f);
        }
    }
}