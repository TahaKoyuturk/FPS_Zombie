using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    #region Variables

    #region Public Variables

    [Header("Play Game")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    #endregion

    #endregion

    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        //playButton.onClick.AddListener(() => SceneManager.LoadScene("ZombieWar"));
        //quitButton.onClick.AddListener(() => Application.Quit());
    }

    #endregion

    #region Custom Methods
    public void PlayBtn()
    {
        SceneManager.LoadScene("ZombieWar");
    }
    public void QuitBtn()
    {
        Application.Quit();
    }
    #endregion

    #endregion
}
