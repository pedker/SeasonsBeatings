using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandToHand : Weapon
{
    [Header("Combat Data")]
    [SerializeField] float knockback = 50;
    [SerializeField] float damage = 5;
    [SerializeField] float stun = 0.125f;


    [Header("Sound")]
    [SerializeField] float whooshVolume = 0.65f;
    [SerializeField] float whooshPitchMinimum = 0.90f;
    [SerializeField] float whooshPitchMaximum = 1.10f;
    [SerializeField] float collideSoundVolume = 0.65f;
    [SerializeField] float collidePitchMinimum = 0.95f;
    [SerializeField] float collidePitchMaximum = 1.05f;
    [SerializeField] string punchWhooshSoundEffect = "";
    [SerializeField] string punchSoundEffect = "";

    AudioPlayer m_audioPlayer;
    public Animator m_animator = null;

    void Awake()
    {
        m_audioPlayer = GetComponentInChildren<AudioPlayer>();
        m_animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("LeftClick");
            Attack();
        }
    }

    override public void Attack()
    {
        Debug.Log("Attacking");
        int randNum = Random.Range(0, 2);
        if (randNum == 1)
        {
            m_animator.Play("PunchingLeftArm", 1, 0.0f);
            m_audioPlayer.playSFX(punchWhooshSoundEffect, whooshVolume, whooshPitchMinimum, whooshPitchMaximum);
        }

        else
        {
            m_animator.Play("PunchingRightArm", 1, 0.0f);
            m_audioPlayer.playSFX(punchWhooshSoundEffect, whooshVolume, whooshPitchMinimum, whooshPitchMaximum);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
            Debug.Log("Collided with " + other.tag);
            IDamageable damageableComponent = other.GetComponent<IDamageable>();

            if (damageableComponent != null)
            {
                damageableComponent.Damage(damage, stun, knockback * (Vector2)(other.transform.position - transform.position));
            }

            if (other.CompareTag("Player") || other.CompareTag("Enemy")) //So it registers a hit and plays sounds only when hitting enemies or players
            {
                m_audioPlayer.playSFX(punchSoundEffect, collideSoundVolume, collidePitchMinimum, collidePitchMaximum);
            }
    }
}
