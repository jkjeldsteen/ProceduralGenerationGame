using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetection : MonoBehaviour
{
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            //collision.gameObject.GetComponentInParent<TestMoveObject>().speed = 0;
            collision.gameObject.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}
