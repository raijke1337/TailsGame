using Arcatech.AI;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcatech.Level
{
    public class PatrolPointsHandler : MonoBehaviour
    {
        private List<PatrolPoint> _points;
        [SerializeField] private float _size = 0.3f;
        [SerializeField]
        SerializedDictionary<Color, List<PatrolPoint>> _dict;

        private void OnValidate()
        {
            _points = new List<PatrolPoint>();
            _points.AddRange(GetComponentsInChildren<PatrolPoint>());
            _dict = new SerializedDictionary<Color, List<PatrolPoint>>();

            foreach (PatrolPoint point in _points)
            {
                if (_dict.ContainsKey(point.GizmoColor))
                {
                    _dict[point.GizmoColor].Add(point);
                }
                else
                {
                    _dict[point.GizmoColor] = new List<PatrolPoint>();
                    _dict[point.GizmoColor].Add(point);
                }
            }
        }


    }

}