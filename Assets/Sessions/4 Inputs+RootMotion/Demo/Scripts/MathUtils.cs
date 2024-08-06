using UnityEngine;

public static class MathUtils
{
    public static float ShaderLikeSmoothstep(float a, float b, float x)
    {
        float t = Mathf.Clamp01((x - a) / (b - a));
        return t * t * (3 - (2 * t));
    }
}
