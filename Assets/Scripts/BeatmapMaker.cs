using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BeatmapMaker : MonoBehaviour
{
    public AudioSource music;

    private List<string> beatLines = new List<string>();
    private readonly float[] xPositions = new float[] { -2.25f, -0.75f, 0.75f, 2.25f };

    private bool isHolding = false;
    private float holdStartTime;
    private float holdX;

    void Update()
    {
        // Space → ghi 1 tile thường
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float time = music.time;
            float x = xPositions[Random.Range(0, xPositions.Length)];
            beatLines.Add($"{time:F2},{x:F2}");
            Debug.Log($"Ghi 1 tile: {time:F2}, {x:F2}");
        }

        // Click chuột → ghi 2 tile thường cùng lúc
        if (Input.GetMouseButtonDown(0))
        {
            float time = music.time;
            var shuffled = xPositions.OrderBy(x => Random.value).ToList();
            beatLines.Add($"{time:F2},{shuffled[0]:F2}");
            beatLines.Add($"{time:F2},{shuffled[1]:F2}");
            Debug.Log($"Ghi 2 tile: {time:F2}, {shuffled[0]:F2} & {shuffled[1]:F2}");
        }

        // Giữ H → bắt đầu tile hold
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHolding = true;
            holdStartTime = music.time;
            holdX = xPositions[Random.Range(0, xPositions.Length)];
            Debug.Log($"Bắt đầu HOLD: {holdStartTime:F2}, {holdX:F2}");
        }

        // Nhả H → kết thúc tile hold
        if (Input.GetKeyUp(KeyCode.H) && isHolding)
        {
            float holdEndTime = music.time;
            beatLines.Add($"{holdStartTime:F2},{holdX:F2},{holdEndTime:F2}");
            Debug.Log($"Kết thúc HOLD: {holdStartTime:F2}, {holdX:F2}, {holdEndTime:F2}");
            isHolding = false;
        }

        // Esc → lưu file
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string clipName = music.clip != null ? music.clip.name : "beatmap";
            string outputFileName = clipName + ".txt";
            string path = Path.Combine(Application.dataPath, outputFileName);
            File.WriteAllLines(path, beatLines);
            Debug.Log("Beatmap đã lưu tại: " + path);
        }
    }
}