using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Level
{

    public class LevelBlockDecorPrefabComponent : ManagedControllerBase
    {
        [SerializeField,Tooltip("1 will spawn")] protected List<LevelBlockDecorPrefabComponent> ObligatoryDecors;
        [Space,SerializeField,Tooltip ("1 may spawn")] protected List<LevelBlockDecorPrefabComponent> PossibleDecors;
        [SerializeField, Range(0f, 1f), Tooltip("Chance of spawning a 'Possible' decor")] protected float _chance;
        [SerializeField,Tooltip("Decor Rotates on spawn")] protected bool _rotates;

        protected float[] _yRotations;

        private List<LevelBlockDecorPrefabComponent> _loadedDecors;

        public override void StartController()
        {
            _yRotations = new float[] { 0, 90, 180, 270 };

            var y = _yRotations[Random.Range(0, _yRotations.Length)];

            if (_rotates)
            {
                transform.localEulerAngles = new Vector3(0, y, 0);
            }

            _loadedDecors = new List<LevelBlockDecorPrefabComponent>();

            if (ObligatoryDecors != null && ObligatoryDecors.Count > 0)
            {

                var prefab = ObligatoryDecors[Random.Range(0, ObligatoryDecors.Count)];
                var s = Instantiate(prefab, transform);
                s.transform.SetPositionAndRotation(transform.position, transform.rotation);

                _loadedDecors.Add(s);

                Debug.Log($"Selected obligatory {s.name} for {name}, rotation {y}");

            }
            if (PossibleDecors != null && PossibleDecors.Count > 0)
            {
                bool place = (Random.Range(0, 1) < _chance);
                if (place)
                {
                    var prefab = PossibleDecors[Random.Range(0, PossibleDecors.Count)];
                    var s = Instantiate(prefab, transform);
                    s.transform.SetPositionAndRotation(transform.position, transform.rotation);

                    _loadedDecors.Add(s);

                    Debug.Log($"Selected optional {prefab.name} for {name}, rotation {y}");
                }
                else
                {
                    Debug.Log($"Skipping optional decor for {name}");
                }
            }
            foreach (var d in _loadedDecors)
            {
                d.StartController();
            }

        }

        public override void StopController()
        {
            foreach (var d in _loadedDecors)
            {
                d.StopController();
            }
        }

        public override void UpdateController(float delta)
        {
            if (_loadedDecors == null) return;
            foreach (var d in _loadedDecors)
            {
                d.UpdateController(delta);
            }
        }
    }
}