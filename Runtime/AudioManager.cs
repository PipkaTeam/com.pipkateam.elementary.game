using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Elementary.Game
{
    public enum AudioState
    {
        Unmuted,
        Muted
    }

    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField]
        protected AudioMixer audioMixer;

        [Space]

        [SerializeField]
        protected const string soundsVolumeKey = "Sounds";

        [SerializeField]
        protected const string musicVolumeKey = "Music";

        [Space]

        [SerializeField]
        protected AudioSource soundsAudioSource;

        [SerializeField]
        protected AudioSource musicAudioSource;

        private AudioState soundsAudioState;
        private AudioState musicAudioState;

        public Action<AudioState> OnSoundsAudioStateChanged;
        public Action<AudioState> OnMusicAudioStateChanged;

        protected override void Start()
        {
            base.Start();

            if (audioMixer == null)
            {
                Debug.LogError("[Elementary Game][AudioManager] AudioMixer is not assigned.");
                return;
            }

            if (soundsAudioSource == null)
            {
                Debug.LogError("[Elementary Game][AudioManager] Sounds AudioSource is not assigned.");
                return;
            }

            if (musicAudioSource == null)
            {
                Debug.LogError("[Elementary Game][AudioManager] Music AudioSource is not assigned.");
                return;
            }

            SaveManager saveManager = SaveManager.Instance;

            if (saveManager != null)
            {
                AudioState newSoundsAudioState = saveManager.LoadSounds();
                SetSoundsAudioState(newSoundsAudioState);

                AudioState newMusicAudioState = saveManager.LoadMusic();
                SetMusicAudioState(newMusicAudioState);
            }
            else
            {
                Debug.LogWarning("[Elementary Game][AudioManager] SaveManager.Instance is null, cannot load audio settings.");
            }

            Game.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newGaneState)
        {
            switch (newGaneState)
            {
                case GameState.Playing:

                    UnPauseMusic();

                    break;

                case GameState.Paused:

                    PauseMusic();

                    break;
            }
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("[Elementary Game][AudioManager] Attempted to play a null AudioClip.");
                return;
            }

            soundsAudioSource.PlayOneShot(clip);
            Debug.Log("[Elementary Game][AudioManager] Sound played.");
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("[Elementary Game][AudioManager] Attempted to play a null AudioClip.");
                return;
            }

            StopMusic();
            musicAudioSource.clip = clip;
            musicAudioSource.Play();
            Debug.Log("[Elementary Game][AudioManager] Music started.");
        }

        public void PauseMusic()
        {
            if (musicAudioSource.clip == null)
            {
                Debug.LogWarning("[Elementary Game][AudioManager] Attempted to pause music, but no music is playing.");
                return;
            }

            musicAudioSource.Pause();
            Debug.Log("[Elementary Game][AudioManager] Music paused.");
        }

        public void UnPauseMusic()
        {
            if (musicAudioSource.clip == null)
            {
                Debug.LogWarning("[Elementary Game][AudioManager] Attempted to unpause music, but no music is playing.");
                return;
            }

            musicAudioSource.UnPause();
            Debug.Log("[Elementary Game][AudioManager] Music unpaused.");
        }

        public void StopMusic()
        {
            if (musicAudioSource.clip == null)
            {
                Debug.LogWarning("[Elementary Game][AudioManager] Attempted to stop music, but no music is playing.");
                return;
            }

            musicAudioSource.Stop();
            Debug.Log("[Elementary Game][AudioManager] Music stopped.");
        }

        public AudioState GetSoundsAudioState()
        {
            return soundsAudioState;
        }

        public void SetSoundsAudioState(AudioState newAudioState)
        {
            if (audioMixer == null)
            {
                Debug.LogError("[Elementary Game][AudioManager] AudioMixer is not assigned.");
                return;
            }

            switch (newAudioState)
            {
                case AudioState.Unmuted:

                    audioMixer.SetFloat(soundsVolumeKey, 0f);
                    SaveManager.Instance?.SaveSounds(AudioState.Unmuted);
                    soundsAudioState = AudioState.Unmuted;

                    Debug.Log("[Elementary Game][AudioManager] Sounds unmuted.");

                    break;

                case AudioState.Muted:

                    audioMixer.SetFloat(soundsVolumeKey, -80f);
                    SaveManager.Instance?.SaveSounds(AudioState.Muted);
                    soundsAudioState = AudioState.Muted;

                    Debug.Log("[Elementary Game][AudioManager] Sounds muted.");

                    break;
            }

            OnSoundsAudioStateChanged?.Invoke(newAudioState);
        }

        public AudioState GetMusicAudioState()
        {
            return musicAudioState;
        }

        public void SetMusicAudioState(AudioState newAudioState)
        {
            if (audioMixer == null)
            {
                Debug.LogError("[Elementary Game][AudioManager] AudioMixer is not assigned.");
                return;
            }

            switch (newAudioState)
            {
                case AudioState.Unmuted:

                    audioMixer.SetFloat(musicVolumeKey, 0f);
                    SaveManager.Instance?.SaveMusic(AudioState.Unmuted);
                    musicAudioState = AudioState.Unmuted;

                    Debug.Log("[Elementary Game][AudioManager] Music unmuted.");

                    break;

                case AudioState.Muted:

                    audioMixer.SetFloat(musicVolumeKey, -80f);
                    SaveManager.Instance?.SaveMusic(AudioState.Muted);
                    musicAudioState = AudioState.Muted;

                    Debug.Log("[Elementary Game][AudioManager] Music muted.");

                    break;
            }

            OnMusicAudioStateChanged?.Invoke(newAudioState);
        }
    }
}