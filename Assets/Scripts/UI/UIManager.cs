using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    private Coroutine scoreTextCoroutine;

    [Header("Feedback Display")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float feedbackDuration = 0.7f;
    private Coroutine feedbackCoroutine;
    [Header("Gradient Presets")]
    [SerializeField] private TMP_ColorGradient perfectGradient;
    [SerializeField] private TMP_ColorGradient greatGradient;
    [SerializeField] private TMP_ColorGradient goodGradient;
    [SerializeField] private TMP_ColorGradient missGradient;

    [Header("Scale Animation")]
    [SerializeField] private float scaleUpFactor = 1.5f;
    [SerializeField] private float scaleUpDuration = 0.3f;

    public void UpdateScore(int amount)
    {
        score = amount;
        scoreText.text = score.ToString();

        if (scoreTextCoroutine != null)
            StopCoroutine(scoreTextCoroutine);

        scoreTextCoroutine = StartCoroutine(ScoreAnimateScale(scoreText));
    }

    public void ShowFeedback(string message, HitResult result)
    {        
        if (feedbackCoroutine != null)
            StopCoroutine(feedbackCoroutine);

        feedbackCoroutine = StartCoroutine(ShowFeedbackRoutine(message,result));
    }

    private IEnumerator ShowFeedbackRoutine(string message, HitResult result)
    {
        
        Debug.Log("Start feedback animation");

        feedbackText.text = message;    
        feedbackText.colorGradientPreset = GetPresetByHitResult(result);
        Vector3 originalScale = feedbackText.transform.localScale;
        Vector3 enlargedScale = originalScale * scaleUpFactor;
        feedbackText.transform.localScale = enlargedScale;

        float time = 0f;
        while (time < scaleUpDuration)
        {
            time += Time.deltaTime;
            float t = time / scaleUpDuration;
            feedbackText.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, t);
            yield return null;
        }     
        feedbackText.transform.localScale = originalScale;       
        float remaining = Mathf.Max(0f, feedbackDuration - scaleUpDuration);
        yield return new WaitForSeconds(remaining);
        feedbackText.text = "";    
    }
    private TMP_ColorGradient GetPresetByHitResult(HitResult result)
    {
        return result switch
        {
            HitResult.Perfect => perfectGradient,
            HitResult.Great => greatGradient,
            HitResult.Good => goodGradient,
            HitResult.Miss => missGradient,
            _ => null
        };
    }

    private IEnumerator ScoreAnimateScale(TextMeshProUGUI targetText)
    {
        Vector3 originalScale = targetText.transform.localScale;
        Vector3 enlargedScale = originalScale * scaleUpFactor;
        targetText.transform.localScale = enlargedScale;

        float time = 0f;
        while (time < scaleUpDuration)
        {
            time += Time.deltaTime;
            float t = time / scaleUpDuration;
            targetText.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, t);
            yield return null;
        }

        targetText.transform.localScale = originalScale;
    }
}