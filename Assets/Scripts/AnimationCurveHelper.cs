using UnityEngine;

public static class AnimationCurveHelper
{
    public static void ChangeFirstKeyframe(AnimationCurve curve, float value)
    {
        Keyframe startKeyframe = curve[0];
        startKeyframe.value = value;
        curve.MoveKey(0, startKeyframe);
    }

    public static Keyframe LastKey(AnimationCurve curve)
    {
        return curve.keys[Mathf.Max(0, curve.length - 1)];
    }
}