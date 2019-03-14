﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable, IHealable
{
    public static PlayerController instance;

    new Rigidbody2D rigidbody2D;
    new Collider2D collider2D;
    public Animator animator = null;

    [SerializeField] GameObject playerTorso = null;
    [SerializeField] GameObject playerLegs = null;
    [SerializeField] GameObject playerArmLeft = null;
    [SerializeField] GameObject playerArmRight = null;
    [SerializeField] GameObject Blood = null;
    [SerializeField] ParticleSystem BloodParticles = null;
    //[SerializeField] ScreenShake screenShake = null;

    [SerializeField] Weapon defaultWeapon = null;
    public Weapon weapon;
    bool hasWeapon = false;

    [SerializeField] float speed = 5f;
    [SerializeField] float stun = 0;
    [SerializeField] bool faceTarget = true;

    [Header("HealthUI")]
    [SerializeField] float maxHealth = 200f;
    [SerializeField] float health = 100f;
    public Slider slider;
    public Image fillImage;
    public Color FullHealthColor;
    public Color ZeroHealthColor;

    [Header("Sound")]
    [SerializeField] string footStepFileName = null;
    [SerializeField] float footStepVolume = 0.30f;
    [SerializeField] float footStepPitchMin = 0.9f;
    [SerializeField] float footStepPitchMax = 1.1f;

    [SerializeField] string damagedFileName = null;
    [SerializeField] float damagedVolume = 0.30f;
    [SerializeField] float damagedPitchMin = 0.9f;
    [SerializeField] float damagedPitchMax = 1.1f;

    [SerializeField] string deathFileName = null;
    [SerializeField] float deathVolume = 0.30f;
    [SerializeField] float deathPitchMin = 0.9f;
    [SerializeField] float deathPitchMax = 1.1f;

    AudioPlayer m_audioPlayer;


    void Awake()
    {
        instance = this;
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        collider2D = this.GetComponent<Collider2D>();
        m_audioPlayer = this.GetComponentInChildren<AudioPlayer>();
        m_audioPlayer.addSFX(footStepFileName);
        m_audioPlayer.addSFX(damagedFileName);
        m_audioPlayer.addSFX(deathFileName);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (weapon.name != "Arms")
            Physics2D.IgnoreCollision(collider2D, weapon.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(collider2D, playerArmLeft.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(collider2D, playerArmRight.GetComponent<Collider2D>());
        SetHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (animator)
            {
                animator.SetFloat("Speed", Mathf.Abs(this.rigidbody2D.velocity.x) + Mathf.Abs(this.rigidbody2D.velocity.y));
            }

            if (Input.GetKeyDown(KeyCode.Q)) DropWeapon();

            if (stun > 0) stun -= Time.deltaTime;
        
            else
            {

                playerTorso.transform.right = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

                rigidbody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
                if (rigidbody2D.velocity != Vector2.zero)
                {
                    playerLegs.transform.right = rigidbody2D.velocity;
                    if (faceTarget && Quaternion.Angle(playerTorso.transform.rotation, playerLegs.transform.rotation) > 90) playerLegs.transform.right = -1 * playerLegs.transform.right; // Keeps body facing mouse
                }

                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
                {
                    weapon.Attack();
                }
            }
        }
        //DEBUG
        //Debug.DrawRay(transform.position, playerTorso.transform.right.normalized, Color.red, .01f);
        //Debug.DrawRay(transform.position, playerLegs.transform.up, Color.green, .01f); 
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Checkout"))
        {
            UIManager.instance.EndGame(true);
        }

        else
        {
            if (collider.CompareTag("WeaponGround"))
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.E))
                {
                    IPickupable itemComponent = collider.GetComponent<IPickupable>();
                    if (itemComponent != null)
                    {
                        if (hasWeapon)
                            Instantiate(weapon.GetPickupVersion(), transform.position, Quaternion.identity);
                        itemComponent.pickUp();
                        hasWeapon = true;
                    }
                }
            }
            else
            {
                IPickupable itemComponent = collider.GetComponent<IPickupable>();
                if (itemComponent != null)
                {
                    itemComponent.pickUp();
                }
            }
        }
    }    

    public void Damage(float damage, float stun, Vector2 knockback)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(knockback);
        health -= damage;
        SetHealthUI();
        this.stun = stun;
        BloodParticles.Play();
        //StartCoroutine(screenShake.Shake(.02f, .05f));

        m_audioPlayer.playSFX(damagedFileName, damagedVolume, damagedPitchMin, damagedPitchMax);

        if (health <= 0)
        {
            m_audioPlayer.playSFX(deathFileName, deathVolume, deathPitchMin, deathPitchMax);
            UIManager.instance.EndGame(false);
        }
    }

    public void Heal(float healthDelta)
    {
        float newHealthValue = health + healthDelta;
        if (newHealthValue > maxHealth)
            health = maxHealth;
        else
            health = newHealthValue;
    }
    
    private void DropWeapon()
    {
        if (hasWeapon)
        {
            IDropable dropComponent = weapon.GetComponent<IDropable>();
            if (dropComponent != null)
            {
                Debug.Log("Q");
                dropComponent.Drop();
                Debug.Log("P");
                weapon = defaultWeapon;
                Debug.Log("R");
                hasWeapon = false;
                Debug.Log("S");
            }
        }
    }

    private void SetHealthUI()
    {
        slider.value = health;

        fillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, health / 100); // 100 is the hardcoded starting health, might need to change later
    }


    private void playFootStepSFX()
    {
        m_audioPlayer.playSFX(footStepFileName, footStepVolume, footStepPitchMin, footStepPitchMax);
    }
}
