using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
public static class Extensions
{
    public static Vector3 Map(this Vector3 value, Vector3 fromSource, Vector3 toSource, Vector3 fromTarget, Vector3 toTarget)
    {
        var divtop = (value - fromSource);
        var divbottom = (toSource - fromSource);

        var mulA = new Vector3(divtop.x / divbottom.x, divtop.y / divbottom.y, divtop.z / divbottom.z);
        var mulB = (toTarget - fromTarget);

        var mul = new Vector3(mulA.x * mulB.x, mulA.y * mulB.y, mulA.z * mulB.z);
        return mul + fromTarget;
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
}