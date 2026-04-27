using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject darkOverlay;
    public GameObject resumeButton;
    public static bool isPaused = false;
    public static bool isDead = false;

    void Start()
    {

    }

    void Update()
    {
        if (isDead)
        {
            DeathScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isDead)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
        resumeButton.SetActive(true);
        darkOverlay.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void DeathScreen()
    {
        Pause();
        resumeButton.gameObject.SetActive(false);
    }

    public void Resume()
    {
        resumeButton.SetActive(true);
        pauseUI.SetActive(false);
        darkOverlay.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        isPaused = false;
        Time.timeScale = 1f;
        isDead = false;
        SceneManager.LoadScene("MainMenu");
    }



}
