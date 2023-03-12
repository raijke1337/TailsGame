using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SelectableItemPanel : MonoBehaviour
{

    [SerializeField] protected TextMeshProUGUI _description;
    [SerializeField] protected RawImage _image;
    protected SelectableItem _item;

    public virtual void AssignItem(SelectableItem item)
    {
        _description.text = item.GetText;
        item.ToggleCam(true);
    }


    protected bool _act;
    public bool IsNeeded
    {
        get => _act;
        set
        {
            if (!value)
            {
                if (_item!= null)
                {
                    _item.ToggleCam(false);
                }                
                _item = null;
            }
            _act = value;
            gameObject.SetActive(value);
        }
    }

}

