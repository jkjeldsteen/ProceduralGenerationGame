using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    //Standard info
    public float playerHealth = 100;
    public float moveSpeed = 5;
    public bool canMove;
    public Rigidbody2D rb;
    private Vector2 moveInput;
    public float timeBtwDamage;

    //Dash info
    public bool dashUnlocked = false;
    public float activeMoveSpeed;
    private float dashSpeed = 6f;
    private bool isDashing = false;
    private float dashLength = 0.5f;
    private float dashCooldown = 1f;
    private float dashCounter;
    public float dashCoolCounter;
    //Trailrenderer for dash
    [SerializeField] private TrailRenderer tr;

    //Inventory count for buffs/potions
    public int healthPots;
    [SerializeField] private ParticleSystem healthUpParticles;
    private int maxHealthPots;
    public float timeBtwHealthPot;
    [SerializeField] private KeyHolder keyInventory;

    //Animation info
    private Animator anim;
    private bool isMoving;
    private Vector2 lastMoveDir;

    //Sword
    [SerializeField] private GameObject swordCollider;
    private float swordCooldown;

    //Camera shake info
    public CameraHandling cameraHandling;

    //Healthbar
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject healthBarBG;
    private float healthScaleX;
    private Vector3 healthScale;
    private Vector3 healthOffset;

    private bool canBeBombed;

    // Start is called before the first frame update
    void Start()
    {
        cameraHandling = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandling>();
        playerHealth = 100;
        activeMoveSpeed = moveSpeed;
        anim = GetComponent<Animator>();
        //healthScale = transform.localScale;
        healthScale.x = healthBar.transform.localScale.x;
        healthScale.y = healthBar.transform.localScale.y;
        healthScale.z = healthBar.transform.localScale.z;
        healthScaleX = healthScale.x;
        healthOffset = new Vector3(-0.07f * transform.localScale.x, 0.19f * transform.localScale.y, 0f);
        canBeBombed = true;
        timeBtwDamage = 1f;
        healthPots = 0;
        maxHealthPots = 10;
        timeBtwHealthPot = 0;
        swordCooldown = 0;
        swordCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
        }
        DontDestroyOnLoad(this.gameObject);
        if(canMove)
        {
            Movement();
        }
        HealthBar();
        PlayerAnimation();
        InventoryInputs();
    }

    void Movement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        
        rb.velocity = moveInput * activeMoveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && dashUnlocked == true)
        {
            if (dashCoolCounter >= 1 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                isDashing = true;
            }
        }
        if (dashCounter > 0)
        {
            tr.emitting = true;
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                tr.emitting = false;
                isDashing = false;
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = 0;
            }
        }
        if (dashCoolCounter < dashCooldown)
        {
            dashCoolCounter += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(swordCooldown <= 0)
            {
                anim.SetBool("isAttacking", true);
                swordCooldown = 1;
            }
        }
        if(swordCooldown > 0)
        {
            swordCooldown -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour enemyBehaviour = collision.gameObject.GetComponentInParent<EnemyBehaviour>();
            if (isDashing)
            {
                //TODO: Damage hardcoded to be 10 at dash. Make different attack types with different damage values
                enemyBehaviour.DamagedByPlayer(10);

                StartCoroutine(cameraHandling.Shake(0.15f, 0.1f));
            }
            else
            {
                if (playerHealth > 0)
                {
                    if(timeBtwDamage <= 0)
                    {
                        playerHealth -= 10;
                        StartCoroutine(Shake(0.1f, 0.03f));
                        enemyBehaviour.Attack(this.gameObject);
                        enemyBehaviour.StopMoving();
                        timeBtwDamage = 1f;
                    }
                }
                if (playerHealth <= 0)
                {
                    this.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("isStunned", false);
                    this.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("isDead", true);
                    anim.SetBool("isDead", true);
                }
            }
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            EnemyBehaviour enemyBehaviour = collision.gameObject.GetComponentInParent<EnemyBehaviour>();
            if (isDashing)
            {
                enemyBehaviour.DamagedByPlayer(enemyBehaviour.startHealth * 0.0625f);

                StartCoroutine(cameraHandling.Shake(0.15f, 0.1f));
            }
            else
            {
                if (playerHealth > 0)
                {
                    if (timeBtwDamage <= 0)
                    {
                        playerHealth -= 10;
                        StartCoroutine(Shake(0.1f, 0.03f));
                        enemyBehaviour.Attack(this.gameObject);
                        enemyBehaviour.StopMoving();
                        timeBtwDamage = 1f;
                    }
                }
                if (playerHealth <= 0)
                {
                    this.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("isStunned", false);
                    this.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("isDead", true);
                    anim.SetBool("isDead", true);
                }
            }
        }
        if (collision.gameObject.CompareTag("Pickup"))
        {
            if (healthPots < maxHealthPots)
            {
                Destroy(collision.gameObject);
                healthPots++;
            }
        }
        if (collision.gameObject.CompareTag("Key"))
        {
            keyInventory.keys.Add(new KeyType(collision.gameObject.GetComponent<Key>().GetKeyType(), collision.gameObject.GetComponent<Key>().GetKeyObject()));
            Destroy(collision.gameObject);
            keyInventory.UpdateKeys();
        }
        if (collision.gameObject.CompareTag("LockedDoor"))
        {
            string doortype = collision.gameObject.GetComponent<LockedDoor>().GetLockType();
            if (keyInventory.CompareKeyType(doortype))
            {
                Destroy(collision.gameObject);
                keyInventory.RemoveKey(doortype);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.CompareTag("Ability"))
        {
            dashUnlocked = true;
            Destroy(trigger.gameObject);
        }

        if(trigger.gameObject.CompareTag("EndFloor"))
        {
            Gamehandler gamehandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gamehandler>();
            gamehandler.NextFloor();
            canMove = false;
        }
        if (trigger.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(trigger.gameObject.GetComponentInParent<EnemyBehaviour>().EnemyTriggered(this.transform.position));
        }
    }

    private void OnTriggerStay2D(Collider2D triggerStay)
    {
        if(triggerStay.gameObject.CompareTag("BombTrigger"))
        {
            StartCoroutine(triggerStay.gameObject.GetComponentInParent<EnemyBehaviour>().EnemyTriggered(this.transform.position));
        }
        if (triggerStay.gameObject.CompareTag("Bomb"))
        {
            if (canBeBombed)
            {
                StartCoroutine(BombDamage());
                if (playerHealth <= 0)
                {
                    this.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("isStunned", false);
                    this.transform.GetChild(2).GetComponentInChildren<Animator>().SetTrigger("isDead");
                    anim.SetTrigger("IsDead");
                }
            }
        }
    }
    void PlayerAnimation()
    {
        if (moveInput.magnitude > 0)
        {
            isMoving = true;
            lastMoveDir = moveInput;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            anim.SetFloat("Horizontal", moveInput.x);
            anim.SetFloat("Vertical", moveInput.y);
            anim.SetFloat("Magnitude", moveInput.magnitude);
        } else if (!isMoving)
        {
            anim.SetFloat("Magnitude", moveInput.magnitude);
        }
        anim.SetBool("isDashing", isDashing);

    }
    void HealthBar()
    {
        //healthBarBG.transform.position = new Vector3(healthOffset.x + this.transform.position.x, healthOffset.y + this.transform.position.y, healthOffset.z);
        //healthBar.transform.position = new Vector3(healthOffset.x + this.transform.position.x, healthOffset.y + this.transform.position.y, healthOffset.z);
        healthScale.x = (playerHealth / 100) * healthScaleX;
        healthBar.transform.localScale = healthScale;
    }
    private void InventoryInputs()
    {
        //Healthpot
        if(Input.GetKeyDown(KeyCode.Alpha1) && healthPots > 0 && timeBtwHealthPot <= 0  && playerHealth < 100)
        {
            healthPots--;
            Instantiate(healthUpParticles, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);
            playerHealth += 10;
            timeBtwHealthPot = 2;
            if(playerHealth > 100)
            {
                playerHealth = 100;
            }
        }
        else if(timeBtwHealthPot > 0)
        {
            timeBtwHealthPot -= Time.deltaTime;
        }
    }
    void MessageDeathToGameHandler()
    {
        Gamehandler gamehandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gamehandler>();
        gamehandler.PlayerDead();
    }
    public IEnumerator Shake(float duration, float magnitude)
    {

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            healthOffset.x += x;
            healthOffset.y += y;

            elapsed += Time.deltaTime;

            yield return null;
        }

        healthOffset.x = -0.07f * transform.localScale.x;
        healthOffset.y = 0.19f * transform.localScale.y;
    }
    public IEnumerator Knockback(float knockbackDuration, float knockbackPower, Transform enemy)
    {
        float timer = 0;
        rb.velocity = new Vector2(0, 0);
        canMove = false;
        StartCoroutine(cameraHandling.Shake(0.15f, 0.1f));
        this.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("isStunned", true);
        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = (this.transform.position - enemy.transform.position).normalized;
            rb.AddForce(direction * knockbackPower, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(1);
        this.transform.GetChild(2).GetComponentInChildren<Animator>().SetBool("isStunned", false);
        canMove = true;
        rb.velocity = new Vector2(0, 0);
    }
    private IEnumerator BombDamage()
    {
        playerHealth -= 20;
        canBeBombed = false;
        yield return new WaitForSeconds(1);
        canBeBombed = true;
    }

    private void ActivateSwordCollider()
    {
        if(lastMoveDir.x > 0)
        {
            swordCollider.transform.position = new Vector2(this.transform.position.x+0.1f*2, this.transform.position.y+0f*2);
        }
        else if (lastMoveDir.x < 0)
        {
            swordCollider.transform.position = new Vector2(this.transform.position.x - 0.1f*2, this.transform.position.y + 0f*2);
        }
        else if (lastMoveDir.y > 0)
        {
            swordCollider.transform.position = new Vector2(this.transform.position.x + 0f*2, this.transform.position.y + 0.1f*2);
        }
        if (lastMoveDir.y < 0)
        {
            swordCollider.transform.position = new Vector2(this.transform.position.x + 0f*2, this.transform.position.y - 0.1f*2);
        }
        swordCollider.SetActive(true);
    }

    private void DeactivateSwordCollider()
    {
        swordCollider.SetActive(false);
        anim.SetBool("isAttacking", false);
    }

}
