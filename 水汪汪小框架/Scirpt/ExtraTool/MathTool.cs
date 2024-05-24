using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathTool
{
    //Deg½Ç¶È Rad»¡¶È
    public static float DegToRad(float deg)
    {
        return deg * Mathf.Deg2Rad;
    }
    public static float RadToDeg(float rad)
    {
        return rad * Mathf.Rad2Deg;
    }
}
