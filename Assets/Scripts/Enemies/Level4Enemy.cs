using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Enemy : EnemyBehaviour
{

    void Start()
    {
        this.level = 4;
        this.anim = gameObject.GetComponent<Animator>();
        this.dungeonGeneration = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<DungeonGenerator>();
        //this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.sr = gameObject.GetComponent<SpriteRenderer>();
        this.healthScale = this.transform.localScale;
        base.OnStart();
    }

    void Update()
    {
        base.OnUpdate();
    }

    public override void Attack(GameObject attackableGameobject)
    {
        anim.SetBool("isAttacking", true);
        float strength = 0.5f;
        float knockbackDuration = 0.1f;
        if (attackableGameobject.transform.position.x > this.transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        StartCoroutine(attackableGameobject.GetComponent<PlayerController>().Knockback(knockbackDuration, strength, this.transform));
    }

    public override IEnumerator EnemyTriggered(Vector2 targetPos)
    {
       yield return null;
    }
}
