using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOnGround : MonoBehaviour, IPickupable
{
    [SerializeField] Weapon m_weapon = null;
    [SerializeField] Transform parent = null;
    [SerializeField] Transform rotator = null;
    [SerializeField] float rotationMult = 50;


    [Header("Audio")]
    [SerializeField] string dropFileName = null;
    [SerializeField] float dropVolume = 0.65f;
    [SerializeField] float dropPitchMinimum = 0.95f;
    [SerializeField] float dropPitchMaximum = 1.05f;

    AudioPlayer m_audioPlayer;

    void Awake()
    {
        m_audioPlayer = this.GetComponentInChildren<AudioPlayer>();
        m_audioPlayer.addSFX(dropFileName);
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.FindWithTag("PlayerTorso").transform;

        m_audioPlayer.playSFX(dropFileName, dropVolume, dropPitchMinimum, dropPitchMaximum);
    }

    // Update is called once per frame
    void Update()
    {
        rotator.localRotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotationMult);
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
