using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesSpawner : MonoBehaviour
{
    [SerializeField] private normalTile normalTilePrefab;
    [SerializeField] private HoldTile holdTilePrefab;

    [SerializeField] private float spawnY = 6f;
    [SerializeField] private float[] xPositions = new float[] {-405f, -135f, 135f, 405f};

    public void Start()
    {
        PoolManager.Instance.CreatePool<normalTile>("normalTiles",normalTilePrefab,50);
        PoolManager.Instance.CreatePool<HoldTile>("holdTiles", holdTilePrefab,20);
    }

    public void SpawnTile(NoteData note)
    {          
        Vector3 pos = new Vector3(xPositions[note.lane], spawnY, 0f);
        if (note.type == NoteType.Normal)
            PoolManager.Instance.GetObject<normalTile>("normalTiles", pos, Quaternion.identity);
        if (note.type == NoteType.Hold)
        {
            pos.y += 1f;
            HoldTile holdTile = PoolManager.Instance.GetObject<HoldTile>("holdTiles", pos, Quaternion.identity);
            holdTile.Initialize(note);
        }
    }
}
