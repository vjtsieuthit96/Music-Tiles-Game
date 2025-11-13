using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalTile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float targetY = -4.5f;
    private bool isHit = false;

    private void Update()
    {
        if (!isHit)        
           transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y < targetY - 1.5f && !isHit)
        {
            PoolManager.Instance.ReturnObject("normalTiles", this);
            FindObjectOfType<GameManager>().TriggerGameOver();
        }
    }

    public void OnHit()
    {
        if (isHit) return;
        isHit = true;
        //add score logic or effects here
        float distance = Mathf.Abs(transform.position.y - targetY);
        HitResult result = distance switch
        {
            <= 0.6f => HitResult.Perfect,
            <= 1.6f => HitResult.Great,
            <= 2.5f => HitResult.Good,
            _ => HitResult.Miss
        };        
        FindObjectOfType<GameManager>().OnTileHit(result);
        PoolManager.Instance.ReturnObject("normalTiles", this);
    }
}
