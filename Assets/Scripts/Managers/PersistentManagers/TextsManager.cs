using System.Collections.Generic;
using UnityEngine;

public class TextsManager : SingletonManagerBase
{
    private Dictionary<string, TextContainer> textContainers = new Dictionary<string, TextContainer>();

    public TextContainer GetContainerByID(string ID) => textContainers[ID];

    public override void SetupManager()
    {
        var texts = Extensions.GetAssetsOfType<TextContainerSO>(Constants.Texts.c_TextsPath);

        foreach (var text in texts)
        {
            textContainers[text.container.ID] = text.container;
        }
        Debug.Log($"Found total {textContainers.Count} texts");

    }


    #region SingletonLogic

    protected static TextsManager _instance = null;
    public override void InitSingleton()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance == this) Destroy(gameObject); // remove copies just in case
        SetupManager();
    }
    public static TextsManager GetInstance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
}

