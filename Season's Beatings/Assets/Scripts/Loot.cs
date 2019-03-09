using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour, IPickupable
{
    [SerializeField] int m_score = 0;

    public void pickUp()
    {
        GameManager.instance.addScore(m_score);
        Destroy(this.gameObject);
    }
}
