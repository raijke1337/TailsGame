using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DodgeController : BaseController, IStatsComponentForHandler, IUsesItems, ITakesTriggers
{
    public IEquippable EquippedDodgeItem { get; private set; }

    Dictionary<DodgeStatType, StatValueContainer> _stats;
    public IReadOnlyDictionary<DodgeStatType, StatValueContainer> GetDodgeStats { get { return _stats; } }
    public ItemEmpties Empties { get; }
    public DodgeController(ItemEmpties ie) => Empties = ie;
    public int GetDodgeCharges() => _stats != null ? (int)_stats[DodgeStatType.Charges].GetCurrent : 0;

    private Queue<Timer> _timerQueue = new Queue<Timer>();
    private EquipmentBase instantiatedItem;

    public override void SetupStatsComponent()
    {
        if (!IsReady) return;
        _stats = new Dictionary<DodgeStatType, StatValueContainer>();
        var cfg = DataManager.Instance.GetConfigByID<DodgeStatsConfig>(EquippedDodgeItem.GetContents.ID);

        if (cfg == null)
        {
            IsReady = false;
            throw new Exception($"Mising cfg by ID {EquippedDodgeItem.GetContents.ID} from item {EquippedDodgeItem.GetID} : {this}");
        }

        foreach (var c in cfg.Stats)
        {
            _stats[c.Key] = new StatValueContainer(c.Value);
        }
        foreach (var st in _stats.Values)
        { st.Setup(); }
    }


    public bool IsDodgePossibleCheck()
    {
        if (_stats == null) return false;
        if (_stats[DodgeStatType.Charges].GetCurrent == 0f) return false;
        else
        {
            _stats[DodgeStatType.Charges].ChangeCurrent(-1);
            var t = new Timer(_stats[DodgeStatType.Cooldown].GetCurrent);
            _timerQueue.Enqueue(t);
            t.TimeUp += T_TimeUp;
            return true;
        }
    }

    private void T_TimeUp(Timer arg)
    {
        _timerQueue.Dequeue();
        _stats[DodgeStatType.Charges].ChangeCurrent(1);
    }

    public override void UpdateInDelta(float deltaTime)
    {
        foreach (var timer in _timerQueue.ToList()) timer.TimerTick(deltaTime);
        base.UpdateInDelta(deltaTime);
    }

    public void LoadItem(IEquippable item)
    {
        if (!(item.GetContents.ItemType == EquipItemType.Booster)) return;
        else
        {
            EquippedDodgeItem = item;
            instantiatedItem = GameObject.Instantiate(item.GetEquipmentBase(), Empties.SheathedWeaponEmpty.position, Empties.SheathedWeaponEmpty.rotation, Empties.SheathedWeaponEmpty);
            IsReady = true;
        }
    }

    protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
    {
        return _stats[DodgeStatType.Charges];
    }

}



