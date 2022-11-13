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
    Other = 255
}
public enum DodgeStatType
{
    Charges,
    Range,
    Cooldown,
    Speed
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
    Shield = 10,
    Combo = 20,
    MoveSpeed = 30,
    TurnSpeed = 40,
    Stagger = 50
}


public enum CursorType
{
    Menu,
    Explore,
    EnemyTarget,
    Item,
}
public enum UnitType : byte
{
    Small = 0,
    Big = 1,
    Boss = 2,
    Self = 253,
    Any = 254,
    Player = 255
}

public enum TextType
{
    Tutorial,
    Story,
    Description
}

public enum GameMenuType
{
    Items = 0,
    Pause = 255
}

public enum InteractiveItemType : byte
{
    Enemy = 0,
    Pickup = 10,

}

public enum GameMode
{
    Menus,
    Paused,
    Gameplay
}
