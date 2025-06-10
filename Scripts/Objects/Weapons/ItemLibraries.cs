using Godot;
using System;
using System.Collections.Generic;

public class ItemLibraries
{
    public static readonly Dictionary<ItemType, HeldItem> heldItemLibrary = new Dictionary<ItemType, HeldItem>()
    {
        { ItemType.None, new HeldItem( new string[] { "RightPunch", "LeftHook" }, new string[] { }) }

    };
}

public class HeldItem
{
    public readonly string[] useAnimations;
    public readonly string[] alternateUseAnimations;

    public HeldItem(string[] useAnimations, string[] alternateUseAnimations)
    {
        this.useAnimations = useAnimations;
        this.alternateUseAnimations = alternateUseAnimations;
    }
}