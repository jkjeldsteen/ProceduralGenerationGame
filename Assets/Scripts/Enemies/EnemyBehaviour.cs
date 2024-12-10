using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] protected DungeonGenerator dungeonGeneration;
    [SerializeField] protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    private float timeBtwDamage;
    protected bool canBeDamaged;
    protected float timeBtwMove;
    public int level;
    private float minY;
    private float maxY;
    private float minX;
    private float maxX;
    private float originalX;
    private float originalrbX;
    private float originalY;
    private float originalrbY;
    protected float maxChaseX;
    protected float minChaseX;
    protected float maxChaseY;
    protected float minChaseY;
    protected float speed;

    public Vector2 targetPos;
    public Animator anim;

    //Health
    [SerializeField] protected GameObject healthBar;
    [SerializeField] private GameObject healthBarBG;
    protected Vector3 healthScale;
    private Vector3 healthOffset;
    private float healthDisplay;
    public float health;
    public float startHealth;

    public void OnStart()
    {
        rb = this.GetComponent<Rigidbody2D>();
        speed = 1.5f;
        minY = this.transform.position.y - 3.5f;
        maxY = this.transform.position.y + 3.5f;
        minX = this.transform.position.x - 3.5f;
        maxY = this.transform.position.x + 3.5f;
        originalX = this.transform.position.x;
        originalrbX = rb.position.x;
        originalY = this.transform.position.y;
        originalrbY = rb.position.y;
        maxChaseX = this.transform.position.x + 5;
        minChaseX = this.transform.position.x - 5;
        maxChaseY = this.transform.position.y + 5;
        minChaseY = this.transform.position.y - 5;
        timeBtwMove = 0;
        timeBtwDamage = 0;
        canBeDamaged = true;

        //Setting health values at the start
        healthScale.x = 1f;
        healthScale.y = 1f;
        healthScale.z = 1f;
        health = 10 * level;
        startHealth = health;
        healthDisplay = 100;
    }

    public void FixedUpdate()
    {
        Move();
    }

    public void OnUpdate()
    {
        if(timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
        }
        HealthBar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StopMoving();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            int damageTaken = collision.gameObject.GetComponent<SwordBehaviour>().damage;
            DamagedByPlayer(damageTaken);
        }
    }

    private void HealthBar()
    {
        if(health < 0)
        {
            health = 0;
        }
        healthDisplay = (100 / startHealth) * health;
        healthScale.x = healthDisplay / 100;
        this.healthBar.transform.localScale = healthScale;
    }

    public void DestroyEnemy()
    {
        dungeonGeneration.enemyCount--;
        speed = 0;
        Destroy(gameObject);
    }

    private void Move()
    {  
        float newY;
        float newX;
        if (timeBtwMove <= 0)
        {
            speed = 1.5f;
            newY = Random.Range(-2, 2);
            newX = Random.Range(-2, 2);
            if (rb.position.y + newY > maxY || rb.position.y + newY < minY)
            {
                newY *= -1;
            }
            if (rb.position.x + newX > maxX || rb.position.x + newX < minX)
            {
                newX *= -1;
            }
            targetPos = new Vector2(originalX + newX, originalY + newY);
            timeBtwMove = 3;
            Vector2 direction = (targetPos - new Vector2(transform.position.x, transform.position.y)).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Impulse);
        }
        if (timeBtwMove > 0)
        {
            timeBtwMove -= Time.fixedDeltaTime;
        }


        if (transform.position.x != targetPos.x || transform.position.y != targetPos.y)
        {
            if (anim.GetBool("isRunning") == false || anim.GetBool("isIdle") == true)
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isIdle", false);
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isIdle", true);
        }

        if (targetPos.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else if (targetPos.x < transform.position.x)
        {
            sr.flipX = true;
        }
    }

    private void MoveEnemy()
    {
        float newY;
        float newX;
        if(timeBtwMove <= 0)
        {
            speed = 1.5f;
            newY = Random.Range(-2, 2);
            newX = Random.Range(-2, 2);
            if (transform.position.y + newY > maxY || transform.position.y + newY < minY)
            {
                newY *= -1;
            }
            if (transform.position.x + newX > maxX || transform.position.x + newX < minX)
            {
                newX *= -1;
            }
            targetPos = new Vector2(originalX + newX, originalY + newY);
            timeBtwMove = 3;
        }
        if(timeBtwMove > 0)
        {
            timeBtwMove -= Time.deltaTime;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        
        if(transform.position.x != targetPos.x || transform.position.y != targetPos.y)
        {
            if(anim.GetBool("isRunning") == false || anim.GetBool("isIdle") == true)
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isIdle", false);
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isIdle", true);
        }

        if(targetPos.x > transform.position.x)
        {
            sr.flipX = false;
        }else if (targetPos.x < transform.position.x)
        {
            sr.flipX = true;
        }
    }

    private void AttackAni()
    {
        anim.SetBool("isAttacking", false);
    }

    public abstract void Attack(GameObject attackableGameobject);

    public void DamagedByPlayer(float damage)
    {
        if (timeBtwDamage <= 0 && canBeDamaged == true)
        {
            health -= damage;
            if (health <= 0)
            {
                anim.SetBool("isHit", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isDead", true);
                speed = 0;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                this.transform.GetChild(2).gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
            }
            else
            {
                anim.SetBool("isHit", true);
                anim.SetBool("isAttacking", false);
            }
            timeBtwDamage = 0.5f;
            anim.SetBool("isAttacking", false);
        }
    }

    public void StopMoving()
    {
        rb.velocity = new Vector2(0, 0);
    }

    private void EndHit()
    {
        anim.SetBool("isHit", false);
    }

    public abstract IEnumerator EnemyTriggered(Vector2 targetPos);
}
