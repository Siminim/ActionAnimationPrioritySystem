using Godot;
using System;
using System.Collections.Generic;

public partial class LookupTables : Node
{
    public static Dictionary<WeaponType, string> WeaponAnimationLibrary = new Dictionary<WeaponType, string>()
    {
        { WeaponType.Fists, "Fist_Attack_"},
    };
}

