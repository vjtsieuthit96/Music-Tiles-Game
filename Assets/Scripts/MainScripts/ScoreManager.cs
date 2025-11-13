using UnityEngine;
public enum HitResult
{
    Perfect,
    Great,
    Good,
    Miss
}

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private int combo = 0;
    [SerializeField] private int missCount = 0;
    [SerializeField] private int maxMissAllowed = 5;

    private HitResult? lastHit = null;

    public int CurrentScore => score;
    public int CurrentCombo => combo;
    public int CurrentMiss => missCount;

    public void AddScore(HitResult result)
    {
        if (result == HitResult.Miss)
        {
            combo = 0;
            missCount++;

            if (missCount >= maxMissAllowed)
                FindObjectOfType<GameManager>().TriggerGameOver();

            return;
        }
        
        combo = (lastHit == null || lastHit != result) ? 0 : combo + 1;
        
        lastHit = result;

        int baseScore = result switch
        {
            HitResult.Perfect => 5,
            HitResult.Great => 3,
            HitResult.Good => 2,
            _ => 0
        };

        score += baseScore + combo;
    }

    //public void AddHoldBonus()
    //{
    //    combo++;
    //    score += 5 + (combo * 10);
    //    lastHit = HitResult.Perfect;
    //}
}