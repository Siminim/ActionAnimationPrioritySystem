using Godot;

public static class Delegates
{
    // default
    public delegate void VoidDelegate();

    // returns
    public delegate float FloatReturnDelegate();
    public delegate bool BoolReturnDelegate();

    // parameters

    // public delegate void CharacterMoveStateParameterDelegate(CharacterLocomotionState characterState);
    // public delegate void CharacterUpperBodyStateParameterDelegate(CharacterUpperBodyState characterState);

    public delegate void FloatParameterDelegate(float value);
    public delegate void DoubleParameterDelegate(double value);
    public delegate void BoolParameterDelegate(bool value);

    public delegate void Node3DParameterDelegate(Node3D node);
    public delegate void Vector2DoubleParameterDelegate(Vector2 direction, double delta);
    public delegate void MouseMotionDelegate(InputEventMouseMotion mouseMotion);
}

public struct FloatModifiers
{
    public Delegates.FloatReturnDelegate Additive;
    public Delegates.FloatReturnDelegate Subtractive;
    public Delegates.FloatReturnDelegate Multiplier;
    public Delegates.FloatReturnDelegate Divider;

    private float GetAddedTotal(Delegates.FloatReturnDelegate floatReturnDelegate, float failsafe = 1.0f)
    {
        if (floatReturnDelegate == null || floatReturnDelegate.GetInvocationList().Length == 0)
            return failsafe;

        float total = 0.0f;

        foreach (Delegates.FloatReturnDelegate floatDelegate in floatReturnDelegate.GetInvocationList())
        {
            total += floatDelegate.Invoke();
        }

        return total;
    }

    public float GetUnsafeOutput()
    {
        return (GetAddedTotal(Additive, 0.0f) - GetAddedTotal(Subtractive, 0.0f)) * GetAddedTotal(Multiplier) / GetAddedTotal(Divider);
    }

    /// <summary>
    /// Ensures that the sum is at least 1, multipliers don't lower the number, and dividers don't raise the number.
    /// </summary>
    public float GetOutput()
    {
        float add = GetAddedTotal(Additive, 0.0f);
        float sub = GetAddedTotal(Subtractive, 0.0f);

        // Negative numbers here can cause issues
        float sum = Mathf.Max(0.0f, add - sub);

        // Make sure that the final value is at least 1
        float mul = Mathf.Max(1.0f, GetAddedTotal(Multiplier));
        float div = Mathf.Max(1.0f, GetAddedTotal(Divider));

        return sum * mul / div;
    }
}