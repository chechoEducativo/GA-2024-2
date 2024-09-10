using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransformUtils
{
    public static TComponent Query<TComponent>(this Transform t, string name) where TComponent : Component
    {
        return t.GetComponentsInChildren<Transform>().OfType<TComponent>().Where(t => t.name == name).FirstOrDefault();
    }
}
