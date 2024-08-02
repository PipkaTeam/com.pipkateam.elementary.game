using UnityEngine;

namespace Elementary.Game
{
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField]
        protected const string soundsKey = "Sounds";

        [SerializeField]
        protected const string musicKey = "Music";

        public AudioState LoadSounds()
        {
            int sounds = PlayerPrefs.GetInt(soundsKey, 1);
            AudioState audioState = sounds == 0 ? AudioState.Muted : AudioState.Unmuted;
            Debug.Log($"[Elementary Game][SaveManager] Loaded Sounds state: {audioState}");
            return audioState;
        }

        public void SaveSounds(AudioState newAudioState)
        {
            int sounds = newAudioState == AudioState.Unmuted ? 1 : 0;
            PlayerPrefs.SetInt(soundsKey, sounds);
            PlayerPrefs.Save();
            Debug.Log($"[Elementary Game][SaveManager] Saved Sounds state: {newAudioState}");
        }

        public AudioState LoadMusic()
        {
            int music = PlayerPrefs.GetInt(musicKey, 1);
            AudioState audioState = music == 0 ? AudioState.Muted : AudioState.Unmuted;
            Debug.Log($"[Elementary Game][SaveManager] Loaded Music state: {audioState}");
            return audioState;
        }

        public void SaveMusic(AudioState newAudioState)
        {
            int music = newAudioState == AudioState.Unmuted ? 1 : 0;
            PlayerPrefs.SetInt(musicKey, music);
            PlayerPrefs.Save();
            Debug.Log($"[Elementary Game][SaveManager] Saved Music state: {newAudioState}");
        }
    }
}