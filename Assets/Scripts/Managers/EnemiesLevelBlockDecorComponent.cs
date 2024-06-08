using Arcatech.AI;
using Arcatech.Level;
using Arcatech.Units;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Arcatech.Managers
{
    [RequireComponent(typeof(BoxCollider))]
    public class EnemiesLevelBlockDecorComponent : LevelBlockDecorPrefabComponent
    {      
       // private PlayerUnit _player;
        private List<NPCUnit> _unitsInRoom = new List<NPCUnit>();
        public List<NPCUnit> GetAllUnitsInRoom
        {
            get { return _unitsInRoom; }
        }

        #region decor logic
        [SerializeField] protected SerializedDictionary<BaseUnit, int> _unitsToSpawn;
        protected BoxCollider _spawnArea;


        #endregion

        #region managed
        public override void StartController()
        {
            
            base.StartController(); // place decors etc here
            _spawnArea = GetComponent<BoxCollider>();

            _unitsInRoom.AddRange(GetComponentsInChildren<NPCUnit>());
            _unitsInRoom.AddRange(PlaceUnits());
            

        }

        #endregion


        #region units

        protected List<NPCUnit> PlaceUnits()
        {
            List<NPCUnit> units = new List<NPCUnit>();

            foreach (NPCUnit prefab in _unitsToSpawn.Keys)
            {
                for (int i = 1; i <= _unitsToSpawn[prefab]; i++)
                {
                    float x = UnityEngine.Random.Range(_spawnArea.bounds.min.x, _spawnArea.bounds.max.x);
                    float y = _spawnArea.bounds.max.y;
                    float z = UnityEngine.Random.Range(_spawnArea.bounds.min.z, _spawnArea.bounds.max.z);

                    Vector3 place = new Vector3(x, y, z);
                    var unit = Instantiate(prefab, place, Quaternion.identity, this.transform);
                    units.Add(unit);
                    if (DebugMessage) Debug.Log($"placed {unit.name} at {place}");
                }
            }

            return units;
        }

        #endregion
    }
}