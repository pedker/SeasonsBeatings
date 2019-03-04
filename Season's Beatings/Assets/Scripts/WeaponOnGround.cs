using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOnGround : MonoBehaviour, IPickupable
{
    [SerializeField] Weapon m_weapon;
    [SerializeField] Transform parent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickUp()
    {
        Destroy(this.gameObject);

        var newWeapon = Instantiate(m_weapon, parent);

        Destroy(PlayerController.instance.weapon.gameObject);

        PlayerController.instance.weapon = newWeapon;
    }
}
