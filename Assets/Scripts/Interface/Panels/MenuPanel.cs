using UnityEngine;

public class MenuPanel : MonoBehaviour
{

    public bool IsOpen { get; protected set; } = false;


    public virtual void OnToggle(bool isShow)
    {
        gameObject.SetActive(isShow);
        OnStateChange(isShow);
        IsOpen = isShow;
    }

    protected virtual void OnStateChange(bool isShow)
    { }



}

