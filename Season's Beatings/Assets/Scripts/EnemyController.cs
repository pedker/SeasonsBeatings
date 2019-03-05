using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] float stun = 0;
    [SerializeField] float viewAngle = 45f;
    [SerializeField] float speed = 5f;
    [SerializeField] bool faceTarget = true;
    [SerializeField] float health = 100f;

    public Animator animator = null;

    [Header("HealthUI")]
    public Slider slider;
    public Image fillImage;
    public Color FullHealthColor;
    public Color ZeroHealthColor;

    [Header("AITweaks")]
    public float movementChance = .5f;
    public float actionTime = 3;
    float timeElapsed;
    bool acting = false;
    bool followingPlayer = false;

    [Header("Sound")]

    [SerializeField] string footStepFileName;
    [SerializeField] float footStepVolume = 0.30f;
    [SerializeField] float footStepPitchMin = 0.9f;
    [SerializeField] float footStepPitchMax = 1.1f;

    [SerializeField] string enemyDamagedFileName;
    [SerializeField] float damagedSoundVolume = 0.65f;
    [SerializeField] float damagedPitchMinimum = 0.85f;
    [SerializeField] float damagedPitchMaximum = 1.15f;
    
    AudioPlayer m_audioPlayer;

    void Awake()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        collider2D = this.GetComponent<Collider2D>();
        m_audioPlayer = GetComponentInChildren<AudioPlayer>();
        m_audioPlayer.addSFX(enemyDamagedFileName);
        m_audioPlayer.addSFX(footStepFileName);
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(collider2D, weapon.GetComponent<Collider2D>());
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

            Vector2 vectorToPlayer = PlayerController.instance.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, vectorToPlayer);
            Debug.Log(hit.collider.name);

            if (stun > 0) stun -= Time.deltaTime;
            else
            {
                
                // Sight Cone
                if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) < range &&
                    Vector3.Angle(vectorToPlayer, EnemyTorso.transform.right) < viewAngle &&
                    hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        acting = false;
                        followingPlayer = true;
                        EnemyTorso.transform.right = (Vector2)(vectorToPlayer);

                        // Attack Range
                        hit = Physics2D.Raycast(transform.position, vectorToPlayer, weapon.GetComponent<SpriteRenderer>().bounds.size.y);                        
                        if (hit.collider != null && hit.collider.CompareTag("Player"))
                        {
                            weapon.Attack();
                            rigidbody2D.velocity = Vector2.zero;
                        }
                        else
                        {
                            rigidbody2D.velocity = EnemyTorso.transform.right.normalized * speed;
                        }

                        if (rigidbody2D.velocity != Vector2.zero)
                        {
                            EnemyLegs.transform.right = rigidbody2D.velocity;
                            if (faceTarget && Quaternion.Angle(EnemyTorso.transform.rotation, EnemyLegs.transform.rotation) > 90) EnemyLegs.transform.right = -1 * EnemyLegs.transform.right; // Keeps body facing mouse
                        }
                    }
                }
                else
                {
                    if (followingPlayer)
                    {
                        acting = true;
                        followingPlayer = false;
                        timeElapsed = 0;
                        rigidbody2D.velocity = Vector2.zero;
                    }
                    else if (acting == false)
                    {
                        float action = Random.value;
                        acting = true;
                        timeElapsed = 0;
                        if (action < movementChance)
                        {
                            EnemyLegs.transform.right = EnemyTorso.transform.right = Random.insideUnitCircle;
                            rigidbody2D.velocity = EnemyTorso.transform.right.normalized * speed;
                        }
                        else
                        {
                            rigidbody2D.velocity = Vector2.zero;
                        }
                    }
                    else if (timeElapsed < actionTime)
                    {
                        timeElapsed += Time.deltaTime;
                    }
                    else
                    {
                        acting = false;
                    }
                }
            }
        }
        //DEBUG
        //Debug.Log("Distance is " + Vector3.Distance(PlayerController.instance.transform.position, transform.position));
        //Debug.Log("Angle is " + Vector3.Angle(PlayerController.instance.transform.position - transform.position, EnemyTorso.transform.up));
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, viewAngle) * EnemyTorso.transform.right).normalized * range, Color.yellow, .01f);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, -viewAngle) * EnemyTorso.transform.right).normalized * range, Color.yellow, .01f);
        Debug.DrawRay(transform.position, EnemyTorso.transform.right.normalized * weapon.GetComponent<SpriteRenderer>().bounds.size.y, Color.red, .01f);
        //Debug.DrawRay(transform.position, EnemyLegs.transform.right, Color.green, .01f);
    }

    public void Damage(float damage, float stun, Vector2 knockback)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(knockback);

        // Face Attacker
        EnemyLegs.transform.right = -knockback;
        EnemyTorso.transform.right = -knockback;

        health -= damage;
        SetHealthUI();
        this.stun = stun;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        m_audioPlayer.playSFX(enemyDamagedFileName, damagedSoundVolume, damagedPitchMinimum, damagedPitchMaximum);
    }

    private void SetHealthUI()
    {
        slider.value = health;

        fillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, health / 100); // 100 is the hardcoded starting health, might need to change later
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Wall"))
        {
            rigidbody2D.velocity = Vector2.zero;
        }

        if (collision.collider.CompareTag("Player"))
        {
            Vector2 vectorToPlayer = PlayerController.instance.transform.position - transform.position;

            EnemyLegs.transform.right = vectorToPlayer;
            EnemyTorso.transform.right = vectorToPlayer;
        }
    }

    private void playFootStepSFX()
    {
        m_audioPlayer.playSFX(footStepFileName, footStepVolume, footStepPitchMin, footStepPitchMax);
    }
}
