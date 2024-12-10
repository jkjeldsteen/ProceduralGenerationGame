using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms;
    //Rooms without specific doors for end of row or column
    public GameObject[] roomsWithoutRightDoor;
    public GameObject[] roomsWithoutLeftDoor;
    public GameObject[] roomsWithoutTopDoor;
    public GameObject[] roomsWithoutBottomDoor;
    public GameObject[] cornerRooms; //Topleft = 0, Topright = 1, Bottomright = 2, Bottomleft = 3.

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject abilityCodex;
    [SerializeField] private GameObject healthPot;
    public List<WeightedEnemiesValue> weightedEnemies;
    [SerializeField] private GameObject[] endFloorArtefact;
    private Gamehandler gameHandler;
    private int direction;
    private bool dashCodexSpawned = false;
    public float moveAmount;
    private int rowCount; //1 to 5
    private int columnCount; //1 to 5
    private int roomCount;
    public int enemyCount;
    Vector3 playerStartPos;
    public bool playerSpawned;
    private bool endFloorArtefactSpawned;
    private bool hasFaded;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.5f; //set to 0.25f

    public float minX;
    public float maxX;
    public float minY;
    private int downCounter;
    public bool RunGeneration;
    public int levelCount;
    public LayerMask room;
    public bool gameStarted;

    private void Start()
    {
        hasFaded = false;
        playerSpawned = false;
        gameStarted = false;
        RunGeneration = true;
        if(levelCount > 1)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        player.GetComponent<PlayerController>().canMove = false;
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gamehandler>();
        int randStartingPos = Random.Range(0, startingPositions.Length);
        columnCount = 1;
        rowCount = randStartingPos+1; //+1 fordi random kører fra index 0-4 men rows tælles i 1-5
        transform.position = startingPositions[randStartingPos].position;
        if(rowCount <= 1)
        {
            Instantiate(cornerRooms[0], transform.position, Quaternion.identity);
        }
        else if(rowCount >= 5)
        {
            Instantiate(cornerRooms[1], transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(rooms[1], transform.position, Quaternion.identity);
        }
        playerStartPos = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
        direction = Random.Range(1,6);
        roomCount = 0;
        enemyCount = 0;
    }

    private void Update()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (timeBtwRoom <= 0 && RunGeneration == true){
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }else{
            timeBtwRoom -= Time.deltaTime;
        }
        if (RunGeneration == false && gameStarted == false)
        {
            if(levelCount > 1)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            playerController.canMove = true;
            gameStarted = true;
        }
        else if (RunGeneration == true)
        {
            playerController.canMove = false;
        }
        SpawnEndFloor();
        if(playerSpawned == false && RunGeneration == false)
        {
            if(levelCount == 1)
            {
                Instantiate(player, playerStartPos, Quaternion.identity);
                playerSpawned = true;
                playerController.canMove = true;
                player = GameObject.FindGameObjectWithTag("Player");
            }else if (levelCount > 1)
            {
                playerController.canMove = true;
                player.transform.position = playerStartPos;
                playerSpawned = true;
            }
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
        foreach (var entry in weightedEnemyList)
        {
            processedWeight += entry.weight;
            if (rndWeightValue <= processedWeight)
            {
                output = entry.enemy;
                break;
            }
        }

        return output;
    }

    private void Move()
    {
        if(direction == 1 || direction == 2)    // Move Right
        {
            downCounter = 0;
            if(transform.position.x < maxX) {
                SpawnEnemy();
                rowCount++;
                transform.position += Vector3.right * moveAmount;
                if(rowCount >= 5 && columnCount >= 5)
                {
                        Instantiate(cornerRooms[2], transform.position, Quaternion.identity);
                }
                else if(rowCount >= 5 && columnCount <= 1)
                {
                    Instantiate(cornerRooms[1], transform.position, Quaternion.identity);
                }
                else if(rowCount >= 5)
                {
                    int rand = Random.Range(0, roomsWithoutRightDoor.Length);
                    Instantiate(roomsWithoutRightDoor[rand], transform.position, Quaternion.identity);
                }
                else if(columnCount >= 5)
                {
                    int rand = Random.Range(0, roomsWithoutBottomDoor.Length);
                    Instantiate(roomsWithoutBottomDoor[rand], transform.position, Quaternion.identity);
                }
                else if(columnCount <= 1)
                {
                    int rand = Random.Range(0, roomsWithoutTopDoor.Length);
                    Instantiate(roomsWithoutTopDoor[rand], transform.position, Quaternion.identity);
                }
                else
                {
                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);
                }               
                direction = Random.Range(1,6);
                if(direction == 3){
                    direction = 2;
                }else if(direction == 4){
                    direction = 5;
                }
            }else {
                direction = 5;
            }
        }

        else if(direction == 3 || direction == 4)   // Move Left
        {
            downCounter = 0;
            if(transform.position.x > minX) {
                SpawnEnemy();
                rowCount--;
                transform.position += Vector3.left * moveAmount;

                if (rowCount <= 1 && columnCount >= 5)
                {
                    Instantiate(cornerRooms[3], transform.position, Quaternion.identity);
                }
                else if(rowCount <= 1 && columnCount <= 1)
                {
                    Instantiate(cornerRooms[0], transform.position, Quaternion.identity);
                }
                else if(rowCount <= 1)
                {
                    int rand = Random.Range(0, roomsWithoutLeftDoor.Length);
                    Instantiate(roomsWithoutLeftDoor[rand], transform.position, Quaternion.identity);
                }
                else if (columnCount >= 5)
                {
                    int rand = Random.Range(0, roomsWithoutBottomDoor.Length);
                    Instantiate(roomsWithoutBottomDoor[rand], transform.position, Quaternion.identity);
                }
                else if (columnCount <= 1)
                {
                    int rand = Random.Range(0, roomsWithoutTopDoor.Length);
                    Instantiate(roomsWithoutTopDoor[rand], transform.position, Quaternion.identity);
                }
                else
                {
                    int rand = Random.Range(0, rooms.Length);
                    Instantiate(rooms[rand], transform.position, Quaternion.identity);
                }
                direction = Random.Range(3,6);

            } else{
                direction = 5;
            }
        }

        else if(direction == 5) // Move down
        {
            downCounter++;
            if (transform.position.y > minY)
            {
                SpawnEnemy();
                columnCount++;
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                //Roomtypes: LR = 0, LRB = 1, LRT = 2, LRTB = 3
                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {
                    if(rowCount > 1 && rowCount < 5)
                    {
                        if(columnCount < 5)
                        {
                            if(columnCount == 2)
                            {
                                roomDetection.GetComponent<RoomType>().RoomDestruction();
                                Instantiate(roomsWithoutTopDoor[3], transform.position, Quaternion.identity);
                            }
                            else
                            {
                                if (downCounter >= 2)
                                {
                                    roomDetection.GetComponent<RoomType>().RoomDestruction();
                                    Instantiate(rooms[3], transform.position, Quaternion.identity);
                                }
                                else
                                {
                                    roomDetection.GetComponent<RoomType>().RoomDestruction();

                                    int randbottomroom = Random.Range(1, 4);
                                    if (randbottomroom == 2)
                                    {
                                        randbottomroom = 1;
                                    }
                                    Instantiate(rooms[randbottomroom], transform.position, Quaternion.identity);
                                }
                            }
                        }
                        else if(columnCount >= 5)
                        {
                            if (downCounter >= 2)
                            {
                                roomDetection.GetComponent<RoomType>().RoomDestruction();
                                Instantiate(rooms[3], transform.position, Quaternion.identity);
                            }
                            else
                            {
                                roomDetection.GetComponent<RoomType>().RoomDestruction();
                                
                                //Choose a random room with a top and bottom openingm these are located at index 3 og 4.
                                int randbottomroom = Random.Range(1, rooms.Length);
                                if(randbottomroom == 2)
                                {
                                    randbottomroom = 1;
                                }
                                Instantiate(rooms[randbottomroom], transform.position, Quaternion.identity);
                            }
                        }
                        
                    }
                    else if(rowCount <= 1)
                    {
                        if (downCounter >= 2)
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(roomsWithoutLeftDoor[3], transform.position, Quaternion.identity);
                        }
                        else
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(roomsWithoutLeftDoor[3], transform.position, Quaternion.identity);
                        }
                    }
                    else if(rowCount >= 5)
                    {
                        if (downCounter >= 2)
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(roomsWithoutRightDoor[3], transform.position, Quaternion.identity);
                        }
                        else
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(roomsWithoutRightDoor[3], transform.position, Quaternion.identity);
                        }
                    }
                    
                }
                transform.position += Vector3.down * moveAmount;
                if (rowCount > 1 && rowCount < 5)
                {
                    if (columnCount < 5)
                    {
                        int rand = Random.Range(2, rooms.Length);
                        Instantiate(rooms[rand], transform.position, Quaternion.identity);
                    }
                    else if(columnCount >= 5)
                    {
                        int rand = Random.Range(2, roomsWithoutBottomDoor.Length);
                        Instantiate(roomsWithoutBottomDoor[rand], transform.position, Quaternion.identity);
                    }
                }
                else if(rowCount <= 1)
                {
                    if(columnCount < 5)
                    {
                        int rand = Random.Range(2, roomsWithoutLeftDoor.Length);
                        Instantiate(roomsWithoutLeftDoor[3], transform.position, Quaternion.identity);
                    }
                    else if(columnCount >= 5)
                    {
                        Instantiate(cornerRooms[3], transform.position, Quaternion.identity);
                    }
                }
                else if(rowCount >= 5)
                {
                    if (columnCount < 5)
                    {
                        int rand = Random.Range(2, roomsWithoutRightDoor.Length);
                        Instantiate(roomsWithoutRightDoor[3], transform.position, Quaternion.identity);
                    }
                    else if(columnCount >= 5)
                    {
                        Instantiate(cornerRooms[2], transform.position, Quaternion.identity);
                    }
                }

                direction = Random.Range(1,6);
            }else //STOP GENERATION
            {
                RunGeneration = false;
                if (!hasFaded)
                {
                    gameHandler.CanvasFade();
                    hasFaded = true;
                }
            }
        }
        SpawnDashCodex();
        SpawnHealthPot();
    }

    private void SpawnDashCodex()
    {
        if(levelCount == 1)
        {
            int spawnOffsetX = Random.Range(-2, 2);
            int spawnOffsetY = Random.Range(-2, 2);
            Vector3 spawnPos = new Vector3(transform.position.x + spawnOffsetX, transform.position.y + spawnOffsetY, transform.position.z);
            if (player.GetComponent<PlayerController>().dashUnlocked == false)
            {
                if (!dashCodexSpawned && RunGeneration == true)
                {
                    int rand = Random.Range(1, 5);
                    if (rand == 1)
                    {
                        Instantiate(abilityCodex, spawnPos, Quaternion.identity);
                        dashCodexSpawned = true;
                    }
                }
                else if (!dashCodexSpawned && RunGeneration != true)
                {
                    Instantiate(abilityCodex, spawnPos, Quaternion.identity);
                    dashCodexSpawned = true;
                }
            }
        }
    }

    private void SpawnHealthPot()
    {
        int spawnX = Random.Range(-3, 3);
        int spawnY = Random.Range(-3, 3);
        Vector3 spawnPos = new Vector3(transform.position.x + spawnX, transform.position.y + spawnY, transform.position.z);
        int spawnChance = Random.Range(1, 3);
        if(spawnChance == 1)
        {
            Instantiate(healthPot, spawnPos, Quaternion.identity);
        }
    }

    private void SpawnEnemy()
    {
        if(roomCount >= 1 && RunGeneration)
        {
            GameObject randEnemy = GetRandomEnemy(weightedEnemies);
            Instantiate(randEnemy, transform.position, Quaternion.identity);
            enemyCount++;
        }
        else if (!RunGeneration)
        {
            GameObject randEnemy = GetRandomEnemy(weightedEnemies);
            Instantiate(randEnemy, transform.position, Quaternion.identity);
            enemyCount++;
        }
        roomCount++;
    }

    private void SpawnEndFloor()
    {
        if (RunGeneration == false && enemyCount == 0 && endFloorArtefactSpawned == false)
        {
            int rand = Random.Range(0, endFloorArtefact.Length);
            Instantiate(endFloorArtefact[0], transform.position, Quaternion.identity);
            endFloorArtefactSpawned = true;
        }
    }
}
