using Arcatech.Items;
using TMPro;
using UnityEngine;

public class TooltipComp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _desc;
    private RectTransform _rect;

    public RectTransform GetRect { get => _rect; }

    public void SetTexts(InventoryItem item)
    {
        if (_name == null | _desc == null)
        {
            Debug.LogWarning("Set fields for " + this);
            return;
        }
        if (item == null) return;
        _name.text = item.DisplayName;
        TextContainer descC;
        try
        {
            descC = TextsManager.Instance.GetContainerByID(item.ID);
        }
        catch
        {
            Debug.LogWarning($"No text description for {item.ID}");
        }
        _desc.text = item.Description.ToString();
    }
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }



}
