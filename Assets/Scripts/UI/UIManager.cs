using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    private Coroutine scoreTextCoroutine;
    [Header("Combo Display")]
    [SerializeField] private TextMeshProUGUI comboText;
    private int combo = 0;
    private Coroutine comboCoroutine;
    [Header("Star Slider Display")]
    [SerializeField] private Slider starSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject[] yellowStarIcons;
    [SerializeField] private GameObject[] whiteStarIcons;
    [SerializeField] private Color glowColor1 = new Color(1f, 0.85f, 0.3f);
    [SerializeField] private Color glowColor2 = new Color(1f, 1f, 0.6f);
    [SerializeField] private float glowSpeed = 3f;
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
    private void Start()
    {
        starSlider.value = 0f;
        foreach (var star in yellowStarIcons)
            star.SetActive(false);
        foreach (var star in whiteStarIcons)
            star.SetActive(true);
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f;
        fillImage.color = Color.Lerp(glowColor1, glowColor2, t);
    }
    #region Score & Combo Updates
    public void UpdateScore(int amount)
    {
        score = amount;
        scoreText.text = score.ToString();

        if (scoreTextCoroutine != null)
            StopCoroutine(scoreTextCoroutine);

        scoreTextCoroutine = StartCoroutine(AnimateScale(scoreText));
    }

    public void UpdateCombo(int amount)
    {
        combo = amount;
        comboText.text = combo >= 1 ? "x" + combo.ToString() : "";
        if (comboCoroutine != null)
                StopCoroutine(comboCoroutine);
            comboCoroutine = StartCoroutine(AnimateScale(comboText));        
    }
    private IEnumerator AnimateScale(TextMeshProUGUI targetText)
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
    #endregion

    #region Feedback Update
    public void ShowFeedback(string message, HitResult result)
    {        
        if (feedbackCoroutine != null)
            StopCoroutine(feedbackCoroutine);

        feedbackCoroutine = StartCoroutine(ShowFeedbackRoutine(message,result));
    }

    private IEnumerator ShowFeedbackRoutine(string message, HitResult result)
    {       
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
    #endregion
    #region Star Slider
    public void UpdateStarSlider(HitResult result)
    {
        float increment = result switch
        {
            HitResult.Perfect => 0.05f,
            HitResult.Great => 0.03f,
            HitResult.Good => 0.01f,
            _ => 0f
        };

        starSlider.value = Mathf.Clamp01(starSlider.value + increment);
        UpdateStarIcons();
    }
    private void UpdateStarIcons()
    {
        float value = starSlider.value;

        bool star1Active = value >= 0.3f;
        yellowStarIcons[0].SetActive(star1Active);
        whiteStarIcons[0].SetActive(!star1Active);

        bool star2Active = value >= 0.66f;
        yellowStarIcons[1].SetActive(star2Active);
        whiteStarIcons[1].SetActive(!star2Active);

        bool star3Active = value >= 1f;
        yellowStarIcons[2].SetActive(star3Active);
        whiteStarIcons[2].SetActive(!star3Active);
    }
    #endregion
}