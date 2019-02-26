using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    [SerializeField] float attackRate = .5f;
    [SerializeField] float knockback = 100;
    [SerializeField] float damage = 5;
    [SerializeField] float stun = .5f;


    [SerializeField] float startArc = -75;
    [SerializeField] float endArc = 75;
    float attackSpeed;

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
        if (attackRate != 0) attackSpeed = (endArc - startArc) / attackRate;
        else attackSpeed = 0;
        collider2D.enabled = false;
    }

    private void Update()
    {
        if (collider2D.enabled)
        {
            time += Time.deltaTime;
            if (time < attackRate)
            {
                transform.Rotate(0, 0, attackSpeed * Time.deltaTime);
            }
            else
            {
                collider2D.enabled = false;
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
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with " + other.name);

        IDamageable damageableComponent = other.GetComponent<IDamageable>();

        if (damageableComponent != null)
        {
            damageableComponent.Damage(damage, stun, knockback * (Vector2)(other.transform.position - transform.position));
        }
    }
}
