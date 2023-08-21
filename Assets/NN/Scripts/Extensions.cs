using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using System.Drawing;
using Color = UnityEngine.Color;

public static class Extensions
{
    public static string Lerp(this string from, string to,float t)
    {
        if (t > 0.99)
            return to;
        if (to.Length < from.Length)
            to = to.PadRight(from.Length);
        else
            from = from.PadRight(to.Length);
        int border = (int)(to.Length * t);
        string first = to.Substring(0, border);
        string second = from.Substring(border);
        string result = first + second;
        return result;
    }
    public static bool Equals2(this Color color1,Color color2)
    {
        return (((int)(color1.r * 1000) == (int)(color2.r * 1000)) && ((int)(color1.b * 1000) == (int)(color2.b * 1000)) && ((int)(color1.g * 1000) == (int)(color2.g * 1000)));
    }
    public static Vector3 Map(this Vector3 value, Vector3 fromSource, Vector3 toSource, Vector3 fromTarget, Vector3 toTarget)
    {
        float x = value.x.Map(fromSource.x, toSource.x, fromTarget.x, toTarget.x);
        float y = value.y.Map(fromSource.y, toSource.y, fromTarget.y, toTarget.y);
        float z = value.z.Map(fromSource.z, toSource.z, fromTarget.z, toTarget.z);
        return new Vector3(x, y, z);
        /*
        var divtop = (value - fromSource);
        var divbottom = (toSource - fromSource);
        var mulA = new Vector3(divtop.x / divbottom.x, divtop.y / divbottom.y, divtop.z / divbottom.z);
        var mulB = (toTarget - fromTarget);
        var mul = new Vector3(mulA.x * mulB.x, mulA.y * mulB.y, mulA.z * mulB.z);
        return mul + fromTarget;*/
    }
    public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
    public static float Avg(this Vector3 value)
    {
        return (value.x + value.y + value.z) / 3;
    }
    public static IEnumerable<PropertyInfo> GetPropertiesWithoutAttribute<TType, TAttribute>()
    {
        Func<PropertyInfo, bool> matching =
                property => !property.GetCustomAttributes(typeof(TAttribute), false)
                                    .Any();

        return typeof(TType).GetProperties().Where(matching);
    }
    public static TType GetParentComponent<TType>(this Transform transform)
    {
        if (transform == null)
            return default(TType);
        if (transform.parent.GetComponent<TType>() == null)
            return transform.parent.GetParentComponent<TType>();
        else
            return transform.parent.GetComponent<TType>();
    }
    public static Quaternion LinePerpendicular(Vector3 p0,Vector3 p1)
    {
        Vector3 directionVector = p1 - p0;
        bool LeftToRight = Camera.main.WorldToScreenPoint(p0).x < Camera.main.WorldToScreenPoint(p1).x;
        Vector3 from = Vector3.right;
        if (!LeftToRight)
        {
            directionVector = -directionVector;
        }
        Quaternion rotate = Quaternion.FromToRotation(from, directionVector.normalized);
        return rotate;
    }
}