using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DecorationManager : MonoBehaviour
{
    [SerializeField] private TileBase[] decorationTiles;
    [SerializeField] private TileBase[] nonCollidableDecorationTiles;

    [SerializeField] private Tilemap decorationMap;
    [SerializeField] private Tilemap nonCollidableDecorationMap;

    public Vector3Int[] positions;

    void Start()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            int spawnTileDecider = Random.Range(0, 3);
            if(spawnTileDecider == 1)
            {
                int randomTile = Random.Range(0, decorationTiles.Length);
                decorationMap.SetTile(positions[i], decorationTiles[randomTile]);
            }
            else if(spawnTileDecider == 2)
            {
                int randomTile = Random.Range(0, nonCollidableDecorationTiles.Length);
                nonCollidableDecorationMap.SetTile(positions[i], nonCollidableDecorationTiles[randomTile]);
            }
        }
    }
}
