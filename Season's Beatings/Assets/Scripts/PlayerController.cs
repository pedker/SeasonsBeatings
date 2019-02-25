using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] GameObject PlayerTorso;
    [SerializeField] GameObject PlayerLegs;
    Rigidbody2D m_rigidbody2D;

    [SerializeField] float speed = 5f;
    [SerializeField] bool faceTarget = true;

    void Awake()
    {
        instance = this;
        m_rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTorso.transform.up = (Vector2) (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        m_rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        if (m_rigidbody2D.velocity != Vector2.zero)
        {
            PlayerLegs.transform.up = m_rigidbody2D.velocity;
            if (faceTarget && Quaternion.Angle(PlayerTorso.transform.rotation, PlayerLegs.transform.rotation) > 90) PlayerLegs.transform.up = -1 * PlayerLegs.transform.up; // Keeps body facing mouse
        }

        //DEBUG
        Debug.DrawRay(transform.position, PlayerTorso.transform.up, Color.red, .01f);
        Debug.DrawRay(transform.position, PlayerLegs.transform.up, Color.green, .01f); 
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        I_item itemComponent = collision.collider.GetComponent<I_item>();
        if (itemComponent != null)
        {
            itemComponent.pickUp();
        }
    }
}
