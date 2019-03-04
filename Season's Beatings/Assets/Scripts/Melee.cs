using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    [Header("Battle Values")]
    [SerializeField] float attackRate = .5f;
    [SerializeField] float knockback = 100;
    [SerializeField] float damage = 5;
    [SerializeField] float stun = .5f;
    bool attackReset = true;
    bool attackHitWall = false;

    [Header("Animation")]
    [SerializeField] float startArc = -75;
    [SerializeField] float endArc = 75;

    [Header("Sound")]
    [SerializeField] float whooshVolume = 0.65f;
    [SerializeField] float whooshPitchMinimum = 0.90f;
    [SerializeField] float whooshPitchMaximum = 1.10f;
    [SerializeField] float collideSoundVolume = 0.65f;
    [SerializeField] float collidePitchMinimum = 0.95f;
    [SerializeField] float collidePitchMaximum = 1.05f;

    AudioPlayer m_audioPlayer;
    float attackSpeed;
    Collider2D collider2D;
    Rigidbody2D rigidbody2D;
    float time = 0;
    

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        m_audioPlayer = GetComponentInChildren<AudioPlayer>();
        m_audioPlayer.addSFX("BatSwing1");
        m_audioPlayer.addSFX("BatSwing2");
        m_audioPlayer.addSFX("BatHit2");
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
            if (time < attackRate && !attackHitWall)
            {
                transform.Rotate(0, 0, attackSpeed * Time.deltaTime);
            }
            else
            {
                collider2D.enabled = false;
                transform.localRotation = Quaternion.Euler(0, 0, startArc);
                time = 0;
                attackReset = true;
                attackHitWall = false;
            }
        }
    }

    override public void Attack()
    {
        if (collider2D.enabled == false)
        {
            Debug.Log("Attacking");
            collider2D.enabled = true;
            
            m_audioPlayer.playSFX("BatSwing1", whooshVolume, whooshPitchMinimum, whooshPitchMaximum);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (attackReset)
        {
            Debug.Log("Collided with " + other.tag);
            IDamageable damageableComponent = other.GetComponent<IDamageable>();

            if (damageableComponent != null)
            {
                damageableComponent.Damage(damage, stun, knockback * (Vector2)(other.transform.position - transform.position));
            }

            if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Wall")) //So it registers a hit and plays sounds only when hitting enemies, players or walls
            {
                if (other.CompareTag("Wall"))
                {
                    attackHitWall = true;
                }

                attackReset = false;
                m_audioPlayer.playSFX("BatHit2", collideSoundVolume, collidePitchMinimum, collidePitchMaximum);
            }
        }
    }

}
