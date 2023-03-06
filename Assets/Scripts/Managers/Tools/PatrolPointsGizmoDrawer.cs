using System.Collections.Generic;
using UnityEngine;

public class PatrolPointsGizmoDrawer : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private float _size = 0.3f;

    private void OnValidate()
    {
        UpdateInfo();
    }

    private void OnDrawGizmos()
    {
        UpdateInfo();
        Gizmos.color = Color.cyan;

        foreach (var p in _points)
        {
            Gizmos.DrawWireSphere(p.position, _size);
        }
    }

    [ContextMenu("Refresh")]
    public void UpdateInfo()
    {
        _points = new List<Transform>();
        _points.AddRange(GetComponentsInChildren<Transform>());
    }

}

