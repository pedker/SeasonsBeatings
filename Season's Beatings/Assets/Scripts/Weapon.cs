using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour, IPickupable
{
    private PlayerController player;

    void Awake()
    {
        player = PlayerController.instance;
    }

    virtual public void Attack()
    {
        Debug.Log("Wrong Function");
    }

    virtual public void pickUp()
    {
        player.weapon = this;
    }
}
