using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

// 这个类代表一个地图区块
public class TerrainChunk
{
    public enum ChunkState
    {
        Empty,          // 未生成
        Generating,     // 生成中
        Generated,      // 已生成
        Clearing,       // 清除中
    }
    public Vector2Int Coord { get; private set; } // 区块的坐标（例如 (0,0), (1,0)）
    public Bounds Bounds { get; private set; } // 区块的世界空间边界
    public ChunkState State { get; private set; } // 当前状态
    public bool IsActive => State == ChunkState.Generated;

    private Tilemap groundTilemap;
    private DefaultTerrainConfig config;
    private float xOffset;
    private float yOffset;
    private MonoBehaviour coroutineRunner;
    private Coroutine currentCoroutine;
    
    public TerrainChunk(Vector2Int coord, DefaultTerrainConfig terrainConfig, Tilemap tilemap, float xOff, float yOff,MonoBehaviour runner)
    {
        Coord = coord;
        config = terrainConfig;
        groundTilemap = tilemap;
        xOffset = xOff;
        yOffset = yOff;
        coroutineRunner = runner;
        
        // 计算这个区块在世界空间中的实际边界
        int chunkWorldSize = config.chunkSize;
        Vector3 chunkWorldPos = new Vector3(coord.x * chunkWorldSize, coord.y * chunkWorldSize, 0);
        Bounds = new Bounds(chunkWorldPos + new Vector3(chunkWorldSize, chunkWorldSize) * 0.5f, new Vector3(chunkWorldSize, chunkWorldSize));
    }
    public void CancelCurrentOperation()
    {
        if (currentCoroutine != null)
        {
            coroutineRunner.StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        
        // 根据当前状态设置适当的状态
        if (State == ChunkState.Generating)
            State = ChunkState.Generated;
        else if (State == ChunkState.Clearing)
            State = ChunkState.Empty;
    }
    
    public void Generate()
    {
        CancelCurrentOperation(); // 先取消任何现有操作
        
        if (State == ChunkState.Empty)
        {
            currentCoroutine = coroutineRunner.StartCoroutine(GenerateRoutine());
        }
    }
    
    public void Clear()
    {
        CancelCurrentOperation(); // 先取消任何现有操作
        
        if (State == ChunkState.Generated)
        {
            currentCoroutine = coroutineRunner.StartCoroutine(ClearRoutine());
        }
    }

    private IEnumerator GenerateRoutine()
    {
        State = ChunkState.Generating;
        
        int startX = Coord.x * config.chunkSize;
        int startY = Coord.y * config.chunkSize;
        int endX = startX + config.chunkSize;
        int endY = startY + config.chunkSize;

        // 确保不超出地图边界
        startX = Mathf.Max(startX, config.baseSizeLeft);
        endX = Mathf.Min(endX, config.baseSizeRight);
        startY = Mathf.Max(startY, config.baseSizeBottom);
        endY = Mathf.Min(endY, config.baseSizeTop);

        for (int x = startX; x < endX; x++)
        {
            int surfaceY = GetSurfaceHeight(x);
            int bottomY = Mathf.Max(startY, config.baseSizeBottom);
            int topY = Mathf.Min(surfaceY, endY); // 确保不超出当前区块和地表

            for (int y = bottomY; y < topY; y++)
            {
                SetTile(x, y);
                // 每处理个瓦片等待一帧，避免卡顿
                if ((x * y) % config.updateCount == 0)
                    yield return null;
            }
        }
        for (int x = startX; x < endX; x++)
        {
            int surfaceY = GetSurfaceHeight(x);
            int bottomY = Mathf.Max(startY, config.baseSizeBottom);
            int topY = Mathf.Min(surfaceY, endY);
            int updateCount = 0;

            for (int y = bottomY; y < topY; y++)
            {
                if (groundTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    TileBase advancedTile = GetAdvancedTile(x, y);
                    if (advancedTile != null)
                    {
                        groundTilemap.SetTile(new Vector3Int(x, y, 0), advancedTile);
                        updateCount++;
                    }
                }
                
                // 等待一帧，避免卡顿
                if (updateCount >= config.updateCount)
                {
                    yield return null;
                    updateCount = 0;
                }
            }
        }
        State = ChunkState.Generated;
    }

    private IEnumerator ClearRoutine()
    {
        State = ChunkState.Clearing;
        
        // 只清除真正属于这个区块的瓦片（按实际生成的范围）
        int startX = Coord.x * config.chunkSize;
        int startY = Coord.y * config.chunkSize;
        int endX = startX + config.chunkSize;
        int endY = startY + config.chunkSize;

        // 确保不超出地图边界（与Generate保持一致）
        startX = Mathf.Max(startX, config.baseSizeLeft);
        endX = Mathf.Min(endX, config.baseSizeRight);
        startY = Mathf.Max(startY, config.baseSizeBottom);
        endY = Mathf.Min(endY, config.baseSizeTop);

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), null);
                // 等待一帧，避免卡顿
                if ((x * y) % config.updateCount == 0)
                    yield return null;
            }
        }
        State = ChunkState.Empty;
    }

    private int GetSurfaceHeight(int x)
    {
        return (int)(config.baseSizeTop - config.surfaceHeightVariation + Mathf.PerlinNoise(x * config.surfaceFrequency + xOffset, 0) * config.surfaceHeightVariation);
    }

    private void SetTile(int x, int y)
    {
        float perlinValue = CalculateCombinedPerlinNoise(x, y);
        Vector3Int position = new Vector3Int(x, y, 0);
        
        TileBase tileToSet = null;
        if (perlinValue < config.dirtThreshold)
            tileToSet = config.dirts[Random.Range(0, config.dirts.Length)];
        else
            return;
        
        groundTilemap.SetTile(position, tileToSet);
    }

    private float CalculateCombinedPerlinNoise(int x, int y)
    {
        float baseNoise = Mathf.PerlinNoise(x * config.baseFrequency + xOffset, y * config.baseFrequency + yOffset) * config.baseAmplitude;
        float detailNoise = Mathf.PerlinNoise(x * config.detailFrequency + xOffset, y * config.detailFrequency + yOffset) * config.detailAmplitude;
        return Mathf.Clamp01(baseNoise + detailNoise);
    }
    private TileBase GetAdvancedTile(int x, int y)
    {
        // 检查周围8个方向的瓦片存在情况
        bool hasTop = HasTile(x, y + 1);
        bool hasBottom = HasTile(x, y - 1);
        bool hasLeft = HasTile(x - 1, y);
        bool hasRight = HasTile(x + 1, y);
        // bool hasTopLeft = HasTile(x - 1, y + 1);
        // bool hasTopRight = HasTile(x + 1, y + 1);
        // bool hasBottomLeft = HasTile(x - 1, y - 1);
        // bool hasBottomRight = HasTile(x + 1, y - 1);
        
        // 九宫格匹配逻辑
        if (!hasTop)
        {
            if (!hasLeft && hasRight) return config.dirtTopLeft;
            if (!hasRight && hasLeft) return config.dirtTopRight;
            return config.dirtTop;
        }
    
        if (!hasBottom)
        {
            if (!hasLeft && hasRight) return config.dirtBottomLeft;
            if (!hasRight && hasLeft) return config.dirtBottomRight;
            return config.dirtBottom;
        }
    
        if (!hasLeft) return config.dirtLeft;
        if (!hasRight) return config.dirtRight;

        return null;
    }

    private bool HasTile(int x, int y)
    {
        // 检查坐标是否在地图边界内
        if (x < config.baseSizeLeft || x >= config.baseSizeRight || 
            y < config.baseSizeBottom || y >= config.baseSizeTop)
        {
            return false;
        }
    
        // 检查该位置是否有瓦片
        return groundTilemap.GetTile(new Vector3Int(x, y, 0)) != null;
    }
}