using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using System.Reflection;

public class Bonus : MonoBehaviour
{
    Quaternion turn = Quaternion.Euler(0, 0.5f, 0f);
    MethodInfo mi;
    private void Start()
    {
        mi = typeof(BonusEffects).GetMethod(name);
        Debug.Log(mi);
    }
    void Update()
    {
        Rotate();
    }
    void Rotate()
    {
        transform.localRotation *= turn;
    }
    public void Use()
    {
        mi.Invoke(BonusEffects.instance, null);
    }
}
