using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon, IDropable
{
    //ADD SOUND VARIABLES

    //OTHER VARIABLES
    [SerializeField] public GameObject pickupVersion = null;
    [SerializeField] GameObject bulletSpawn = null;
    [SerializeField] GameObject bullet = null;
    [SerializeField] float rateOfFire = 1f;
    [SerializeField] public float weaponRange = 10;
    float attackTime = 1f;
    PlayerController player;

    private void Awake()
    {
        player = PlayerController.instance;
    }

    private void Update()
    {
        attackTime += Time.deltaTime;
    }

    public override void Attack()
    {
        if (attackTime >= rateOfFire)
        {
            attackTime = 0;
            GameObject newBullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        }
    }

    public void Drop()
    {
        Destroy(this.gameObject);
        Instantiate(pickupVersion, PlayerController.instance.transform.position, Quaternion.identity);
    }
}
