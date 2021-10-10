using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class JoanUtils
{
    public static List<T> myGetComponentsInChildren<T>(GameObject go) where T:Component
    {
        return FindDeepChilds(go.transform, p => (T)p.GetComponent(typeof(T)) != null, null).Select(p => (T)p.GetComponent(typeof(T))).ToList<T>();
    }
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = FindDeepChild(child, aName);
            if (result != null)
                return result;
        }
        return null;
    }
    public delegate bool FindDeepChildsCondition(Transform aParent);
    public static List<Transform> FindDeepChilds(this Transform aParent, FindDeepChildsCondition fdp, List<Transform> lt = null)
    {
        if (lt == null) lt = new List<Transform>();
        if (fdp(aParent)) lt.Add(aParent);
        foreach (Transform child in aParent)
        {
            FindDeepChilds(child, fdp, lt);
        }
        return lt;
    }

}