using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public float weaponRange { get; protected set; } = 1;
    public GameObject pickupVersion;
    
    virtual public void Attack()
    {
        Debug.Log("Wrong Function");
    }
}
