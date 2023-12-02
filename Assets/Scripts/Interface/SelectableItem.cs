using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableItem : MonoBehaviour
{
    public SelectableItemType Type;

    [SerializeField,Tooltip("ID for text container")] private string _selectableID;
    public string GetTextID { get => _selectableID; }
    protected Camera _cam;
    public void ToggleCam(bool s)=> _cam.enabled = s;

    private void Awake()
    {
        var cam = new GameObject("ItemCamera", typeof(Camera));
        cam.tag = "Untagged";
        _cam = cam.GetComponent<Camera>();
        _cam.orthographic = true;
        _cam.orthographicSize = 0.65f;
        _cam.nearClipPlane = 0;
        _cam.targetTexture = AssetDatabase.LoadAssetAtPath<RenderTexture>("Assets/Scripts/Cameras/SelectedItemPicture.renderTexture");
        _cam.allowHDR = false;
        _cam.allowMSAA = false;
        _cam.transform.SetParent(transform, true);
        _cam.transform.position = transform.position + new Vector3(0.5f, 0.7f, 0.5f); // magic numbers woo
        _cam.transform.Rotate(new Vector3(30, 225, 0));
        ToggleCam(false);
    }

}
