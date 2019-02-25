using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject EnemyTorso;
    [SerializeField] GameObject EnemyLegs;
    Rigidbody2D m_rigidbody2D;

    [SerializeField] float range = 10f;
    [SerializeField] float viewAngle = 45f;
  
    [SerializeField] float speed = 5f;
    [SerializeField] bool faceTarget = true;

    void Awake()
    {
        m_rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // Sight Cone
        if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) < range &&
            Vector3.Angle(PlayerController.instance.transform.position - transform.position, EnemyTorso.transform.up) < viewAngle)
        {
            EnemyTorso.transform.up = (Vector2)(PlayerController.instance.transform.position - transform.position);
            m_rigidbody2D.velocity = EnemyTorso.transform.up.normalized * speed;
            if (m_rigidbody2D.velocity != Vector2.zero)
            {
                EnemyLegs.transform.up = m_rigidbody2D.velocity;
                if (faceTarget && Quaternion.Angle(EnemyTorso.transform.rotation, EnemyLegs.transform.rotation) > 90) EnemyLegs.transform.up = -1 * EnemyLegs.transform.up; // Keeps body facing mouse
            }
        }

        else
        {
            m_rigidbody2D.velocity = Vector2.zero;
        }

        //DEBUG
        //Debug.Log("Distance is " + Vector3.Distance(PlayerController.instance.transform.position, transform.position));
        //Debug.Log("Angle is " + Vector3.Angle(PlayerController.instance.transform.position - transform.position, EnemyTorso.transform.up));
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, viewAngle) * EnemyTorso.transform.up).normalized * range, Color.yellow, .01f);
        Debug.DrawRay(transform.position, (Quaternion.Euler(0, 0, -viewAngle) * EnemyTorso.transform.up).normalized * range, Color.yellow, .01f);
        Debug.DrawRay(transform.position, EnemyTorso.transform.up, Color.red, .01f);
        Debug.DrawRay(transform.position, EnemyLegs.transform.up, Color.green, .01f); 
    }
}
