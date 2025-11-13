using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // tốc độ nhấp nháy
    [SerializeField] private float minAlpha = 0.3f;
    [SerializeField] private float maxAlpha = 1f;

    [SerializeField]private SpriteRenderer decorIM;
    private float time;
    
    private void Update()
    {
        time += Time.deltaTime * speed;

        // Tạo giá trị alpha dao động giữa min và max
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(time) + 1f) / 2f);

        Color c = decorIM.color;
        c.a = alpha;
        decorIM.color = c;
    }
}