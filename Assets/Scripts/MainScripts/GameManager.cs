using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private TilesSpawner tilesSpawner;
    [SerializeField] private BeatmapReader beatmapReader;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        musicSource.Play();
        beatmapReader.LoadBeatmap();
        beatmapReader.OnNoteSpawn += tilesSpawner.SpawnTile;
    }

    public void OnTileHit(HitResult result)
    {
        scoreManager.AddScore(result);
        uiManager.UpdateScore(scoreManager.CurrentScore);
        uiManager.ShowFeedback(result.ToString(),result);
    }

    public void OnHoldCompleted()
    {
        //scoreManager.AddHoldBonus();
        //uiManager.ShowFeedback("Hold Bonus!");
    }

    public void TriggerGameOver()
    {        
        musicSource.Stop();
        SceneManager.LoadScene("GameOverScene");
        Debug.Log("Game Over triggered in GameManager.");        
    }
}
