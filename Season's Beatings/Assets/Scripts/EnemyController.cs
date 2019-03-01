using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] GameObject EnemyTorso;
    [SerializeField] Weapon weapon;
    [SerializeField] GameObject EnemyLegs;
    Rigidbody2D rigidbody2D;
    Collider2D collider2D;

    [Header("Attributes")]
    [SerializeField] float range = 10f;
    [SerializeField] float health = 100f;
    [SerializeField] float stun = 0;
    [SerializeField] float viewAngle = 45f;
    [SerializeField] float speed = 5f;
    [SerializeField] bool faceTarget = true;

    [Header("Sound")]
    [SerializeField] AudioSource m_audioPlayer;
    [SerializeField] AudioClip damagedSound;
    [SerializeField] float damagedSoundVolume = 0.65f;
    [SerializeField] float damagedPitchMinimum = 0.85f;
    [SerializeField] float damagedPitchMaximum = 1.15f;

    void Awake()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        collider2D = this.GetComponent<Collider2D>();
        damagedSound = Resources.Load("Audio/SFX/Enemy/EnemyHitGrunt") as AudioClip;
        m_audioPlayer = this.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(collider2D, weapon.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (stun > 0) stun -= Time.deltaTime;

            else
            {
                // Sight Cone
                if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) < range &&
                    Vector3.Angle(PlayerController.instance.transform.position - transform.position, EnemyTorso.transform.up) < viewAngle)
                {
                    EnemyTorso.transform.up = (Vector2)(PlayerController.instance.transform.position - transform.position);
                    rigidbody2D.velocity = EnemyTorso.transform.up.normalized * speed;
                    if (rigidbody2D.velocity != Vector2.zero)
                    {
                        EnemyLegs.transform.up = rigidbody2D.velocity;
                        if (faceTarget && Quaternion.Angle(EnemyTorso.transform.rotation, EnemyLegs.transform.rotation) > 90) EnemyLegs.transform.up = -1 * EnemyLegs.transform.up; // Keeps body facing mouse
                    }
                }

                else
                {
                    rigidbody2D.velocity = Vector2.zero;
                }
            }
        }
        //DEBUG
        //Debug.Log("Distance is " + Vector3.Distance(PlayerController.instance.transform.position, transform.position));
        //Debug.Log("Angle is " + Vector3.Angle(PlayerController.instance.transform.position - transform.position, EnemyTorso.transform.up));
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, viewAngle) * EnemyTorso.transform.up).normalized * range, Color.yellow, .01f);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, -viewAngle) * EnemyTorso.transform.up).normalized * range, Color.yellow, .01f);
        Debug.DrawRay(transform.position, EnemyTorso.transform.up, Color.red, .01f);
        Debug.DrawRay(transform.position, EnemyLegs.transform.up, Color.green, .01f);
    }

    public void Damage(float damage, float stun, Vector2 knockback)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(knockback);
        health -= damage;
        this.stun = stun;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        StartCoroutine(playSoundCoroutine(damagedPitchMinimum, damagedPitchMaximum));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            weapon.Attack();
        }
    }

    private IEnumerator playSoundCoroutine(float minimumPitch, float maximumPitch)
    {
        float timePassed = 0.0f;
        m_audioPlayer.pitch = Random.Range(damagedPitchMinimum, damagedPitchMaximum);
        m_audioPlayer.PlayOneShot(damagedSound, damagedSoundVolume);

        while (timePassed < damagedSound.length)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        m_audioPlayer.pitch = 1.0f;
    }

}
