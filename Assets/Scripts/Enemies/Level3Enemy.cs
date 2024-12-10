using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Enemy : EnemyBehaviour
{
    [SerializeField] private GameObject bomb;
    private float timeBtwBomb;


    // Start is called before the first frame update
    void Start()
    {
        this.level = 3;
        this.anim = gameObject.GetComponent<Animator>();
        this.dungeonGeneration = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<DungeonGenerator>();
        this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.sr = gameObject.GetComponent<SpriteRenderer>();
        this.healthScale = this.transform.localScale;
        base.OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        base.OnUpdate();
        if (timeBtwBomb > 0)
        {
            timeBtwBomb -= Time.deltaTime;
        }
        if(this.health <= 0)
        {
            this.anim.SetBool("isBlowing", false);
            this.anim.SetBool("isAttacking", false);
            this.anim.SetBool("isHit", false);
        }
    }

    public override IEnumerator EnemyTriggered(Vector2 targetPos)
    {
        if (timeBtwBomb <= 0)
        {
            this.anim.SetBool("isBlowing", true);
            this.anim.SetBool("isAttacking", false);
            this.anim.SetBool("isHit", false);
            
            timeBtwBomb = 2;
        }
        yield return null;
    }

    public override void Attack(GameObject attackableGameobject)
    {
    }

    private void SpitBomb()
    {
        GameObject newBomb = Instantiate(bomb, transform.position, Quaternion.identity);
        float targetX = Random.Range(-2, 2);
        float targetY = Random.Range(-2, 2);
        Vector3 bombTargetPos = new Vector3(this.transform.position.x + targetX, this.transform.position.y + targetY, 0);
        Vector2 bombDirection = (this.transform.position - bombTargetPos).normalized;
        newBomb.GetComponent<Rigidbody2D>().AddForce(bombDirection * 2, ForceMode2D.Impulse);
    }

    private void SetBlowFalse()
    {
        this.anim.SetBool("isBlowing", false);
    }
}
