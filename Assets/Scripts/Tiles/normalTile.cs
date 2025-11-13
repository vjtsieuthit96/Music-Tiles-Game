using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalTile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float targetY = -4.5f;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private SpriteRenderer spriteRenderer;    
    private bool isHit = false;

    private void Update()
    {
        if (!isHit)        
           transform.Translate(Vector3.down * speed * Time.deltaTime);
        //if (transform.position.y < targetY - 1.5f && !isHit)
        //{
        //    PoolManager.Instance.ReturnObject("normalTiles", this);
        //    FindObjectOfType<GameManager>().TriggerGameOver();
        //}
    }
    private void Start()
    {
        
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
        hitEffect.Play();
        StartCoroutine(FadeAndShrinkThenReturn());
    }
    private IEnumerator FadeAndShrinkThenReturn()
    {
        float duration = 0.3f;
        float elapsed = 0f;

        Color startColor = spriteRenderer.color;
        Vector3 startScale = transform.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;           
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = newColor;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        transform.localScale = Vector3.zero;

        PoolManager.Instance.ReturnObject("normalTiles", this);
    }
    private void OnEnable()
    {
        isHit = false;
        transform.localScale = new Vector3(0.5f,0.6f);

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
    }
}
