using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
        public float xPos;
        public float yPos;

        public Cell(float xpos, float ypos)
        {
            this.xPos = xpos;
            this.yPos = ypos;
        }
    }
    public class TreeNode
    {
        public float xPos;
        public float yPos;

        public TreeNode(float xpos, float ypos)
        {
            this.xPos = xpos;
            this.yPos = ypos;
        }
    }
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bossEnemy;
    [SerializeField] private GameObject abilityCodex;
    [SerializeField] private GameObject healthPot;
    public List<WeightedEnemiesValue> weightedEnemies;
    [SerializeField] private GameObject[] endFloorArtefact;
    public Gamehandler gameHandler;

    public int k;
    private Vector2 size;
    private Vector2 offset;
    private int startPos = 0;
    [SerializeField] private GameObject room;
    [SerializeField] private GameObject lockedRoomUpDown;
    [SerializeField] private GameObject lockedRoomRightLeft;
    [SerializeField] private GameObject key;
    private bool dashCodexSpawned = false;
    private int roomCount;
    public int enemyCount;
    public bool keylockSpawned;
    public List<TreeNode> roomNodes;

    public bool playerSpawned;
    private Vector3 playerStartPos;
    private bool endRoomSpawned;
    private bool endFloorArtefactSpawned;
    private bool hasFaded;
    public int levelCount;
    public bool RunGeneration;
    public bool gameStarted;

    List<Cell> board;
    void Start()
    {
        gameHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gamehandler>();
        if (levelCount > 1)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        size = new Vector2(5, 5);
        offset = new Vector2(10, 10);
        RunGeneration = true;
        roomNodes = new List<TreeNode>();
        MazeGenerator();
        playerSpawned = false;
        StartCoroutine(gameHandler.CanvasFade());
        endRoomSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerSpawned == false)
        {
            TreeNode startRoom = roomNodes[0];
            playerStartPos = new Vector3(startRoom.xPos, startRoom.yPos, 0);
            if (levelCount == 1)
            {
                Instantiate(player, playerStartPos, Quaternion.identity);
                playerSpawned = true;
                playerController.canMove = true;
                player = GameObject.FindGameObjectWithTag("Player");
            }
            else if (levelCount > 1)
            {
                playerController.canMove = true;
                player.transform.position = playerStartPos;
                playerSpawned = true;
            }
        }
        if(endRoomSpawned == false && RunGeneration == false && enemyCount == 0)
        {
            TreeNode endRoom = roomNodes[roomNodes.Count - 1];
            SpawnEndFloor(endRoom);
            endRoomSpawned = true;
        }
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var newRoom = Instantiate(room, new Vector3(i * offset.x, -j * offset.y, 0), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                newRoom.UpdateRoom(board[Mathf.FloorToInt(i + j * size.x)].status);

                newRoom.name += " " + i + "-" + j;
                
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for(int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell(j * offset.y, -i * offset.x));
            }
        }

        int currentCell = startPos;
        TreeNode currNode = new(0,0);

        Stack<int> path = new Stack<int>();

        k = 0;
        int roomNo = 0;
        while (k<100)
        {
            k++;

            board[currentCell].visited = true;

            //Check the cell's neighbours
            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    bool roomAlreadyInList = false;
                    currNode.xPos = board[currentCell].xPos;
                    currNode.yPos = board[currentCell].yPos;
                    for (int i = 0; i < roomNodes.Count; i++)
                    {
                        if (currNode.xPos == roomNodes[i].xPos && currNode.yPos == roomNodes[i].yPos)
                        {
                            roomAlreadyInList = true;
                            break;
                        }
                    }
                    if (roomAlreadyInList == false)
                    {
                        roomNodes.Add(new TreeNode(currNode.xPos, currNode.yPos));
                    }
                    //Removes the last element from stack and sets the currentcell to be that last element
                    currentCell = path.Pop();
                }
            }
            else
            {
                //Throws currentcell into the stack at the end
                path.Push(currentCell);

                currNode.xPos = board[currentCell].xPos;
                currNode.yPos = board[currentCell].yPos;
                bool roomAlreadyInList = false;
                for (int i = 0; i < roomNodes.Count; i++)
                {
                    if(currNode.xPos == roomNodes[i].xPos && currNode.yPos == roomNodes[i].yPos)
                    {
                        roomAlreadyInList = true;
                        break;
                    }
                }
                if(roomAlreadyInList == false)
                {
                    roomNodes.Add(new TreeNode(currNode.xPos, currNode.yPos));
                }
                int newCell = neighbours[Random.Range(0, neighbours.Count)];
                

                if (newCell > currentCell)
                {
                    //Down or right?
                    if(newCell -1 == currentCell)
                    {
                        //Right neighbour
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        //Down neighbour
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //Up or left
                    if (newCell + 1 == currentCell)
                    {
                        //Left neighbour
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        //Up neighbour
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
                roomNo++;
                if (keylockSpawned == false && roomNo > 2)
                {
                    int lockedRoomChance = Random.Range(0, 10);
                    if(lockedRoomChance == 0)
                    {
                        SpawnLockedDoor(0);
                    }
                }
            }
        }
        RunGeneration = false;

        for (int i = 1; i < roomNodes.Count; i++)
        {
            currNode = roomNodes[i];
            if (currNode.xPos != 0 || currNode.yPos != 0)
            {
                SpawnDashCodex(currNode);
                SpawnEnemy(currNode);
                SpawnHealthPot(currNode);
            }
        }
        if (keylockSpawned == false)
        {
            SpawnLockedDoor(0);
        }
        GenerateDungeon();
    }

    private void SpawnLockedDoor(int roomCountModifier)
    {
        bool doorSpawned = false;
        if(roomCountModifier >= roomNodes.Count + 2)
        {
            roomCountModifier = roomNodes.Count - 2;
        }
        TreeNode currRoom = roomNodes[roomNodes.Count - 1 - roomCountModifier];
        TreeNode roomBefore = roomNodes[roomNodes.Count - 2 - roomCountModifier];
        if(currRoom.xPos == roomBefore.xPos && currRoom.yPos == roomBefore.yPos)
        {
            doorSpawned = false;
            SpawnLockedDoor(roomCountModifier++);
        }
        else if (currRoom.xPos == roomBefore.xPos)
        {
            Instantiate(lockedRoomUpDown, new Vector2(currRoom.xPos - ((currRoom.xPos - roomBefore.xPos) / 2), currRoom.yPos - ((currRoom.yPos - roomBefore.yPos) / 2)), Quaternion.identity);
            doorSpawned = true;
        }
        else if (currRoom.yPos == roomBefore.yPos)
        {
            Instantiate(lockedRoomRightLeft, new Vector2(currRoom.xPos - ((currRoom.xPos - roomBefore.xPos) / 2), currRoom.yPos - ((currRoom.yPos - roomBefore.yPos) / 2) + 0.42f), Quaternion.identity);
            doorSpawned = true;
        }
        else
        {
            doorSpawned = false;
            SpawnLockedDoor(++roomCountModifier);
        }
        if (doorSpawned)
        {
            int keyPosNo = Random.Range(0, roomNodes.Count - 1 - roomCountModifier);
            TreeNode keyPos = roomNodes[keyPosNo];
            Instantiate(key, new Vector2(keyPos.xPos, keyPos.yPos), Quaternion.identity);
            keylockSpawned = true;
        }        
    }

    private void SpawnDashCodex(TreeNode currRoom)
    {
        if (levelCount == 1)
        {
            int spawnOffsetX = Random.Range(-2, 2);
            int spawnOffsetY = Random.Range(-2, 2);
            Vector2 spawnPos = new Vector2(currRoom.xPos + spawnOffsetX, currRoom.yPos + spawnOffsetY);
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

    private void SpawnEnemy(TreeNode currRoom)
    {
        Vector2 spawnPos = new Vector2(currRoom.xPos, currRoom.yPos);
        if (roomCount >= 0 && roomCount < 23)
        {
            GameObject randEnemy = GetRandomEnemy(weightedEnemies);
            Instantiate(randEnemy, spawnPos, Quaternion.identity);
            enemyCount++;
        }
        roomCount++;
    }

    private void SpawnEndFloor(TreeNode currRoom)
    {
        Vector2 spawnPos = new Vector2(currRoom.xPos, currRoom.yPos);
        if(levelCount == 5)
        {
            Instantiate(bossEnemy, spawnPos, Quaternion.identity);
        }
        if (endFloorArtefactSpawned == false)
        {
            Instantiate(endFloorArtefact[0], spawnPos, Quaternion.identity);
            endFloorArtefactSpawned = true;
        }
    }

    private void SpawnHealthPot(TreeNode currRoom)
    {
        int spawnX = Random.Range(-3, 3);
        int spawnY = Random.Range(-3, 3);
        Vector3 spawnPos = new Vector3(currRoom.xPos + spawnX, currRoom.yPos + spawnY, transform.position.z);
        int spawnChance = Random.Range(1, 3);
        if (spawnChance == 1)
        {
            Instantiate(healthPot, spawnPos, Quaternion.identity);
        }
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        //Check up neighbour
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell-size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }
        //Check down neighbour
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }
        //Check right neighbour
        if ((cell+1) % size.x != 0 && !board[Mathf.FloorToInt(cell +1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }
        //Check left neighbour
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }
        return neighbours;
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
}
