using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToDuplicate; // Tile prefab
    [SerializeField] GameObject[] treePrefabs; // Array of different tree prefabs
    public int tileSize = 9;
    public int viewRadius = 4;
    public int maxTreesPerTile = 1;

    [Header("Other Objects Management")]
    [SerializeField] string otherObjectTag = "ExpOrb"; 

    private Dictionary<Vector2Int, TileData> spawnedTiles = new Dictionary<Vector2Int, TileData>();
    private Dictionary<Vector2Int, List<TreeData>> savedTreePositions = new Dictionary<Vector2Int, List<TreeData>>();

    private Dictionary<Vector2Int, List<GameObject>> activeManagedObjects = new Dictionary<Vector2Int, List<GameObject>>();

    private Vector2Int currentTileCoord;

    void Start()
    {
        if (objectToDuplicate == null || treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogError("Tile or Tree prefabs are not assigned!");
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
                    Vector3 tilePosition = new Vector3(tileCoord.x * tileSize, tileCoord.y * tileSize, 0);
                    GameObject tile = Instantiate(objectToDuplicate, tilePosition, Quaternion.identity);

                    List<GameObject> treeList = new List<GameObject>();

                    if (!savedTreePositions.ContainsKey(tileCoord))
                    {
                        int treeCount = Random.Range(1, maxTreesPerTile + 1);
                        List<TreeData> treeDataList = new List<TreeData>();

                        for (int i = 0; i < treeCount; i++)
                        {
                            Vector3 offset = new Vector3(
                                Random.Range(0.5f, tileSize - 0.5f),
                                Random.Range(0.5f, tileSize - 0.5f),
                                0
                            );
                            Vector3 treePos = tilePosition + offset;
                            int treeIndex = Random.Range(0, treePrefabs.Length);
                            GameObject selectedTreePrefab = treePrefabs[treeIndex];
                            GameObject tree = Instantiate(selectedTreePrefab, treePos, Quaternion.identity);
                            treeList.Add(tree);
                            treeDataList.Add(new TreeData(treePos, treeIndex));
                        }
                        savedTreePositions[tileCoord] = treeDataList;
                    }
                    else
                    {
                        foreach (TreeData treeData in savedTreePositions[tileCoord])
                        {
                            GameObject tree = Instantiate(treePrefabs[treeData.prefabIndex], treeData.position, Quaternion.identity);
                            treeList.Add(tree);
                        }
                    }

                    spawnedTiles[tileCoord] = new TileData(tile, treeList);
                }

                if (activeManagedObjects.ContainsKey(tileCoord))
                {
                    foreach (GameObject obj in activeManagedObjects[tileCoord])
                    {
                        if (obj != null && !obj.activeSelf)
                        {
                            obj.SetActive(true);
                        }
                    }
                }
            }
        }

        FindAndManageOtherObjects();
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

                if (activeManagedObjects.ContainsKey(coord))
                {
                    foreach (GameObject obj in activeManagedObjects[coord])
                    {
                        if (obj != null && obj.activeSelf)
                        {
                            obj.SetActive(false);
                        }
                    }
                }

                tilesToRemove.Add(coord);
            }
        }

        foreach (var coord in tilesToRemove)
        {
            spawnedTiles.Remove(coord);
        }
    }

    void FindAndManageOtherObjects()
    {
        GameObject[] allOtherObjects = GameObject.FindGameObjectsWithTag(otherObjectTag);

        HashSet<Vector2Int> currentViewTiles = new HashSet<Vector2Int>();
        for (int dx = -viewRadius; dx <= viewRadius; dx++)
        {
            for (int dy = -viewRadius; dy <= viewRadius; dy++)
            {
                currentViewTiles.Add(new Vector2Int(currentTileCoord.x + dx, currentTileCoord.y + dy));
            }
        }

        foreach (GameObject obj in allOtherObjects)
        {
            if (obj == null) continue;

            Vector2Int objTileCoord = GetTileCoord(obj.transform.position);

            if (!activeManagedObjects.ContainsKey(objTileCoord))
            {
                activeManagedObjects[objTileCoord] = new List<GameObject>();
            }

            if (!activeManagedObjects[objTileCoord].Contains(obj))
            {
                activeManagedObjects[objTileCoord].Add(obj);
            }

            if (currentViewTiles.Contains(objTileCoord))
            {
                if (!obj.activeSelf) obj.SetActive(true);
            }
            else
            {
                if (obj.activeSelf) obj.SetActive(false);
            }
        }
    }

    // --- Data Structures ---
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

    class TreeData
    {
        public Vector3 position;
        public int prefabIndex;

        public TreeData(Vector3 pos, int index)
        {
            position = pos;
            prefabIndex = index;
        }
    }
}
