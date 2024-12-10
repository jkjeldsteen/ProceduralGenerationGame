using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Enemy : EnemyBehaviour
{

    public void Awake()
    {
        this.level = 1;
        this.anim = gameObject.GetComponent<Animator>();
        this.dungeonGeneration = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<DungeonGenerator>();
        this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.sr = gameObject.GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        base.OnStart();
    }

    public void Update()
    {
        base.OnUpdate();
    }
    public override IEnumerator EnemyTriggered(Vector2 targetPos)
    {
        //Does nothing but has to be implemented from superclass
        yield return new WaitForSeconds(0);
    }

    public override void Attack(GameObject attackableGameobject)
    {
        anim.SetBool("isAttacking", true);
        anim.SetBool("isHit", false);
    }
}
