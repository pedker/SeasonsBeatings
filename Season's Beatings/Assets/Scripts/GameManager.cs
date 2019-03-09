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

    void Awake()
    {
        instance = this;
    }

    public void addScore(int score)
    {
        this.score += score;
    }
}
