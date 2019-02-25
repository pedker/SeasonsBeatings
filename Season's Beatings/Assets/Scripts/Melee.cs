using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    [SerializeField] float range = 1;
    [SerializeField] float speed = 5;

    [SerializeField] float startArc = -75;
    [SerializeField] float endArc = 75;

    Collider2D collider2D;
    Rigidbody2D rigidbody2D;
    float time = 0;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, startArc);
        collider2D.enabled = false;
    }

    private void Update()
    {
        if (collider2D.enabled)
        {
            time += Time.deltaTime;
            if (time >= speed)
            {
                collider2D.enabled = false;
                rigidbody2D.angularVelocity = 0;
                transform.localRotation = Quaternion.Euler(0, 0, startArc);
                time = 0;
            }
        }
    }

    override public void Attack()
    {
        if (collider2D.enabled == false)
        {
            Debug.Log("Attacking");
            collider2D.enabled = true;
            rigidbody2D.angularVelocity = (endArc - startArc) / speed;
        }
    }
}
