using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{ 
    [Header("Combat Data")]
    [SerializeField] float knockback = 50;
    [SerializeField] float damage = 5;
    [SerializeField] float stun = 0.125f;

    //ADD SOUND VARIABLES

    //OTHER VARIABLES
    [SerializeField] GameObject bullet = null;
    [SerializeField] float rateOfFire = 0.5f;
    float attackTime = 0;

    private void Update()
    {
        attackTime += Time.deltaTime;
    }

    public override void Attack()
    {
        if (attackTime >= rateOfFire)
        {
            attackTime = 0;

            Vector3 projectilePlacement = this.transform.position + new Vector3(0, 0.5f, 0);
            GameObject newBullet = Instantiate(bullet, projectilePlacement, Quaternion.identity);
        }
    }
}
