using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Enemy : EnemyBehaviour
{
    //Level 2 Enemy Charge attack
    private bool isCharging;
    private float chargeTime;
    private float timeBtwCharge;

    void Start()
    {
        this.level = 2;
        this.anim = gameObject.GetComponent<Animator>();
        this.dungeonGeneration = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<DungeonGenerator>();
        this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.sr = gameObject.GetComponent<SpriteRenderer>();
        this.healthScale = this.transform.localScale;
        base.OnStart();
    }

    void Update()
    {
        base.OnUpdate();
        if (timeBtwCharge > 0)
        {
            timeBtwCharge -= Time.deltaTime;
        }
    }

    private void Level2EnemyCharge()
    {
        if (level == 2)
        {
            if (chargeTime > 0)
            {
                isCharging = true;
                chargeTime -= Time.deltaTime;
            }
            else if (chargeTime <= 0)
            {
                isCharging = false;
            }
            if (isCharging)
            {
                this.gameObject.GetComponent<TrailRenderer>().emitting = true;
            }
            else if (!isCharging)
            {
                this.gameObject.GetComponent<TrailRenderer>().emitting = false;
            }
        }
    }

    public override IEnumerator EnemyTriggered(Vector2 targetPos)
    {
        if (transform.position.x < maxChaseX - 1 || transform.position.x > minChaseX + 1)
        {
            if (transform.position.y < maxChaseY - 1 || transform.position.y > minChaseY + 1)
            {
                this.transform.GetChild(3).GetComponentInChildren<Animator>().SetBool("isCharging", true);
                if (timeBtwCharge <= 0)
                {
                    speed = 2.5f;
                    this.targetPos = targetPos;
                    timeBtwMove = 2;
                    timeBtwCharge = 3;
                    chargeTime = 1f;
                }
                yield return new WaitForSeconds(chargeTime);
                if(this != null)
                {
                    this.transform.GetChild(3).GetComponentInChildren<Animator>().SetBool("isCharging", false);
                }
                else if (this == null)
                {
                    yield return null;
                }
            }
        }
        
    }

    public override void Attack(GameObject attackableGameobject)
    {
    }
}
