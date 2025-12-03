using UnityEngine;

[CreateAssetMenu()]
public class SCR_SoundsCollectionSO : ScriptableObject {
    [Header("Music")] 
    public SCR_SoundSO[] WindMusic;
    public SCR_SoundSO[] ForrestMusic;
    public SCR_SoundSO[] DayMusic;
    public SCR_SoundSO[] NightMusic;
    public SCR_SoundSO[] MainMenuMusic;
    public SCR_SoundSO[] EndOfGameMusic;
  
    
    [Header("SFX")]
    public SCR_SoundSO[] DashSFX;
    public SCR_SoundSO[] PickUpSFX;
    public SCR_SoundSO[] PowerUpShieldSFX;
    public SCR_SoundSO[] PowerUpRaySFX;
    public SCR_SoundSO[] CollisionTreeSFX;
    public SCR_SoundSO[] CollisionEvilSFX;
    public SCR_SoundSO[] HittingEvilSFX;
    public SCR_SoundSO[] RayHittingTreeSFX;

}
