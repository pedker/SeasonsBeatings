using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flying : MonoBehaviour
{
    [SerializeField] float speed = 0.0f;

    private void Update()
    {
        transform.Translate((Vector2.right) * speed * Time.deltaTime);
    }
}
