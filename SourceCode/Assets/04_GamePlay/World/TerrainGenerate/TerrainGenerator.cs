using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TerrainGenerator:MonoBehaviour
{
    public DefaultTerrainConfig config;

    [Header("瓦片地图")]
    public Tilemap ground;
    [Header("玩家引用，用于判断位置")]
    public Transform playerTransform;

    //临时变量
    private float xOffset;//用于Perlin噪声x参数的偏移，避免每次运行生成的Perlin噪声一样
    private float yOffset;//用于Perlin噪声y参数的偏移，避免每次运行生成的Perlin噪声一样
    // 存储当前已加载的区块
    private Dictionary<Vector2Int, TerrainChunk> loadedChunks = new Dictionary<Vector2Int, TerrainChunk>();
    private Vector2Int lastPlayerChunkCoord;

    private void Start()
    {
        Init();
        if (playerTransform != null)
        {
            lastPlayerChunkCoord = GetChunkCoordFromPosition(playerTransform.position);
            UpdateChunks(); // 初始加载一次
        }
    }
    private void Update()
    {
        if (playerTransform == null) return;

        // 检查玩家是否移动到了新的区块
        Vector2Int currentChunkCoord = GetChunkCoordFromPosition(playerTransform.position);
        if (currentChunkCoord != lastPlayerChunkCoord)
        {
            lastPlayerChunkCoord = currentChunkCoord;
            UpdateChunks();
        }
    }
    public void Init()
    {
        // 初始化随机偏移
        xOffset = Random.Range(config.randomMin, config.randomMax);
        yOffset = Random.Range(config.randomMin, config.randomMax);
    }
    private Vector2Int GetChunkCoordFromPosition(Vector3 position)
    {
        int chunkX = Mathf.FloorToInt(position.x / config.chunkSize);
        int chunkY = Mathf.FloorToInt(position.y / config.chunkSize);
        return new Vector2Int(chunkX, chunkY);
    }

    private void UpdateChunks()
    {
        // 1. 确定需要加载的区块范围
        HashSet<Vector2Int> chunksToKeep = new HashSet<Vector2Int>();
        for (int x = -config.loadRadius; x <= config.loadRadius; x++)
        {
            for (int y = -config.loadRadius; y <= config.loadRadius; y++)
            {
                Vector2Int chunkCoord = new Vector2Int(lastPlayerChunkCoord.x + x, lastPlayerChunkCoord.y + y);
                chunksToKeep.Add(chunkCoord);

                // 2. 如果这个区块还没加载，就生成它
                if (!loadedChunks.ContainsKey(chunkCoord))
                {
                    TerrainChunk newChunk = new TerrainChunk(chunkCoord, config, ground, xOffset, yOffset,this);
                    newChunk.Generate();
                    loadedChunks.Add(chunkCoord, newChunk);
                }
            }
        }

        // 3. 卸载所有不在需要范围内的区块
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in loadedChunks)
        {
            if (!chunksToKeep.Contains(chunk.Key))
            {
                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (Vector2Int chunkCoord in chunksToRemove)
        {
            loadedChunks[chunkCoord].Clear();
            loadedChunks.Remove(chunkCoord);
        }
    }
    
}
