using UnityEngine;

public class AudioManager : MonoBehaviour
{

    void Awake()
    {
        var listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);

        if (listeners.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 100f);
        AudioListener.volume = savedVolume / 100f;

    }

}
