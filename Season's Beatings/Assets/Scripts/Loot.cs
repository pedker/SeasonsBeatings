using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour, I_item
{
    GameManager gameManager;
    [SerializeField] int m_score = 0;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void pickUp()
    {
        Destroy(this.gameObject);
        gameManager.score += m_score;
    }
}
