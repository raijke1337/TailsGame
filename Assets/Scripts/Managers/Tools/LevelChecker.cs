using Arcatech.Triggers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelChecker : MonoBehaviour
{
    private List<Transform> _floorFix;
    private List<Transform> _staticsFix;
    private List<Transform> _collidersFix;
    private List<Transform> _rigidsFix;
    private List<Transform> _textsFix;

    [ContextMenu("Run check")]
    private void Check()
    {
        _floorFix = new List<Transform>();
        _staticsFix = new List<Transform>();
        _collidersFix = new List<Transform>();
        _rigidsFix = new List<Transform>();
        _textsFix = new List<Transform>();

        List<Transform> list = new List<Transform>();

        var stuffs = GetComponentsInChildren<Transform>();
        foreach (var t in stuffs)
        {
            if (t != transform) { list.Add(t); }
        }


        foreach (var flooritem in list.Where(t => t.name.Contains("Floor")))
        {
            if (!flooritem.CompareTag("Ground"))
            {
                _floorFix.Add(flooritem);
            }
        }
        foreach (var item in list)
        {
            if (!item.gameObject.isStatic || item.gameObject.layer != 2)
            {
                _staticsFix.Add(item);
            }
        }
        foreach (var item in list.Where(t => t.GetComponent<Collider>() == null))
        {
            _collidersFix.Add(item);
        }
        foreach (var item in list.Where(t => t.GetComponent<Rigidbody>() == null))
        {
            _rigidsFix.Add(item);
        }
        foreach (var item in list.Where(t => t.GetComponent<BaseLevelEventTrigger>() != null))
        {
        }

        if (_floorFix.Count == 0 && _staticsFix.Count == 0 && _collidersFix.Count == 0 && _rigidsFix.Count == 0)
            return;
        Debug.Log($"{_floorFix.Count} level floor items missing the FLOOR tag");
        Debug.Log($"{_staticsFix.Count} level items missing the STATICITEM");
        Debug.Log($"{_collidersFix.Count} level items missing the COLLIDER");
        Debug.Log($"{_rigidsFix.Count} level items missing the RigidBody");
        Debug.Log($"{_textsFix.Count} text trigger items missing the TextTrigger tag");
    }

    private void OnValidate()
    {
        Check();
    }
    [ContextMenu("Fix")]
    private void Fix()
    {
        int flr = 0;
        int sta = 0;
        int col = 0;
        int rig = 0;
        int txt = 0;

        Check();

        foreach (var flooritem in _floorFix)
        {
            flooritem.tag = "Ground";
            flooritem.gameObject.isStatic = true;
            flr++;
        }
        foreach (var item in _staticsFix)
        {
            item.gameObject.isStatic = true;
            item.gameObject.layer = 2; //ignoreraycast
            sta++;
        }
        foreach (var item in _collidersFix)
        {
            item.gameObject.AddComponent<MeshCollider>();
            col++;
        }
        foreach (var item in _rigidsFix)
        {
            item.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            rig++;
        }
        foreach (var item in _textsFix)
        {
            item.tag = "TextTrigger";
            txt++;
        }
        Debug.Log($"Fixed: {flr} FLOOR tags, {sta} STATIC, {col} colliders, {rig} rigidbodies, {txt} texts");
    }
}

