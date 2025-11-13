using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesSpawner : MonoBehaviour
{
    [SerializeField] private normalTile normalTilePrefab;

    [SerializeField] private float spawnY = 6f;
    [SerializeField] private float[] xPositions = new float[] {-2.25f, -0.75f, 0.75f, 2.25f};

    public void Awake()
    {
        PoolManager.Instance.CreatePool<normalTile>("normalTiles",normalTilePrefab,50);
    }

    public void SpawnTile(int laneIndex)
    {
        Vector3 pos = new Vector3(xPositions[laneIndex], spawnY, 0f);
        PoolManager.Instance.GetObject<normalTile>("normalTiles", pos, Quaternion.identity);
    }
}
