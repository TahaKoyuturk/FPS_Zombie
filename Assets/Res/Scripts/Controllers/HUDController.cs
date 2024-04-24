using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    #region Variables

    #region Public Variables

    [Header("Health")]
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private TMP_Text HealthText;

    [Header("XP")]
    [SerializeField] private Slider XPSlider;
    [SerializeField] private TMP_Text XPText;

    [Header("Texts")]
    [SerializeField] TMP_Text _interactionText;
    [SerializeField] TMP_Text _ammoText;
    [SerializeField] TMP_Text _killCounterText;
    [SerializeField] TMP_Text _highScoreText;

    [Header("GameOver")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("References")]
    [SerializeField] GameObject talentPanel;
    [SerializeField] GameObject playerDeadPanel;
    [SerializeField] GameObject gotHitScreen;

    #endregion

    #region Private Variables

    private float _maxHealth;
    private int _killCounter = -1;
    private int flipflop = -1;
    private MyInputActions inputActions;

    #endregion


    #endregion

    #region Methods

    #region Unity Callbacks
    private void Awake()
    {
        inputActions = new MyInputActions();

        inputActions.UI.OpenTalent.performed += e => OpenTalentPanel();

        inputActions.Enable();
    }
    private void Start()
    {
        UpdateKillCounterText(-1);
        OpenTalentPanel();
        restartButton.onClick.AddListener(() => RestartButtonAction());
        quitButton.onClick.AddListener(() => QuitButtonAction());
    }
    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnHealthUpdate, UpdateHealthBar);
        EventManager.AddHandler(GameEvent.OnMaxHealthSet, InitialSetHealthValue);

        EventManager.AddHandler(GameEvent.OnXPSet, InitialSetXpValue);
        EventManager.AddHandler(GameEvent.OnXPUpdate, UpdateXPBar);

        EventManager.AddHandler(GameEvent.OnEnableItemInteraction, EnableInteractionText);
        EventManager.AddHandler(GameEvent.OnDisableItemInteraction, DisableInteractionText);

        EventManager.AddHandler(GameEvent.OnAmmoTextUpdate, UpdateAmmoText);

        EventManager.AddHandler(GameEvent.OnKillCounterText, UpdateKillCounterText);

        EventManager.AddHandler(GameEvent.OnPlayerHit, GotHit);
        EventManager.AddHandler(GameEvent.OnPlayerDead, OpenPlayerDeadUI);

        EventManager.AddHandler(GameEvent.OnHighScoreUpdate, UpdateHighScoreText);
    }
    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnHealthUpdate, UpdateHealthBar);
        EventManager.RemoveHandler(GameEvent.OnMaxHealthSet, InitialSetHealthValue);

        EventManager.RemoveHandler(GameEvent.OnXPSet, InitialSetXpValue);
        EventManager.RemoveHandler(GameEvent.OnXPUpdate, UpdateXPBar);

        EventManager.RemoveHandler(GameEvent.OnEnableItemInteraction, EnableInteractionText);
        EventManager.RemoveHandler(GameEvent.OnDisableItemInteraction, DisableInteractionText);

        EventManager.RemoveHandler(GameEvent.OnAmmoTextUpdate, UpdateAmmoText);

        EventManager.RemoveHandler(GameEvent.OnKillCounterText, UpdateKillCounterText);

        EventManager.RemoveHandler(GameEvent.OnPlayerHit, GotHit);
        EventManager.RemoveHandler(GameEvent.OnPlayerDead, OpenPlayerDeadUI);

        EventManager.RemoveHandler(GameEvent.OnHighScoreUpdate, UpdateHighScoreText);
    }

    #endregion

    #region Custom Methods
    private void OpenTalentPanel()
    {
        if (flipflop == 1)
        {
            talentPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            EventManager.Broadcast(GameEvent.OnUIOpening,-1);
        }
        else
        {
            talentPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            EventManager.Broadcast(GameEvent.OnUIClosing, -1);
        }

        flipflop *= -1;
    }
    private void OpenPlayerDeadUI(object value)
    {
        Time.timeScale = 0;
        playerDeadPanel.SetActive(true);
    }
    public void EnableInteractionText(object value)
    {
        _interactionText.gameObject.SetActive(true);
    }
    public void DisableInteractionText(object value)
    {
        _interactionText.gameObject.SetActive(false);
    }
    private void UpdateHealthBar(object healthValue)
    {
        int temphealth = Convert.ToInt32(healthValue);
        HealthSlider.value = (float)(temphealth / _maxHealth);
        HealthText.text = ((temphealth / _maxHealth) * 100).ToString();
    }
    private void UpdateXPBar(object xpValue)
    {
        int tempXp = Convert.ToInt32(xpValue);

        if((float)((float)tempXp / 100) >= 1)
        {
            XPSlider.value = 1;
        }

        XPSlider.value = (float)((float)tempXp /100);
        
        XPText.text = ((int)tempXp).ToString();
    }
    private void InitialSetHealthValue(object value)
    {
        int temp = Convert.ToInt32(value);
        HealthSlider.value = (int)temp;
        _maxHealth = (int)temp;
        HealthText.text = ((int)temp).ToString();
    }
    private void InitialSetXpValue(object value)
    {
        int tempXp = Convert.ToInt32(value);
        XPSlider.value = (int)tempXp;
        XPText.text = ((int)tempXp).ToString();
    }
    private void UpdateAmmoText(object value)
    {
        int _value = Convert.ToInt32(value);
        _ammoText.text = _value.ToString();
    }
    private void UpdateKillCounterText(object value)
    {
        _killCounter++;
        _killCounterText.text = ("Kills : " + _killCounter).ToString();
    }
    private void RestartButtonAction()
    {
        PlayerPrefs.SetInt("Level",1);
        SceneManager.LoadScene("Main");
    }
    private void QuitButtonAction()
    {
        Application.Quit();
    }
    private void UpdateHighScoreText(object value)
    {
        _highScoreText.text = ("Highscore : " + PlayerPrefs.GetInt("Highscore")).ToString();
    }
    private void GotHit(object value)
    {
        StartCoroutine(FlashScreen());
    }
    private IEnumerator FlashScreen()
    {
        Image imageComponent = gotHitScreen.GetComponent<Image>();

        if (imageComponent != null)
        {
            imageComponent.color = Color.red;

            float elapsedTime = 0.0f;
            float duration = 0.5f; 

            while (elapsedTime < duration)
            {
                imageComponent.color = Color.Lerp(Color.red, new Color(1, 0, 0, 0.5882f), elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(.05f);

            elapsedTime = 0.0f;
            while (elapsedTime < duration)
            {
                imageComponent.color = Color.Lerp(new Color(1, 0, 0, 0.5882f), new Color(1, 0, 0, 0), elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
    
    #endregion

    #endregion
}
