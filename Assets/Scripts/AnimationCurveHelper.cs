using UnityEngine;

public static class AnimationCurveHelper
{
    public static void ChangeFirstKeyframeValue(AnimationCurve curve, float value)
    {
        Keyframe startKeyframe = curve[0];
        startKeyframe.value = value;
        
        curve.MoveKey(0, startKeyframe);
    }

    public static void ChangeLastKeyframeValue(AnimationCurve curve, float value)
    {
        int lastIndex = Mathf.Max(0, curve.length - 1);
        Keyframe lastKeyframe = curve.keys[lastIndex];
        lastKeyframe.value = value;
        
        curve.MoveKey(lastIndex, lastKeyframe);
    }
    
    public static void ChangeLastKeyframeTime(AnimationCurve curve, float time)
    {
        int lastIndex = Mathf.Max(0, curve.length - 1);
        Keyframe lastKeyframe = curve.keys[lastIndex];
        lastKeyframe.time = time;
        
        curve.MoveKey(lastIndex, lastKeyframe);
    }

    public static Keyframe LastKey(AnimationCurve curve)
    {
        return curve.keys[Mathf.Max(0, curve.length - 1)];
    }
}