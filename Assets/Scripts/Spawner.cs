using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToDuplicate; // Tile prefab
    [SerializeField] GameObject treePrefab; // Tree prefab
    public int tileSize = 5;
    public int viewRadius = 1;
    public int maxTreesPerTile = 5;

    // Store tile and its associated trees
    private Dictionary<Vector2Int, TileData> spawnedTiles = new Dictionary<Vector2Int, TileData>();
    private Vector2Int currentTileCoord;

    void Start()
    {
        if (objectToDuplicate == null || treePrefab == null)
        {
            Debug.LogError("Tile or Tree prefab is not assigned!");
            return;
        }

        currentTileCoord = GetTileCoord(transform.position);
        SpawnAround(currentTileCoord);
    }

    void Update()
    {
        Vector2Int newTileCoord = GetTileCoord(transform.position);
        if (newTileCoord != currentTileCoord)
        {
            currentTileCoord = newTileCoord;
            SpawnAround(currentTileCoord);
            CleanupDistantTiles(currentTileCoord);
        }
    }

    Vector2Int GetTileCoord(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / tileSize);
        int y = Mathf.FloorToInt(position.y / tileSize);
        return new Vector2Int(x, y);
    }

    void SpawnAround(Vector2Int centerTile)
    {
        for (int x = -viewRadius; x <= viewRadius; x++)
        {
            for (int y = -viewRadius; y <= viewRadius; y++)
            {
                Vector2Int tileCoord = new Vector2Int(centerTile.x + x, centerTile.y + y);
                if (!spawnedTiles.ContainsKey(tileCoord))
                {
                    Vector3 spawnPosition = new Vector3(tileCoord.x * tileSize, tileCoord.y * tileSize, 0);
                    GameObject tile = Instantiate(objectToDuplicate, spawnPosition, Quaternion.identity);

                    List<GameObject> treeList = new List<GameObject>();
                    for (int i = 0; i < Random.Range(1, maxTreesPerTile + 1); i++)
                    {
                        Vector3 randomOffset = new Vector3(
                            Random.Range(0.5f, tileSize - 0.5f),
                            Random.Range(0.5f, tileSize - 0.5f),
                            0
                        );
                        Vector3 treePosition = spawnPosition + randomOffset;
                        GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);
                        treeList.Add(tree);
                    }

                    spawnedTiles.Add(tileCoord, new TileData(tile, treeList));
                }
            }
        }
    }

    void CleanupDistantTiles(Vector2Int centerTile)
    {
        List<Vector2Int> tilesToRemove = new List<Vector2Int>();

        foreach (var pair in spawnedTiles)
        {
            Vector2Int coord = pair.Key;
            int dx = Mathf.Abs(coord.x - centerTile.x);
            int dy = Mathf.Abs(coord.y - centerTile.y);

            if (dx > viewRadius || dy > viewRadius)
            {
                Destroy(pair.Value.tileObject);
                foreach (GameObject tree in pair.Value.trees)
                {
                    Destroy(tree);
                }
                tilesToRemove.Add(coord);
            }
        }

        foreach (var coord in tilesToRemove)
        {
            spawnedTiles.Remove(coord);
        }
    }

    // Helper class to group tile and trees
    class TileData
    {
        public GameObject tileObject;
        public List<GameObject> trees;

        public TileData(GameObject tile, List<GameObject> trees)
        {
            this.tileObject = tile;
            this.trees = trees;
        }
    }
}
