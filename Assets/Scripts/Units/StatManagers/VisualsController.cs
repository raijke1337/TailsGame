using Arcatech.Items;
using Arcatech.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcatech.Units
{
    public class VisualsController : MonoBehaviour
    {
        #region materials change

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

        private void UpdateMaterial()
        {
            //_mesh.material = _materials[_matIndex];
        }

        #endregion
    }

}