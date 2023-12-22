using UnityEditor;
using UnityEngine;

namespace Arcatech.UI
{
    public class BaseTargetableItem : MonoBehaviour
    {
        public string GetTitle { get => _title; }
        protected string _title;

        protected Camera _cam;
        protected virtual void LookUpValuesOnActivation()
        {

        }
        public RenderTexture ToggleCam(bool s)
        {
            LookUpValuesOnActivation();
            if (_cam == null)
            {
                var c = new GameObject("ItemCamera", typeof(Camera));
                c.tag = "Untagged";
                _cam = c.GetComponent<Camera>();
                _cam.orthographic = true;
                _cam.orthographicSize = 0.65f;
                _cam.nearClipPlane = 0;
                _cam.targetTexture = AssetDatabase.LoadAssetAtPath<RenderTexture>("Assets/Scripts/Cameras/SelectedItemPicture.renderTexture");
                _cam.allowHDR = false;
                _cam.allowMSAA = false;
                _cam.transform.SetParent(transform, true);
                _cam.transform.position = transform.position + new Vector3(0.5f, 0.7f, 0.5f); // magic numbers woo
                _cam.transform.Rotate(new Vector3(30, 225, 0));
            }
            _cam.enabled = s;
            return _cam.targetTexture;
        }
    }



}