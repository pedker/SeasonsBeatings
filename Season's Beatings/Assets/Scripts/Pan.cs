using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Weapon
{
    [Header("Battle Stats")]
    [SerializeField] float attackDuration = .4f;
    [SerializeField] public bool attacking = false;
    [SerializeField] public bool attackReset = false;
    [SerializeField] float knockback = 75;
    [SerializeField] float damage = 34;
    [SerializeField] float stun = 0.6f;
    float attackTime = 0;

    /*
    [Header("Sound")]
    [SerializeField] float whooshVolume = 0.65f;
    [SerializeField] float whooshPitchMinimum = 0.90f;
    [SerializeField] float whooshPitchMaximum = 1.10f;
    [SerializeField] string panWhooshSoundEffect = null;
    [SerializeField] float collideSoundVolume = 0.65f;
    [SerializeField] float collidePitchMinimum = 0.95f;
    [SerializeField] float collidePitchMaximum = 1.05f;
    [SerializeField] string panSoundEffect = null;


    AudioPlayer m_audioPlayer;
    */
    public Animator m_animator = null;

    void Awake()
    {
        /*
        m_audioPlayer = GetComponentInChildren<AudioPlayer>();
        m_audioPlayer.addSFX(punchWhooshSoundEffect);
        m_audioPlayer = GetComponentInChildren<AudioPlayer>();
        m_audioPlayer.addSFX(punchSoundEffect);
        */
        m_animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        weaponRange = .75f * transform.lossyScale.x;
    }

    void Update()
    {
        if (attacking)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= attackDuration)
            {
                attacking = false;
                attackReset = true;
                attackTime = 0;
            }
        }
    }

    override public void Attack()
    {

        if (!attacking)
        {
            attacking = true;
            attackReset = false;
            attackTime = 0;

            m_animator.Play("PanSwing", 0, 0.0f);
            
            //m_audioPlayer.playSFX(panWhooshSoundEffect, whooshVolume, whooshPitchMinimum, whooshPitchMaximum);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!attackReset)
        {
            IDamageable damageableComponent = other.GetComponent<IDamageable>();
            if (damageableComponent != null)
            {
                damageableComponent.Damage(damage, stun, knockback * (Vector2)(other.transform.position - transform.position));
                attackReset = true;
            }

            if (other.CompareTag("Player") || other.CompareTag("Enemy")) //So it registers a hit and plays sounds only when hitting enemies or players
            {
                //m_audioPlayer.playSFX(punchSoundEffect, collideSoundVolume, collidePitchMinimum, collidePitchMaximum);
            }
        }
    }
}
