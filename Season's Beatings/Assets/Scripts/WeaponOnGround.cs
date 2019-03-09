using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOnGround : MonoBehaviour, IPickupable
{
    [SerializeField] Weapon m_weapon = null;
    [SerializeField] Transform parent = null;
    
    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.FindWithTag("PlayerTorso").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickUp()
    {
        Destroy(this.gameObject);

        Weapon newWeapon = Instantiate(m_weapon, parent);

        Physics2D.IgnoreCollision(PlayerController.instance.GetComponent<Collider2D>(), newWeapon.GetComponent<Collider2D>());

        Destroy(PlayerController.instance.weapon.gameObject);

        PlayerController.instance.weapon = newWeapon;
    }
}
