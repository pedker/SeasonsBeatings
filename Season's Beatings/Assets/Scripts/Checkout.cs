using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkout : MonoBehaviour
{
    public Text scoreText;
    public Text priceText;
    public GameObject winMenu;

    private int score;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            score = GameObject.Find("Game Manager").GetComponent<GameManager>().score;
            winMenu.SetActive(true);
            scoreText.text = "Total Value of Your Cart: $" + score;
            priceText.text = "You Paid: $" + score * .25f;
        }
    }
}
