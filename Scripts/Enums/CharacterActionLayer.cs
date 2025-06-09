public enum CharacterActionLayer
{
    None = 0,
    Legs = 1 << 0,
    UpperBody = 1 << 1,
    LeftArm = 1 << 2,
    Head = 1 << 3,

    FullbodyOverride = 1 << 4 | Legs | UpperBody | LeftArm | Head,

}