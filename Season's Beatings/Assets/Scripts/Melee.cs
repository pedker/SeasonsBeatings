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
    [SerializeField] AudioSource m_audioPlayer;
    [SerializeField] AudioClip whooshSound;
    [SerializeField] AudioClip whooshSound2;
    [SerializeField] AudioClip collisionSound;
    [SerializeField] float whooshVolume = 0.65f;
    [SerializeField] float whooshPitchMinimum = 0.90f;
    [SerializeField] float whooshPitchMaximum = 1.10f;
    [SerializeField] float collideSoundVolume = 0.65f;
    [SerializeField] float collidePitchMinimum = 0.95f;
    [SerializeField] float collidePitchMaximum = 1.05f;


    float attackSpeed;
    Collider2D collider2D;
    Rigidbody2D rigidbody2D;
    float time = 0;
    public bool attackReset = true;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        m_audioPlayer = GetComponent<AudioSource>();
        whooshSound = Resources.Load("Audio/SFX/Bat/BatSwing1") as AudioClip;
        whooshSound2 = Resources.Load("Audio/SFX/Bat/BatSwing2") as AudioClip;
        collisionSound = Resources.Load("Audio/SFX/Bat/BatHit") as AudioClip;
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
            

            StartCoroutine(playSoundCoroutine(whooshSound, whooshVolume, whooshPitchMinimum, whooshPitchMaximum));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            Physics2D.IgnoreCollision(other.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        else
        {
            if (attackReset)
            {
                Debug.Log("Collided with " + other.tag);
                IDamageable damageableComponent = other.GetComponent<IDamageable>();

                if (damageableComponent != null)
                {
                    damageableComponent.Damage(damage, stun, knockback * (Vector2)(other.transform.position - transform.position));
                }

                if (!other.CompareTag("Wall"))
                {
                    attackReset = false;
                    StartCoroutine(playSoundCoroutine(collisionSound, collideSoundVolume, collidePitchMinimum, collidePitchMaximum));
                }
            }
        }

    }

    private IEnumerator playSoundCoroutine(AudioClip sound, float soundVolume, float minimumPitch, float maximumPitch)
    {
        float timePassed = 0.0f;
        m_audioPlayer.pitch = Random.Range(minimumPitch, maximumPitch);
        m_audioPlayer.PlayOneShot(sound, soundVolume);

        while (timePassed < sound.length)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        m_audioPlayer.pitch = 1.0f;
    }
}
