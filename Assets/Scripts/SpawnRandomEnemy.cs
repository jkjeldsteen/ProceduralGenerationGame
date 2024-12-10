using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomEnemy : MonoBehaviour
{
    public List<WeightedEnemiesValue> weightedEnemies;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject randomEnemy = GetRandomEnemy(weightedEnemies);
            Instantiate(randomEnemy, transform.position, Quaternion.identity);
            //Debug.Log(randomEnemy);
        }
    }

    GameObject GetRandomEnemy(List<WeightedEnemiesValue> weightedEnemyList)
    {
        GameObject output = null;

        int totalWeight = 0;
        foreach (var entry in weightedEnemyList)
        {
            totalWeight += entry.weight;
        }
        int rndWeightValue = Random.Range(1, totalWeight + 1);

        int processedWeight = 0;
        foreach(var entry in weightedEnemyList)
        {
            processedWeight += entry.weight;
            if(rndWeightValue <= processedWeight)
            {
                output = entry.enemy;
                break;
            }
        }

        return output;
    }
}
