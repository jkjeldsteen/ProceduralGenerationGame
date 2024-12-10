using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveObject : MonoBehaviour
{
    public float speed;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 1;
        float targetX = 1;
        float targetY = 1;
        Vector3 bombTargetPos = new Vector3(this.transform.position.x + targetX, this.transform.position.y + targetY, 0);
        Vector2 bombDirection = (this.transform.position - bombTargetPos).normalized;
        rb.AddForce(bombDirection * 1, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
