using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static PlayerController instance;
    [SerializeField] GameObject playerTorso;
    [SerializeField] GameObject playerLegs;
    [SerializeField] Weapon weapon;
    Rigidbody2D rigidbody2D;
    Collider2D collider2D; 

    [SerializeField] float speed = 5f;
    [SerializeField] float health = 100f;
    [SerializeField] float stun = 0;
    [SerializeField] bool faceTarget = true;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (stun > 0) stun -= Time.deltaTime;

            else
            {

                playerTorso.transform.up = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);

                rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
                if (rigidbody2D.velocity != Vector2.zero)
                {
                    playerLegs.transform.up = rigidbody2D.velocity;
                    if (faceTarget && Quaternion.Angle(playerTorso.transform.rotation, playerLegs.transform.rotation) > 90) playerLegs.transform.up = -1 * playerLegs.transform.up; // Keeps body facing mouse
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
        this.stun = stun;

        if (health <= 0)
        {
            SceneManager.LoadScene(0); //Restart Game
        }
    }
}
