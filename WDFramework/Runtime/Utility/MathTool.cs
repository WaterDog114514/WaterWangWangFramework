using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathTool
{
    //Deg�Ƕ� Rad����
    public static float DegToRad(float deg)
    {
        return deg * Mathf.Deg2Rad;
    }
    public static float RadToDeg(float rad)
    {
        return rad * Mathf.Rad2Deg;
    }
}
