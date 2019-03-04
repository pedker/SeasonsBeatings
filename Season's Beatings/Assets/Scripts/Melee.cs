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

    [Header("Animation")]
    [SerializeField] float startArc = -75;
    [SerializeField] float endArc = 75;

    [Header("Sound")]
    [SerializeField] string whooshFileName;
    [SerializeField] string whooshFileName_2;
    [SerializeField] float whooshVolume = 0.65f;
    [SerializeField] float whooshPitchMinimum = 0.90f;
    [SerializeField] float whooshPitchMaximum = 1.10f;

    [SerializeField] string collideFileName;
    [SerializeField] float collideSoundVolume = 0.65f;
    [SerializeField] float collidePitchMinimum = 0.95f;
    [SerializeField] float collidePitchMaximum = 1.05f;

    AudioPlayer m_audioPlayer;
    float attackSpeed;
    Collider2D collider2D;
    Rigidbody2D rigidbody2D;
    float time = 0;
    public bool attackReset = true;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        m_audioPlayer = GetComponentInChildren<AudioPlayer>();
        m_audioPlayer.addSFX(whooshFileName);
        m_audioPlayer.addSFX(whooshFileName_2);
        m_audioPlayer.addSFX(collideFileName);
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
                attackReset = true;
            }
        }
    }

    override public void Attack()
    {
        if (collider2D.enabled == false)
        {
            Debug.Log("Attacking");
            collider2D.enabled = true;

            int random = Random.Range(0, 2);
            if (random == 0)
            {
                m_audioPlayer.playSFX(whooshFileName, whooshVolume, whooshPitchMinimum, whooshPitchMaximum);
            }
            else
            {
                m_audioPlayer.playSFX(whooshFileName_2, whooshVolume, whooshPitchMinimum, whooshPitchMaximum);
            }
            
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

            if (other.CompareTag("Player") || other.CompareTag("Enemy")) //So it registers a hit and plays sounds only when hitting enemies or players
            {
                attackReset = false;
                m_audioPlayer.playSFX(collideFileName, collideSoundVolume, collidePitchMinimum, collidePitchMaximum);
            }
        }
    }

}
