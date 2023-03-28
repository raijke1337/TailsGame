using UnityEngine;

public abstract class EquipmentBase : ItemBase, IEquippable//, IHasSounds
{
    public BaseUnit Owner { get; set; }
    public EquipmentBase GetEquipmentBase { get => this; }
    public GameObject GetObject() => gameObject;

    [SerializeField] public AudioComponentBase Sounds; // todo load from configs or something
    private void Start()
    {
        if (Sounds.SoundsDict.Count == 0) Debug.Log($"{this} on {Owner} has no sounds set up");
    }

}

