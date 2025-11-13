using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public enum NoteType
{
    Normal,
    Hold
}

[Serializable]
public class NoteData
{
    public NoteType type;
    public int lane;
    public float time;
    public float duration;
}

public class BeatmapReader : MonoBehaviour
{
    public event Action<NoteData> OnNoteSpawn;

    private List<NoteData> notes = new List<NoteData>();
    private AudioSource musicSource;

    private void Awake()
    {
        musicSource = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
    }

    public void LoadBeatmap(string songName, Action onComplete)
    {
        notes.Clear();

        string fileName = songName + ".txt";
        string path = Path.Combine(Application.streamingAssetsPath, "Beatmaps", fileName);

#if UNITY_ANDROID && !UNITY_EDITOR
    StartCoroutine(LoadFromAndroid(path, onComplete)); // ✅ truyền callback
#else
        if (!File.Exists(path))
        {
            Debug.LogError("Không tìm thấy beatmap: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);
        ParseLines(lines);
        StartCoroutine(PlayNotes(onComplete));
#endif
    }

    private IEnumerator LoadFromAndroid(string path, Action onComplete)
    {
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Lỗi đọc beatmap: " + request.error);
            yield break;
        }

        string[] lines = request.downloadHandler.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        ParseLines(lines);
        StartCoroutine(PlayNotes(onComplete));
    }

    private void ParseLines(string[] lines)
    {
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Trim().Split(',');

            if (parts.Length < 2) continue;

            NoteData note = new NoteData();
            note.time = float.Parse(parts[0]);
            note.lane = ConvertXToLane(float.Parse(parts[1]));

            if (parts.Length == 3)
            {
                note.type = NoteType.Hold;
                float endTime = float.Parse(parts[2]);
                note.duration = Mathf.Max(0f, endTime - note.time);
            }
            else
            {
                note.type = NoteType.Normal;
                note.duration = 0f;
            }

            notes.Add(note);
        }
    }

    private IEnumerator PlayNotes(Action onComplete)
    {
        onComplete?.Invoke();
        foreach (var note in notes)
        {
            while (musicSource.time < note.time)
                yield return null;

            OnNoteSpawn?.Invoke(note);
        }
    }

    private int ConvertXToLane(float x)
    {
        if (Mathf.Approximately(x, -2.25f)) return 0;
        if (Mathf.Approximately(x, -0.75f)) return 1;
        if (Mathf.Approximately(x, 0.75f)) return 2;
        if (Mathf.Approximately(x, 2.25f)) return 3;
        return 1;
    }
}