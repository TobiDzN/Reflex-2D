using UnityEngine;

public class PlayerSoundScript : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip shot, click, shield;
    public CombatController cmbt;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PauseManager.isPaused) return;

        PlaySound();
    }

    void PlaySound()
    {
        if (Input.GetMouseButtonDown(0) && !cmbt.isReloading && cmbt.shotCount != 0)
        {
            audioSource.PlayOneShot(shot);
        }
        else if (Input.GetMouseButtonDown(0) && (cmbt.isReloading || cmbt.shotCount == 0))
        {
            audioSource.PlayOneShot(click);
        }
        else if (Input.GetMouseButtonDown(1) && !cmbt.shieldOnCooldown)
        {
            audioSource.PlayOneShot(shield);
        }

    }
}
