using System;
using System.Collections;
using UnityEngine;

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
    [SerializeField] private NoteData[] notes;
    public event Action<NoteData> OnNoteSpawn;

    public void LoadBeatmap()
    {
        StartCoroutine(PlayNotes());
    }
    private IEnumerator PlayNotes()
    {
        foreach (var note in notes)
        {
            yield return new WaitForSeconds(note.time);
            OnNoteSpawn?.Invoke(note);
        }
    }
}
