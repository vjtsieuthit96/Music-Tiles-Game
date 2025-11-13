using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private TilesSpawner tilesSpawner;  
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BeatmapReader beatmapReader;
    public bool IsGameOver = false;
    [Header("Feedback Sounds")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip perfectClip;
    [SerializeField] private AudioClip greatClip;
    [SerializeField] private AudioClip goodClip;
    [SerializeField] private AudioClip missClip;
    [SerializeField] private AudioClip gameOver;

    private void Start()
    {
        beatmapReader.OnNoteSpawn += tilesSpawner.SpawnTile;
    }
    public void SetAudioClip(AudioClip clip)
    {
        musicSource.clip = clip;
    }
    public void PlayMusic()
    {
        musicSource.Play();        
    }


    public void OnTileHit(HitResult result)
    {
        scoreManager.AddScore(result);
        uiManager.UpdateScore(scoreManager.CurrentScore);
        uiManager.UpdateCombo(scoreManager.CurrentCombo);
        uiManager.UpdateStarSlider(result);
        uiManager.ShowFeedback(result.ToString(), result);

        AudioClip clip = GetClipByHitResult(result);
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    private AudioClip GetClipByHitResult(HitResult result)
    {
        return result switch
        {
            HitResult.Perfect => perfectClip,
            HitResult.Great => greatClip,
            HitResult.Good => goodClip,
            HitResult.Miss => missClip,
            _ => null
        };
    }

    public void TriggerGameOver()
    {
        if (!IsGameOver)
        {
            StartCoroutine(GameOverRoutine());
            sfxSource.PlayOneShot(gameOver);
            musicSource.Stop();
        }
        
    }
    private IEnumerator GameOverRoutine()
    {
        IsGameOver = true;
        yield return new WaitForSeconds(1.5f);
        IsGameOver = false;
        SceneManager.LoadScene("GameOverScene");
    }
}
