using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalTile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private bool isHit = false;

    private void Update()
    {
        if (!isHit)        
           transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y < -10f)
            PoolManager.Instance.ReturnObject("normalTiles", this);
    }

    public void OnHit()
    {
        isHit = true;
        //add score logic or effects here
        PoolManager.Instance.ReturnObject("normalTiles", this);
    }
}
