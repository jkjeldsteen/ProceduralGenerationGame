using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public LevelGeneration levelGen;
    public GameObject[] fillerRooms;
    [SerializeField] private int roomType; //0 for normal, 1 for left side, 2 for right side, 3 for top, 4 for bottom, 5 for topleft, 6 for topright, 7 for botright, 8 for botleft.

    void Update()
    {
        levelGen = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<LevelGeneration>();
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if(roomDetection == null && levelGen.RunGeneration == false)
        {
            int room = 0;
            if(roomType == 0)
            {
                room = Random.Range(0, fillerRooms.Length);
            }else{
                room = roomType;
            }
            Instantiate(fillerRooms[room], transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
    }
}
