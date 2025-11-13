using UnityEngine;

public class HoldTile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float targetY = -4.5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject holdEffect;
    [SerializeField] private AudioSource holdSound;
    private float holdDuration;
    private float holdTimer;
    private bool isHolding = false;
    private bool isCompleted = false;
    private bool isMissed = false;
    private HitResult clickResult;

    private float initialScaleY;

    public void Initialize(NoteData note)
    {
        holdDuration = note.duration;
        initialScaleY = transform.localScale.y;       
    }

    private void Update()
    {
        if (isCompleted || isMissed) return;

        if (!isHolding)        
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        

        if (transform.position.y < targetY - 1.5f && !isHolding)
        {
            PoolManager.Instance.ReturnObject("normalTiles", this);
            FindObjectOfType<GameManager>().TriggerGameOver();            
        }

        if (isHolding)
        {
            holdTimer += Time.deltaTime;

            float progress = Mathf.Clamp01(holdTimer / holdDuration);
            float newScaleY = Mathf.Lerp(initialScaleY, 0f, progress);
            transform.localScale = new Vector3(transform.localScale.x, newScaleY, transform.localScale.z);

            if (holdTimer >= holdDuration)
            {
                CompleteHold();
            }
        }
    }

    public void OnHoldStart()
    {
        if (isCompleted || isMissed) return;
        isHolding = true;
        holdSound.Play();
        holdEffect.SetActive(true);
        float distance = Mathf.Abs(transform.position.y - targetY);
        HitResult result = distance switch
        {
            <= 0.6f => HitResult.Perfect,
            <= 1.6f => HitResult.Great,
            <= 2.5f => HitResult.Good,
            _ => HitResult.Miss
        };
        clickResult = result;        
        holdTimer = 0f;
    }

    public void OnHoldEnd()
    {
        if (isCompleted || isMissed) return;

        if (holdTimer >= holdDuration * 0.8f)
            CompleteHold();
        else
            MissHold();
    }

    private void CompleteHold()
    {
        isCompleted = true;
        holdSound.Stop();
        FindObjectOfType<GameManager>().OnTileHit(clickResult);
        PoolManager.Instance.ReturnObject("holdTiles", this);
    }

    private void MissHold()
    {
        isMissed = true;
        holdSound.Stop();
        holdEffect.SetActive(false);
        FindObjectOfType<GameManager>().OnTileHit(HitResult.Miss);
        PoolManager.Instance.ReturnObject("holdTiles", this);
    }

    private void OnEnable()
    {
        isHolding = false;
        isCompleted = false;
        isMissed = false;
        holdTimer = 0f;
        holdEffect.SetActive(false);

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