using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Button backButton;
    public Slider volSlider;
    public TextMeshProUGUI sliderValueText;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown displayModeDropdown;
    private Resolution[] resolutions;
    public GameObject settingsUI, characterSelectUI, mapSelectUI, mainMenuUI, difficultySelectUI;
    public static string selectedCharacter, selectedMap, selectedDiff;

    void Start()
    {

        int savedMode = PlayerPrefs.GetInt("SavedDisplayMode", 0);

        displayModeDropdown.value = savedMode;
        displayModeDropdown.RefreshShownValue();

        ApplyDisplayMode(savedMode);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        foreach (Resolution res in resolutions)
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(res.width + "x" + res.height));

        resolutionDropdown.RefreshShownValue();

        int savedResIndex = PlayerPrefs.GetInt("SavedResolution", 0);

        if (savedResIndex < resolutions.Length)
        {
            resolutionDropdown.value = savedResIndex;
            resolutionDropdown.RefreshShownValue();

            Resolution r = resolutions[savedResIndex];
            Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);
        }

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 100f);
        volSlider.value = savedVolume;
        AudioListener.volume = savedVolume / 100f;
        sliderValueText.text = savedVolume + "%";


    }

    void ChangeMenu(GameObject menu)
    {
        settingsUI.SetActive(false);
        mainMenuUI.SetActive(false);
        characterSelectUI.SetActive(false);
        mapSelectUI.SetActive(false);
        difficultySelectUI.SetActive(false);
        menu.SetActive(true);
        if (menu == mainMenuUI)
        {
            return;
        }
        else
        {
            backButton.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        ChangeMenu(characterSelectUI);
    }

    public void SettingsMenu()
    {
        ChangeMenu(settingsUI);
    }

    public void BackButton()
    {
        if (mainMenuUI.activeSelf)
            return;
        if (settingsUI.activeSelf)
        {
            backButton.gameObject.SetActive(true);
            ChangeMenu(mainMenuUI);
            backButton.gameObject.SetActive(false);
        }
        else if (characterSelectUI.activeSelf)
        {
            backButton.gameObject.SetActive(true);
            ChangeMenu(mainMenuUI);
            backButton.gameObject.SetActive(false);
        }
        else if (mapSelectUI.activeSelf)
        {
            backButton.gameObject.SetActive(true);
            ChangeMenu(characterSelectUI);
        }
        else if (difficultySelectUI.activeSelf)
        {
            backButton.gameObject.SetActive(true);
            ChangeMenu(mapSelectUI);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnVolumeChanged(float _)
    {
        float value = volSlider.value;

        AudioListener.volume = value / 100f;
        PlayerPrefs.SetFloat("MasterVolume", value);

        sliderValueText.text = value.ToString("0") + "%";
    }

    public void OnResolutionChanged(int index)
    {
        if (index < 0 || index >= resolutions.Length)
            return;

        Resolution r = resolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);

        PlayerPrefs.SetInt("SavedResolution", index);
    }


    private void ApplyDisplayMode(int index)
    {
        switch (index)
        {
            case 0: // Fullscreen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;

            case 1: // Borderless
                Resolution nat = Screen.currentResolution;
                Screen.SetResolution(nat.width, nat.height, FullScreenMode.FullScreenWindow);
                break;

            case 2: // Windowed
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
    public void OnDisplayModeChanged(int index)
    {
        ApplyDisplayMode(index);
        PlayerPrefs.SetInt("SavedDisplayMode", index);
    }

    public void LunaButton()
    {
        ChangeMenu(mapSelectUI);
        selectedCharacter = "Luna";
    }

    public void RickButton()
    {
        ChangeMenu(mapSelectUI);
        selectedCharacter = "Rick";
    }

    public void ShellyButton()
    {
        ChangeMenu(mapSelectUI);
        selectedCharacter = "Shelly";
    }

    public void ZipButton()
    {
        ChangeMenu(mapSelectUI);
        selectedCharacter = "Zip";
    }
    public void DesertMapButton()
    {
        ChangeMenu(difficultySelectUI);
        selectedMap = "DesertScene";
    }
    public void DungeonMapButton()
    {
        ChangeMenu(difficultySelectUI);
        selectedMap = "DungeonScene";
    }
    public void CorruptedMapButton()
    {
        ChangeMenu(difficultySelectUI);
        selectedMap = "CorruptScene";
    }

    public void EasyButton()
    {
        selectedDiff = "Easy";
        StartGameWithSelections();
    }

    public void MediumButton()
    {
        selectedDiff = "Medium";
        StartGameWithSelections();
    }

    public void HardButton()
    {
        selectedDiff = "Hard";
        StartGameWithSelections();
    }

    void StartGameWithSelections()
    {
        SceneManager.LoadScene(selectedMap);
    }

}
