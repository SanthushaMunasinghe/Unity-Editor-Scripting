using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    public Vector3[] waypoints;
    [SerializeField] private bool looped;

    [SerializeField] private Vector3 GetPositionOnRoute(float t)
    {
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null)
        {
            return;
        }

        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i], 0.1f);

            if (i < waypoints.Length - 1 || looped)
            {
                Gizmos.DrawLine(waypoints[i], waypoints[(i + 1) % waypoints.Length]);
            }
        }
    }
}
