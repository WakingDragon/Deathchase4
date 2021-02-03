using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float ClampedPercent(float numerator, float denominator)
    {
        return Mathf.Clamp(numerator / denominator, 0f, 1f);
    }

    public static int Mod(int i, int m)
    {
        return (i % m + m) % m;
    }

    public static float ModF(float i, float m)
    {
        return (i % m + m) % m;
    }

    public static bool Chance(float probability)    //rolls the dice
    {
        if (Random.Range(0f, 1f) < Mathf.Clamp(probability,0f,1f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool Magnitude(float i, float min)    //checks float is +/- greater than min
    {
        if(i > min || i < -min) { return true; }
        return false;
    }
}
