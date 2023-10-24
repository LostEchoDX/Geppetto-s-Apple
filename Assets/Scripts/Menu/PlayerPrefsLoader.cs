using UnityEngine;
using UnityEngine.Audio;

public class PlayerPrefsLoader : MonoBehaviour
{
    public AudioMixer mixer; // Référence à l'AudioMixer contenant les groupes de mixage

    private const string masterVolumeParam = "Master";   // Paramètre du groupe de mixage pour le volume principal
    private const string effectsVolumeParam = "Effects"; // Paramètre du groupe de mixage pour le volume des effets sonores
    private const string musicVolumeParam = "Music";     // Paramètre du groupe de mixage pour le volume de la musique

    private void Awake()
    {
        LoadVolumePref();
    }

    private void LoadVolumePref()
    {
        // Charger les valeurs des Playerprefs pour les paramètres de volume
        float savedMasterVolume = PlayerPrefs.GetFloat(masterVolumeParam + "Volume");
        float savedEffectsVolume = PlayerPrefs.GetFloat(effectsVolumeParam + "Volume");
        float savedMusicVolume = PlayerPrefs.GetFloat(musicVolumeParam + "Volume");

        // Appliquer les valeurs chargées aux groupes de mixage de l'AudioMixer
        mixer.SetFloat(masterVolumeParam, Mathf.Log10(savedMasterVolume) * 20);
        mixer.SetFloat(effectsVolumeParam, Mathf.Log10(savedEffectsVolume) * 20);
        mixer.SetFloat(musicVolumeParam, Mathf.Log10(savedMusicVolume) * 20);
    }
}
