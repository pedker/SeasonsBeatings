﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float horizontalLimit = 2;
    [SerializeField] private float verticalLimit = 2;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = PlayerController.instance.transform.position;
        float newX = transform.position.x;
        float newY = transform.position.y;
        AudioSource a;
        

        if (playerPos.x - newX > horizontalLimit) newX += playerPos.x - newX - horizontalLimit;
        else if (playerPos.x - newX < -horizontalLimit) newX += playerPos.x - newX + horizontalLimit;
        if (playerPos.y - newY > verticalLimit) newY += playerPos.y - newY - verticalLimit;
        else if (playerPos.y - newY < -verticalLimit) newY += playerPos.y - newY + verticalLimit;

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
