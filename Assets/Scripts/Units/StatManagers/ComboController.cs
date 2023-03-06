using System;
[Serializable]
public class ComboController : BaseController, IStatsComponentForHandler, ITakesTriggers
{

    public StatValueContainer ComboContainer { get; }


    protected float Degen;
    protected float Timeout;
    private float _currentTimeout = 0f;

    public ComboController(string ID)
    {
        var cfg = DataManager.Instance.GetConfigByID<ComboStatsConfig>(ID);
        if (cfg == null) return;
        IsReady = true;

        ComboContainer = new StatValueContainer(cfg.ComboContainer);
        Degen = cfg.DegenCoeff;
        Timeout = cfg.HeatTimeout;
    }

    public bool UseCombo(float value)
    {
        bool result = ComboContainer.GetCurrent >= -value;    // because these are negative in configs
        if (result) ComboContainer.ChangeCurrent(value);
        return result;
    }

    public override void SetupStatsComponent()
    {
        ComboContainer.Setup();
    }
    public override void UpdateInDelta(float deltaTime)
    {

        base.UpdateInDelta(deltaTime);
        if (_currentTimeout <= Timeout)
        {
            _currentTimeout += deltaTime;
            return;
        }
        ComboContainer.ChangeCurrent(-Degen * deltaTime);
    }

    protected override StatValueContainer SelectStatValueContainer(TriggeredEffect effect)
    {
        return ComboContainer;
    }
}

