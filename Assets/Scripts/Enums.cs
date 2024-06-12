public enum Side
{
    PlayerSide,
    EnemySide
}

public enum BaseStatType : byte
{
    Health,
    MoveSpeed,
    TurnSpeed
}
public enum ShieldStatType
{
    Shield,
    ShieldRegen,
    ShieldRegenMultiplier,
    ShieldAbsorbMult
}

public enum EquipItemType
{
    None,
    MeleeWeap,
    RangedWeap,
    Shield,
    Booster,
    Costume,
    Modifier,
    Other = 255
}

public enum CombatActionType
{
    Melee,
    Ranged,
    Dodge,
    MeleeSpecialQ,
    RangedSpecialE,
    ShieldSpecialR
}

public enum TriggerTargetType
{
    TargetsEnemies,
    TargetsUser,
    TargetsAllies
}
public enum TriggerChangedValue : byte
{
    Health = 0,
    Energy = 10,
    Stamina = 20,
    MoveSpeed = 30,
    //TurnSpeed = 40,
    //Stagger = 50,
    //DodgeCharge = 60,
}

#region interface
public enum CursorType
{
    Menu,
    Explore,
    EnemyTarget,
    Item,
}
public enum DisplayValueType : byte
{
    Health = 0,
    Shield = 1,
    Combo = 2
}

public enum FontType
{
    Text,
    Button,
    Title
}



#endregion

public enum LevelType
{
    Menu,
    Scene,
    Game
}


public enum EffectMoment
{
    OnStart,
    OnUpdate,
    OnCollision,
    OnExpiry
}
public enum SkillState
{
    Placer,
    AoE
}

#region AI
public enum ReferenceUnitType : byte
{
    Small = 0,
    Big = 1,
    Boss = 2,
    Focus = 252,
    Self = 253,
    Any = 254,
    Player = 255
}


#endregion
public enum ControlledItemState : byte
{
    None,
    Negative,
    NegativeToPositive,
    Positive,
    PositiveToNegative,
}

public enum FaceExpression : byte
{
    Neutral,
    Happy,
    Angry,
    Action,
    Bothered
}

