using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour, IPickupable
{
    [SerializeField] int m_score = 0;
    [SerializeField] float animSpeed = 2;
    [SerializeField] float animRange = .125f;

    Vector3 startPosition;
    Vector3 offset;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        offset = new Vector3(0, Mathf.Sin(Time.time * animSpeed) * animRange, 0);
        transform.position = startPosition + offset;
    }

    public void pickUp()
    {
        GameManager.instance.addScore(m_score);
        Destroy(this.gameObject);
    }
}
