🛠️ How to run the Project
- Select the aspect ratio: Full HD (Portrait) for the game window.
<img width="207" height="225" alt="image" src="https://github.com/user-attachments/assets/ac0934e1-03d4-4cbd-99e1-e5c8dd1b70da" />

- Open the scene named "MainGameScene" located at Assets/Scenes/MainGameScene.
- Select a song from the dropdown list.
- Press Play to start the game.
- When you lose, the screen will switch to the GameOver scene, press PlayAgain to restart.

You can create a beatmap using the "BeatMapMaker" scene.
  - First, import a new music file into Assets/Resources/Musics.
  - After creating the beatmap, the file will be saved in Assets as songName.txt.
  - Move this file to Assets/StreamingAssets/Beatmaps to load the new beatmap.
To add a new song, make sure:
- The song has a corresponding beatmap file.
- The song name must be added to Hierarchy > Canvas > MusicList > SongNames.


🎨 Design Choices:
- Song selection via TMP_Dropdown: Allows players to easily choose from available songs.
- Separate AudioSources for preview and gameplay: Preview music plays when selecting a song. Once Play is pressed, the clip is passed to GameManager for synchronized beatmap playback.
- Scoring based on combos enhances the sense of achievement, thereby creating a fresh and engaging experience for the player.
- Beatmap loading from StreamingAssets: Beatmaps are stored as .txt files in StreamingAssets/Beatmaps, making them easy to update and compatible with Android builds.
- Note spawning via event system: BeatmapReader triggers OnNoteSpawn at the correct time, separating data parsing from gameplay rendering.
- Callback after beatmap loading: Ensures the game only starts when all data is ready, preventing sync issues between music and notes.
- Tile reuse via PoolManager: Improves performance and reduces overhead by reusing note objects.
- Modular architecture: Components like MusicListManager, GameManager, and BeatmapReader are separated for better maintainability and scalability.

📦 Asset Attribution

This project uses the following external assets:
- JMO Assets
Downloaded from the Unity Asset Store for tap effect.
(https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565?srsltid=AfmBOooFc4kyCP5lK1bzCshz0BamQhcC7R6WM-rN-qx9Q2CQIxiXIbk8)
- Images.
Some UI and background images were sourced from Google & AI generated. These are used for non-commercial, educational purposes only.
- Sound Effects
Downloaded from Pixabay — all sound effects are royalty-free.
- Music Tracks
Downloaded from Nhaccuatui.com for demo purposes only.
