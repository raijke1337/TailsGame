using System.Collections.Generic;
using UnityEngine;

public class TextsManager : MonoBehaviour
{
    #region SingletonLogic

    public static TextsManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    #endregion

    private void Start()
    {
        var texts = DataManager.Instance.GetAssetsOfType<TextContainerSO>(Constants.Texts.c_TextsPath);
        foreach (var text in texts)
        {
            textContainers[text.container.ID] = text.container;
        }
       // Debug.Log($"Found total {textContainers.Count} texts");
    }

    private Dictionary<string, TextContainer> textContainers = new Dictionary<string, TextContainer>();

    public TextContainer GetContainerByID(string ID) => textContainers[ID];



}

