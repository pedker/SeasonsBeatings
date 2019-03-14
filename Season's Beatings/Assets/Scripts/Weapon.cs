using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public float weaponRange { get; protected set; } = 1;
    public int durability { get; protected set; } = 15;
    public int maxDurability { get; protected set; } = 15;
    public Sprite sprite;

    public GameObject pickupVersion { get; set; }

    virtual public void Attack()
    {
        Debug.Log("Wrong Function");
    }

    virtual protected void OnEnable()
    {
        durability = maxDurability;
    }

    virtual public GameObject GetPickupVersion()
    {
        return pickupVersion;
    }

    virtual public bool checkDestroy()
    {
        //durability--;
        //if (durability == 0)
        //{
        //    Destroy(gameObject);
        //    return true;
        //}
        //else return false;
        return false;
    }
}
