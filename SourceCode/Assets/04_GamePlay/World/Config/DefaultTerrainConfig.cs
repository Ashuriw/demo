using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="DeflautTerrainConfig",menuName = "Assets/04_GamePlay/World/Config")]
public class DefaultTerrainConfig : ScriptableObject
{
    [Header("瓦片")]
    public TileBase[] dirts;
    [Header("边缘瓦片")]
    public TileBase dirtTop;      // 上边缘
    public TileBase dirtBottom;   // 下边缘  
    public TileBase dirtLeft;     // 左边缘
    public TileBase dirtRight;    // 右边缘
    [Header("斜角瓦片")]
    public TileBase dirtTopLeft;      // 左上斜角
    public TileBase dirtTopRight;     // 右上斜角
    public TileBase dirtBottomLeft;   // 左下斜角
    public TileBase dirtBottomRight;  // 右下斜角

    [Header("地图参数")]
    [Tooltip("基础地形左边界")]
    public int baseSizeLeft = -100;
    [Tooltip("基础地形右边界")]
    public int baseSizeRight = 100;
    [Tooltip("基础地形上边界")]
    public int baseSizeTop = -1;
    [Tooltip("基础地形下边界")]
    public int baseSizeBottom = -100;
    
    [Header("地表面参数")]
    [Tooltip("地表起伏系数")]
    public float surfaceHeightVariation = 10f;
    [Tooltip("地表起伏频率")]
    public float surfaceFrequency = 0.05f;
    
    [Header("基础地形参数")]
    [Tooltip("大尺度perlin噪声")]
    public float baseFrequency = 0.02f;
    [Tooltip("大尺度perlin噪声权重")]
    public float baseAmplitude = 1.0f;
    [Tooltip("小尺度perlin噪声")]
    public float detailFrequency = 0.1f;
    [Tooltip("小尺度perlin噪声权重")]
    public float detailAmplitude = 0.3f;
    [Tooltip("泥土阈值，表示大概有dirtThreshold的瓦片需要放置泥土")]
    [Range(0,1f)]public float dirtThreshold = 0.7f;
    // [Tooltip("泥土阈值，表示大概有stoneThreshold-dirtThreshold的瓦片需要放置岩石")]
    // [Range(0,1f)]public float stoneThreshold = 0.7f;
    [Tooltip("perlin噪声取值的偏移值范围")]
    public float randomMin = 0;
    [Tooltip("perlin噪声取值的偏移值范围")]
    public float randomMax = 100f;
    [Tooltip("分块大小")]
    public int chunkSize = 32;
    [Tooltip("每处理多少瓦片后等待一帧防止卡顿")]
    public int updateCount = 16;
    [Tooltip("以区块为单位，加载玩家周围多大范围内的区块")]
    public int loadRadius = 2;
}
