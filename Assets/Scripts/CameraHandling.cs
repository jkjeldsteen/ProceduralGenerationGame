using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandling : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    private DungeonGenerator levelGeneration;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        offset = new Vector3(0,0,-1);
        levelGeneration = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<DungeonGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelGeneration.playerSpawned)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, offset.z);
        }
    }

    public IEnumerator Shake(float duration, float magnitude) {

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            offset.x = x;
            offset.y = y;

            elapsed += Time.deltaTime;

            yield return null;
        }

        offset.x = 0;
        offset.y = 0;
    }
}
