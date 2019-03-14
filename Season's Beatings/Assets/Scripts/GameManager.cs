using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int score { get; private set; } = 0;

    [SerializeField] private double time = 300;
    public double countdown { get; private set; }


    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        countdown = time;
    }

    private void Update()
    {
        if (countdown >= Time.deltaTime) countdown -= Time.deltaTime;
        else countdown = 0;
    }

    public void addScore(int score)
    {
        this.score += score;
    }
}
