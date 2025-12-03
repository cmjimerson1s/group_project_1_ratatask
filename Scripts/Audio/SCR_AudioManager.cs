using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SCR_AudioManager : MonoBehaviour {


    [Range(0f,4f)]
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SCR_SoundsCollectionSO _soundCollectionSO;

    [SerializeField] private AudioMixerGroup _sfxAudioMixerGroup;
    [SerializeField] private AudioMixerGroup _musicAudioMixerGroup;
    [SerializeField] private AudioMixerGroup _ambianceAudioMixerGroup;

    [SerializeField] private SCR_PlayerRayLight _playerRayLight;
    
    private AudioSource[] _audioSources;
    private AudioSource _currentMusic;
    private SCR_Player_Movement _playerMovement;
    private SCR_PlayerHitEnemy _playerHitEnemy;
    private SCR_DayNightCycle _dayNightCycle;
    private SCR_SceneManager _sceneManager;

    
    #region Unity Methods

    private void Awake() {
        _dayNightCycle = FindFirstObjectByType<SCR_DayNightCycle>();
        
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            _playerMovement = FindFirstObjectByType<SCR_Player_Movement>();
            _sceneManager = FindFirstObjectByType<SCR_SceneManager>();
            _playerHitEnemy = FindFirstObjectByType<SCR_PlayerHitEnemy>();
        }
        
    }

    private void Start() {
        // Put the method for the starting music here
        
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            PlayForrestMusic();
            PlayWindMusic();
            PlayDayMusic();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0) {
            PlayMainMenuMusic();
        }

    }
    

    private void OnEnable() {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            _playerMovement.onPlayerBump += OnHitTreeSFX;
            _playerMovement.onPlayerLeftDash += OnDash;
            _playerMovement.onPlayerRightDash += OnDash;
            _sceneManager.OnNutPickup += OnPickUpNutSFX;
            _sceneManager.OnRayPickup += OnPickUpNutSFX;
            _sceneManager.OnShieldPickup += OnPickUpNutSFX;
            _playerHitEnemy.OnEnemyHitByPlayer += OnCollisionEvil;
            _playerRayLight.OnEnemyHit += OnHitEvilSFX;
            _playerRayLight.OnTreeHit += OnKillTreeSFX;
        }

        _dayNightCycle.OnNightTime += PlayNightMusic;
        _dayNightCycle.OnGameEnd += PlayEndMusic;
    }



    private void OnDisable() {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            _playerMovement.onPlayerBump -= OnHitTreeSFX;
            _playerMovement.onPlayerLeftDash -= OnDash;
            _playerMovement.onPlayerRightDash -= OnDash;
            _sceneManager.OnNutPickup -= OnPickUpNutSFX;
            _sceneManager.OnRayPickup -= OnPickUpNutSFX;
            _sceneManager.OnShieldPickup -= OnPickUpNutSFX;
            _playerHitEnemy.OnEnemyHitByPlayer -= OnCollisionEvil;
            _playerRayLight.OnEnemyHit -= OnHitEvilSFX;
            _playerRayLight.OnTreeHit -= OnKillTreeSFX;
        }

        _dayNightCycle.OnNightTime -= PlayNightMusic;
        _dayNightCycle.OnGameEnd -= PlayEndMusic;
    }


    #endregion

    #region Sound Methods

    // All sound methods here that changes the audio mixer to be the correct one depending on which one 
    // the sound is determined in the Scriptable object of the music of sfx file. 
    
    private void PlayRandomSound(SCR_SoundSO[] sounds) {
        if(sounds != null && sounds.Length > 0) {
            SCR_SoundSO soundSO = sounds[Random.Range(0, sounds.Length)];
            SoundToPlay(soundSO);
        }
    }

    private void SoundToPlay(SCR_SoundSO soundSO) {
        AudioClip clip = soundSO.Clip;
        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume  * _masterVolume;
        bool loop = soundSO.Loop;
        AudioMixerGroup audioMixerGroup;
        
        pitch = RandomizePitch(soundSO, pitch);
        audioMixerGroup = DetermineAudioMixerGroup(soundSO);
        
        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SCR_SoundSO soundSO) {
        AudioMixerGroup audioMixerGroup;
        switch (soundSO.AudioType) {
            case SCR_SoundSO.AudioTypes.SFX:
                audioMixerGroup = _sfxAudioMixerGroup;
                break;
            case SCR_SoundSO.AudioTypes.Music:
                audioMixerGroup = _musicAudioMixerGroup;
                break;
            case SCR_SoundSO.AudioTypes.Ambiance:
                audioMixerGroup = _ambianceAudioMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }

        return audioMixerGroup;
    }

    private float RandomizePitch(SCR_SoundSO soundSO, float pitch) {
        if (soundSO.RandomizePitch) {
            float randomPitchModifier =
                Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);
            pitch = soundSO.Pitch + randomPitchModifier;
        }

        return pitch;
    }

    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup) {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();
        
        if(!loop){ Destroy(soundObject, clip.length);}

        
        //DetermineMusic(audioMixerGroup, audioSource);
        
    }

    private void DetermineMusic(AudioMixerGroup audioMixerGroup, AudioSource audioSource) {
        if (audioMixerGroup == _musicAudioMixerGroup) {
            if (_currentMusic != null) {
                _currentMusic.Stop();
            }

            _currentMusic = audioSource;
        }
    }

    #endregion

    #region SFX

    // Put all the methods to be called when a specific event is called
    // Don't forget to add them in onEnable and onDisable

    private void OnHitTreeSFX() {
        PlayRandomSound(_soundCollectionSO.CollisionTreeSFX);
    }

    private void OnPickUpNutSFX() {
        PlayRandomSound(_soundCollectionSO.PickUpSFX);
    }

    private void OnPickUpPowerUpShield() {
        PlayRandomSound(_soundCollectionSO.PowerUpShieldSFX);

    }

    private void OnPickUpPowerUpRay() {
        PlayRandomSound(_soundCollectionSO.PowerUpRaySFX);

    }
    
    // When the player bumps into the enemy
    private void OnCollisionEvil() {
        PlayRandomSound(_soundCollectionSO.HittingEvilSFX);
    }
    
    // When the player shoots the ray on the enemy
    private void OnHitEvilSFX() {
        PlayRandomSound(_soundCollectionSO.CollisionEvilSFX);
    }
    
    // Using the same sound for now when the ray hits the tree as the one when the ray hits the enemy
    private void OnKillTreeSFX() {
        PlayRandomSound(_soundCollectionSO.RayHittingTreeSFX); 
    }

    private void OnDash() {
        PlayRandomSound(_soundCollectionSO.DashSFX);
    }

    #endregion

    #region Music

    // Put all the methods that keeps track of the music of the game
    // Use ExampleMethod02 in case a new music should change to and then change back after the delay. 
    private void PlayEndMusic() {
        _audioSources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        if (_audioSources.Length != 0) {
            for (int i = 0; i < _audioSources.Length; i++) {
                //_audioSources[i].Stop();
                Destroy(_audioSources[i].gameObject);
            }
        }
        PlayRandomSound(_soundCollectionSO.EndOfGameMusic);
    }
    private void PlayMainMenuMusic() {
        PlayRandomSound(_soundCollectionSO.MainMenuMusic);
    }
    
    private void PlayForrestMusic() {
        PlayRandomSound(_soundCollectionSO.ForrestMusic);
    }
    private void PlayWindMusic() {
        PlayRandomSound(_soundCollectionSO.WindMusic);
    }

    private void PlayDayMusic() {
        PlayRandomSound(_soundCollectionSO.DayMusic);
    }

    private void PlayNightMusic() {
         if (_dayNightCycle.IsNightTime) {
            PlayRandomSound(_soundCollectionSO.NightMusic);
         }
    }
    
    // private void ExampleMethod02() {
    //     PlayRandomSound(_soundCollectionSO.ExampleMusicArray);
    //     float soundLenght = _soundCollectionSO.ExampleMusicArray[0].Clip.length;
    //     SCR_Utils.RunAfterDelay(this, soundLenght, ExamplePlayMethod);
    // }
    

    #endregion
}
