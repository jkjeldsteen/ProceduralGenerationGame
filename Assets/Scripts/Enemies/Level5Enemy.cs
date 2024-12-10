using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Enemy : EnemyBehaviour
{
    [SerializeField] private GameObject bandit;
    [SerializeField] private GameObject bomber;
    [SerializeField] private GameObject treeleg;
    [SerializeField] private GameObject whale;
    private Vector3 startingPos;

    private bool waveTwoStarted;
    private bool waveTwoDone;
    private bool waveThreeStarted;
    private bool waveThreeDone;
    private bool waveFourStarted;
    private bool waveFourDone;
    [SerializeField] private List<GameObject> waveTwoEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> waveThreeEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> waveFourEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> supportOne = new List<GameObject>();
    [SerializeField] private List<GameObject> supportTwo = new List<GameObject>();
    [SerializeField] private List<GameObject> supportThree = new List<GameObject>();
    void Start()
    {
        base.OnStart();
        this.level = 5;
        this.anim = gameObject.GetComponent<Animator>();
        this.dungeonGeneration = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<DungeonGenerator>();
        this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.sr = gameObject.GetComponent<SpriteRenderer>();
        this.healthScale = this.transform.localScale;
        this.health = 150f;
        this.startHealth = this.health;
        startingPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        waveTwoStarted = false;
        waveThreeStarted = false;
        waveFourStarted = false;
        WaveOne();
    }
    void Update()
    {
        base.OnUpdate();
        //Hvis bossen er over 75% hp checkes der hele tiden i listen af supportOne og de fjernes når de dør.
        if(health >= startHealth * 0.75)
        {
            for (int i = 0; i < supportOne.Count; i++)
            {
                if(supportOne[i].gameObject == null)
                {
                    supportOne.RemoveAt(i);
                }
            }
        }
        //Hvis bossen er på mellem 50-75% hp
        if (health <= startHealth * 0.75f && health >= startHealth * 0.5f)
        {
            //Er alle fra supportone død? If so, start wave to
            if (supportOne.Count == 0)
            {
                WaveTwo();
            }
            //Ellers vent til alle fra supportone dør og sørg for bossen ikke tager skade
            else
            {
                this.canBeDamaged = false;
            }
            //Imens bossen har dette hp checkes support to hele tiden for at fjerne dem fra listen når de dør
            for (int i = 0; i < supportTwo.Count; i++)
            {
                if(supportTwo[i].gameObject == null)
                {
                    supportTwo.RemoveAt(i);
                }
            }
        }
        //Hvis bossen er på mellem 25-50% hp
        if(health <= startHealth * 0.5f && health >= startHealth * 0.25f)
        {
            //Er alle fra supporttwo død? If so, start wave tre
            if (supportTwo.Count == 0)
            { 
                WaveThree();
            }
            //Ellers vent til alle fra supporttwo dør og sørg for bossen ikke tager skade
            else
            {
                this.canBeDamaged = false;
            }
            //Imens bossen har dette hp checkes support tre hele tiden for at fjerne dem fra listen når de dør
            for (int i = 0; i < supportThree.Count; i++)
            {
                if (supportThree[i].gameObject == null)
                {
                    supportThree.RemoveAt(i);
                }
            }
        }
        //Hvis bossen er på mellem 1-25% hp
        if(health <= startHealth * 0.25 && health >= startHealth * 0.01f)
        {
            //Er alle fra supportthree død? If so, start wave fire
            if (supportThree.Count == 0)
            {
                WaveFour();
            }
            //Ellers vent til de er døde og sørg for bossen ikke tager skade
            else
            {
                this.canBeDamaged = false;
            }
        }
        //
        if(health <= 0.00f)
        {
            //TODO: End the game/win scene
        }
    }
    public override void Attack(GameObject attackableGameobject)
    {
        anim.SetBool("isAttacking", true);
    }

    public override IEnumerator EnemyTriggered(Vector2 targetPos)
    {
        throw new System.NotImplementedException();
    }

    private void WaveOne()
    {
        Debug.Log("Wave One starts");
        //Spawn 2 bandits next to the boss (one on each side)
        //Add to "supportcrew 1 list"
        GameObject bandit1 = Instantiate(bandit, new Vector3(this.transform.position.x+4, this.transform.position.y), Quaternion.identity);
        GameObject bandit2 = Instantiate(bandit, new Vector3(this.transform.position.x-4, this.transform.position.y), Quaternion.identity);
        supportOne.Add(bandit1);
        supportOne.Add(bandit2);
        this.canBeDamaged = true;
    }

    private void WaveTwo()
    {
        if(waveTwoStarted == false)
        {
            this.canBeDamaged = false;
            //Spawn 2 lvl 2 enemies (tree leg) in opposite corners
            GameObject treeleg1 = Instantiate(treeleg, new Vector3(startingPos.x + 3, startingPos.y + 3, startingPos.z), Quaternion.identity);
            waveTwoEnemies.Add(treeleg1);
            GameObject treeleg2 = Instantiate(treeleg, new Vector3(startingPos.x - 3, startingPos.y - 3, startingPos.z), Quaternion.identity);
            waveTwoEnemies.Add(treeleg2);
            //Spawn 1 lvl 1 enemy (whale) in the middle
            GameObject whale1 = Instantiate(whale, startingPos, Quaternion.identity);
            waveTwoEnemies.Add(whale1);

            //TODO: Pathfinding for Boss to hide

            waveTwoStarted = true;
            Debug.Log("Wave two started");
        }
        for (int i = 0; i < waveTwoEnemies.Count; i++)
        {
            if (waveTwoEnemies[i].gameObject == null)
            {
                waveTwoEnemies.RemoveAt(i);
            }
        }
        if(waveTwoStarted && waveTwoEnemies.Count == 0 && !waveTwoDone)
        {
            //TODO: Release boss again

            //Spawn 2 lvl 2 enemies
            //Add to "supportcrew 2 list"
            GameObject treeleg1 = Instantiate(treeleg, new Vector3(startingPos.x + 3, startingPos.y, startingPos.z), Quaternion.identity);
            GameObject treeleg2 = Instantiate(treeleg, new Vector3(startingPos.x - 3, startingPos.y, startingPos.z), Quaternion.identity);
            supportTwo.Add(treeleg1);
            supportTwo.Add(treeleg2);
            this.canBeDamaged = true;
            waveTwoDone = true;
            Debug.Log("Wave two done");
        }
    }

    private void WaveThree()
    {
        if (waveThreeStarted == false)
        {
            this.canBeDamaged = false;
            //Spawn 2 lvl 2 enemies (tree leg) in opposite corners
            GameObject treeleg1 = Instantiate(treeleg, new Vector3(startingPos.x + 3, startingPos.y + 3, startingPos.z), Quaternion.identity);
            waveThreeEnemies.Add(treeleg1);
            GameObject treeleg2 = Instantiate(treeleg, new Vector3(startingPos.x - 3, startingPos.y - 3, startingPos.z), Quaternion.identity);
            waveThreeEnemies.Add(treeleg2);
            //Spawn 1 lvl 4 enemy (bandit) in the middle
            GameObject bandit1 = Instantiate(bandit, startingPos, Quaternion.identity);
            waveThreeEnemies.Add(bandit1);

            //TODO: Pathfinding for Boss to hide

            waveThreeStarted = true;
            Debug.Log("Wave three started");
        }
        for (int i = 0; i < waveThreeEnemies.Count; i++)
        {
            if (waveThreeEnemies[i].gameObject == null)
            {
                waveThreeEnemies.RemoveAt(i);
            }
        }
        if (waveThreeStarted && waveThreeEnemies.Count == 0 && !waveThreeDone)
        {
            //TODO: Release boss again

            //Spawn 2 lvl 3 enemies
            //Add to "supportcrew 3 list"
            GameObject bomber1 = Instantiate(bomber, new Vector3(startingPos.x + 3, startingPos.y, startingPos.z), Quaternion.identity);
            GameObject bomber2 = Instantiate(bomber, new Vector3(startingPos.x - 3, startingPos.y, startingPos.z), Quaternion.identity);
            supportThree.Add(bomber1);
            supportThree.Add(bomber2);
            this.canBeDamaged = true;
            waveThreeDone = true;
            Debug.Log("Wave three done");
        }
    }

    private void WaveFour()
    {
        if(waveFourStarted == false)
        {
            this.canBeDamaged = false;
            //Spawn 3 lvl 3 enemies
            GameObject bomber1 = Instantiate(bomber, new Vector3(startingPos.x + 2, startingPos.y + 2, startingPos.z), Quaternion.identity);
            waveFourEnemies.Add(bomber1);
            GameObject bomber2 = Instantiate(bomber, new Vector3(startingPos.x - 2, startingPos.y + 2, startingPos.z), Quaternion.identity);
            waveFourEnemies.Add(bomber2);
            GameObject bomber3 = Instantiate(bomber, new Vector3(startingPos.x, startingPos.y - 2, startingPos.z), Quaternion.identity);
            waveFourEnemies.Add(bomber3);

            //TODO: Pathfinding for Boss to hide
            waveFourStarted = true;
            Debug.Log("Wave four started");
        }
        for (int i = 0; i < waveFourEnemies.Count; i++)
        {
            if (waveFourEnemies[i].gameObject == null)
            {
                waveFourEnemies.RemoveAt(i);
            }
        }
        if (waveFourStarted && waveFourEnemies.Count == 0 && !waveFourDone)
        {
            //TODO: Release boss again for final fight
            this.canBeDamaged = true;
            waveFourDone = true;
            Debug.Log("Wave four done");
        }
    }

}
