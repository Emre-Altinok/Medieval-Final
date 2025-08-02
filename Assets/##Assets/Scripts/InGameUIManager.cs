using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [Header("Ayarlar Paneli")]
    public GameObject settingsPanel;
    public GameObject damagePanel;
    public GameObject questPanel; // Eklendi

    private bool isMenuOpen = false;
    private bool isQuestOpen = false;
    private InputAction menuAction;
    private InputAction questAction;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider deathSlider;
    public Slider footstepSlider;
    public Slider swordSlider;
    public Slider masterSlider;

    void Start()
    {

     //  Debug.Log("[InGameUIManager] Start �a�r�ld�.");
        // Slider ba�lant�lar� ayn�
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        deathSlider.onValueChanged.AddListener(AudioManager.Instance.SetDeathVolume);
        footstepSlider.onValueChanged.AddListener(AudioManager.Instance.SetFootstepVolume);
        swordSlider.onValueChanged.AddListener(AudioManager.Instance.SetSwordVolume);
        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
    }

    void OnEnable()
    {
        //Debug.Log("[InGameUIManager] OnEnable �a�r�ld�.");
        menuAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        menuAction.performed += ctx => ToggleMenu();
        menuAction.Enable();
    }


    void OnDisable()
    {
        //Debug.Log("[InGameUIManager] OnDisable �a�r�ld�.");
        if (menuAction != null)
        {
            menuAction.performed -= ctx => ToggleMenu();
            menuAction.Disable();
        }
    }

    private void ToggleMenu()
    {
        //Debug.Log($"[InGameUIManager] ToggleMenu �a�r�ld�. isMenuOpen: {isMenuOpen}");
        if (isMenuOpen)
            CloseSettingsMenu();
        else
            OpenSettingsMenu();
    }

    public void OpenSettingsMenu()
    {
        //Debug.Log("[InGameUIManager] OpenSettingsMenu �a�r�ld�.");
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isMenuOpen = true;
    }

    public void CloseSettingsMenu()
    {
        //Debug.Log("[InGameUIManager] CloseSettingsMenu �a�r�ld�.");
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isMenuOpen = false;
    }

    public void OpenQuestPanel()
    {
        //Debug.Log("[InGameUIManager] OpenQuestPanel �a�r�ld�.");
        if (questPanel != null)
        {
            questPanel.SetActive(true);
            //Debug.Log("[InGameUIManager] questPanel.SetActive(true) �a�r�ld�.");
        }
        else
        {
            //Debug.LogWarning("[InGameUIManager] questPanel referans� atanmad�!");
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isQuestOpen = true;
    }

    public void CloseQuestPanel()
    {
        //Debug.Log("[InGameUIManager] CloseQuestPanel �a�r�ld�.");
        if (questPanel != null)
            questPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isQuestOpen = false;
    }

    public void ShowDamagePanel()
    {
        //Debug.Log("[InGameUIManager] ShowDamagePanel �a�r�ld�.");
        if (damagePanel != null)
        {
            damagePanel.SetActive(true);
            Invoke("HideDamagePanel", 0.5f);
        }
    }


    public void HideDamagePanel()
    {
        //Debug.Log("[InGameUIManager] HideDamagePanel �a�r�ld�.");
        if (damagePanel != null)
            damagePanel.SetActive(false);
    }
}