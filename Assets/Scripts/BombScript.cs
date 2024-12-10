using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private void Start()
    {
    }

    void DisableCollision()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(1.3f, 1.3f);
        collider.isTrigger = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
