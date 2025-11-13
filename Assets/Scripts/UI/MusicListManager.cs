using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MusicListManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string[] songNames;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BeatmapReader beatmapReader;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject[] musicNotes;

    private string selectedSongName;
    private AudioClip selectedClip;

    private void Start()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(songNames));
        OnSongSelected(0);
    }

    public void OnSongSelected(int index)
    {
        if (index < 0 || index >= songNames.Length)
        {
            Debug.LogError("Index bài hát không hợp lệ: " + index);
            return;
        }

        selectedSongName = songNames[index];
        selectedClip = Resources.Load<AudioClip>("Musics/" + selectedSongName);

        if (selectedClip != null)
        {
            audioSource.Stop();
            audioSource.clip = selectedClip;
            audioSource.Play();
            Debug.Log("Đang phát bài: " + selectedSongName);
        }
        else
        {
            Debug.LogError("Không tìm thấy bài hát: " + selectedSongName);
        }
    }

    public void OnPlayButtonClicked()
    {
        audioSource.Stop();

        if (selectedClip == null || string.IsNullOrEmpty(selectedSongName))
        {
            Debug.LogError("Chưa chọn bài hát hợp lệ.");
            return;
        }

        gameManager.SetAudioClip(selectedClip);

        beatmapReader.LoadBeatmap(selectedSongName, () =>
        {
            foreach (var note in musicNotes)
            {
                note.SetActive(true);
            }            
            gameManager.PlayMusic();
            panel.SetActive(false);
        });
    }
}