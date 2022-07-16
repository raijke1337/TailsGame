using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class VisualsController : MonoBehaviour
{
    [SerializeField] private List<Material> _materials;
    public int StagesTotal => _materials.Count;
    private SkinnedMeshRenderer _mesh;
    private int _matIndex = 0;

    public bool SetMaterialStage(int ind)
    {
        if (ind >= _materials.Count) return false;
        _matIndex = ind;
        UpdateMaterial();
        return true;
    }
    public bool AdvanceMaterialStage()
    {
        if (_matIndex == _materials.Count - 1) return false;
        _matIndex++;
        UpdateMaterial();
        return true;
    }
    

    private void Start()
    {
        _mesh = GetComponentsInChildren<SkinnedMeshRenderer>().First(t => t.name == "Model");
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        _mesh.material = _materials[_matIndex];
    }


}

