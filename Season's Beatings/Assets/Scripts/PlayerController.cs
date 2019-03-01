using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController instance;
    [SerializeField] GameObject playerTorso;
    [SerializeField] GameObject playerLegs;
    [SerializeField] Weapon weapon;
    Rigidbody2D rigidbody2D;
    Collider2D collider2D; 

    [SerializeField] float speed = 5f;
    [SerializeField] float stun = 0;
    [SerializeField] bool faceTarget = true;

    public Animator animator = null;

    [Header("HealthUI")]
    [SerializeField] float health = 100f;
    public Slider slider;
    public Image fillImage;
    public Color FullHealthColor;
    public Color ZeroHealthColor;


    void Awake()
    {
        instance = this;
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        collider2D = this.GetComponent<Collider2D>();
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

                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
                {
                    //Debug.Log("Click");
                    weapon.Attack();
                }
            }
        }
        //DEBUG
        Debug.DrawRay(transform.position, playerTorso.transform.up, Color.red, .01f);
        Debug.DrawRay(transform.position, playerLegs.transform.up, Color.green, .01f); 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        I_item itemComponent = collision.collider.GetComponent<I_item>();
        if (itemComponent != null)
        {
            itemComponent.pickUp();
        }
    }

    public void Damage(float damage, float stun, Vector2 knockback)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.AddForce(knockback);
        health -= damage;
        SetHealthUI();
        this.stun = stun;

        if (health <= 0)
        {
            SceneManager.LoadScene(0); //Restart Game
        }
    }

    private void SetHealthUI()
    {
        slider.value = health;

        fillImage.color = Color.Lerp(ZeroHealthColor, FullHealthColor, health / 100); // 100 is the hardcoded starting health, might need to change later
    }
}
