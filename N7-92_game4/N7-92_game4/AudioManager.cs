using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace N7_92_game4
{
    public class AudioManager : GameComponent
    {
        #region Private vars
        private ContentManager _content;

        private Dictionary<string, Song> _songs = new Dictionary<string,Song>();
        private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();

        private Song _currentSong = null;
        private SoundEffectInstance[] _playingSounds = new SoundEffectInstance[MaxSounds];

        private bool _isMusicPaused = false;
        private bool _disableSound = false;
        private const int MaxSounds = 10;
        public bool Enabled = true;

        // Replacing MediaPlayer values
        private MusicState _currentState;
        private enum MusicState { Stopped, Paused, Playing };


        #endregion

        #region Getters/Setters
        // Gets current song
        public string CurrentSong { get; private set; }

        // Gets or sets the music volume
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }
        // Gets or sets the sound volume
        public float SoundVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }
        public bool IsSongActive
        {
            get { return _currentSong != null && MediaPlayer.State != MediaState.Stopped; }
        }
        public bool IsSongPaused
        {
            get { return _currentSong != null && _isMusicPaused; }
        }
        public bool DisableSound
        {
            get { return _disableSound; }
            set { _disableSound = value; }
        }
        #endregion

        public AudioManager(Game game)
            : base(game)
        {
            _content = new ContentManager(game.Content.ServiceProvider, game.Content.RootDirectory);
        }

        public AudioManager(Game game, string contentFolder)
            : base(game)
        {
            _content = new ContentManager(game.Content.ServiceProvider, contentFolder);
        }

        public void LoadSong(string songName)
        {
            LoadSong(songName, songName);
        }

        public void LoadSong(string songName, string songPath)
        {
            if (_songs.ContainsKey(songName))
                throw new InvalidOperationException(string.Format("Song '{0}' has already been loaded", songName));
            _songs.Add(songName, _content.Load<Song>(songPath));
        }

        public void LoadSound(string soundName)
        {
            LoadSound(soundName, soundName);
        }

        public void LoadSound(string soundName, string soundPath)
        {
            if (_sounds.ContainsKey(soundName))
                throw new InvalidOperationException(string.Format("Sound '{0}' has already been loaded", soundName));
            _sounds.Add(soundName, _content.Load<SoundEffect>(soundPath));
        }

        // GameComponent doesn't have a unload content
        public void UnloadContent()
        {
            _content.Unload();
        }

        public void PlaySong(string songName, bool loop)
        {
            if (CurrentSong != songName)
            {
                if (_currentSong != null)
                    MediaPlayer.Stop();

                if (!_songs.TryGetValue(songName, out _currentSong))
                    throw new ArgumentException(string.Format("Song '{0}' not found", songName));

                CurrentSong = songName;

                _isMusicPaused = false;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(_currentSong);
                _currentState = MusicState.Playing;

                if (!Enabled)
                    MediaPlayer.Pause();
            }
        }

        public void PauseSong()
        {
            if (_currentSong != null && !_isMusicPaused)
            {
                if (Enabled)
                {
                    _currentState = MusicState.Paused;
                    MediaPlayer.Pause();
                }
                _isMusicPaused = true;
            }
        }

        public void ResumeSong()
        {
            if (_currentSong != null && _isMusicPaused)
            {
                if (Enabled)
                {
                    _currentState = MusicState.Playing;
                    MediaPlayer.Resume();
                }
                _isMusicPaused = false;
            }
        }

        public void StopSong()
        {
            if (_currentSong != null && _currentState != MusicState.Stopped)
            {                
                MediaPlayer.Stop();
                _currentState = MusicState.Stopped;
                _isMusicPaused = false;
            }
        }

        public void RestartSong()
        {
            bool loop = true;
            if (_currentSong != null)
            {
                loop = MediaPlayer.IsRepeating;
                MediaPlayer.Stop();
                _isMusicPaused = false;
            }
            PlaySong(CurrentSong, loop);
        }

        public void PlaySound(string soundName)
        {
            PlaySound(soundName, 1.0f, 0.0f, 0.0f);
        }

        public void PlaySound(string soundName, float volume)
        {
            PlaySound(soundName, volume, 0.0f, 0.0f);
        }

        public void PlaySound(string soundName, float volume, float pitch, float pan)
        {
            SoundEffect sound;

            if (!_sounds.TryGetValue(soundName, out sound))
                throw new ArgumentException(string.Format("Sound '{0}' not found", soundName));

            int index = GetAvailableSoundIndex();

            if(index != -1)
            {
                _playingSounds[index] = sound.CreateInstance();

                if(!Enabled)
                    _playingSounds[index].Pause();

                _playingSounds[index].Volume = volume;
                _playingSounds[index].Pitch = pitch;
                _playingSounds[index].Pan = pan;

                if(Enabled)
                    _playingSounds[index].Play();
            }
        }

        public void StopAllSounds()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] != null)
                {
                    _playingSounds[i].Stop();
                    _playingSounds[i].Dispose();
                    _playingSounds[i] = null;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (DisableSound)
            {
                if (Enabled)
                {
                    Enabled = false;
                }
                ChangeEnabled();
            }

            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Stopped)
                {
                    _playingSounds[i].Dispose();
                    _playingSounds[i] = null;
                }
            }

            if (_currentSong != null && _currentState == MusicState.Stopped)
            {
                _currentSong = null;
                CurrentSong = null;
                _isMusicPaused = false;
            }

            base.Update(gameTime);
        }

        // Pause all music and sound when disabled, resume when enabled
        public void ChangeEnabled()
        {
            if (Enabled)
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Paused)
                        _playingSounds[i].Resume();
                }

                if (!_isMusicPaused)
                    MediaPlayer.Resume();
            }
            else
            {
                for (int i = 0; i < _playingSounds.Length; ++i)
                {
                    if (_playingSounds[i] != null && _playingSounds[i].State == SoundState.Playing)
                        _playingSounds[i].Pause();
                }

                MediaPlayer.Pause();
            }
        }

        private int GetAvailableSoundIndex()
        {
            for (int i = 0; i < _playingSounds.Length; ++i)
            {
                if (_playingSounds[i] == null)
                    return i;
            }

            return -1;
        }

    }
}