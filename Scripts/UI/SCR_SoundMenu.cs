using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SCR_SoundMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambianceSlider;


    private void Start() {
        audioMixer.GetFloat("Master", out float masterVolume);
        masterSlider.value = masterVolume;
        
        audioMixer.GetFloat("Music", out float musicVolume);
        musicSlider.value = musicVolume;
        
        audioMixer.GetFloat("SFX", out float sfxVolume);
        sfxSlider.value = sfxVolume;
        
        audioMixer.GetFloat("Ambiance", out float ambianceVolume);
        ambianceSlider.value = ambianceVolume;
    }

    public void SetMasterVolume(float volume) {
        
        audioMixer.SetFloat("Master", volume);
        
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);

    }

    public void SetSoundEffectsVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }

    public void SetAmbianceVolume(float volume)
    {
        audioMixer.SetFloat("Ambiance", volume);
    }
    

}

