using com.cyborgAssets.inspectorButtonPro;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
namespace Arcatech.Level
{

    public class LevelChecker : MonoBehaviour
    {
        [ProButton]
        public void RemoveStrayNavmeshes()
        {
            var list = GetComponentsInChildren<NavMeshSurface>();
            Debug.Log($"Found {list.Count()} surfaces");
            foreach (var n in list)
            {
                DestroyImmediate(n);
            }
            if (!GetComponent<NavMeshSurface>())
            {
                gameObject.AddComponent<NavMeshSurface>();
                Debug.Log("Added navmesh surface to level object");
            }
        }
        [ProButton]
        public void CheckTags()
        {
            var list = GetComponentsInChildren<Transform>();
            foreach (var n in list)
            {
                if (n.CompareTag("Untagged"))
                {
                    Debug.Log(n.name + " has is not tagged!");
                }
            }

        }

    }

}