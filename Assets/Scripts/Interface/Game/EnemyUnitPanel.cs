using UnityEngine;
using UnityEngine.UI;

public class EnemyUnitPanel : SelectableItemPanel
{
    private StatValueContainer _health;
    [SerializeField] private Image _fill;
    public override void AssignItem(SelectableItem item)
    {
        base.AssignItem(item);
        _health = item.GetComponent<BaseUnit>().GetStats[BaseStatType.Health];
    }

    public void UpdateBars(float delta)
    {
        _fill.fillAmount = _health.GetCurrent / _health.GetMax;
    }
}
