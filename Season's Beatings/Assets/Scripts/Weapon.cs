﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public float weaponRange = 1;

    virtual public void Attack()
    {
        Debug.Log("Wrong Function");
    }
}
