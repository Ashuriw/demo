using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("主菜单界面")]
    public GameObject mainMenuPanel;
    public Button startButton;
    public Button quitButton;

    [Header("加载界面")]
    public GameObject loadingScreen;
    public Slider loadingProgress;
    public Text loadingText;

    [Header("结束界面")]
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button menuButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupButtons();
        ShowMainMenu(true);
    }

    private void SetupButtons()
    {
        startButton.onClick.AddListener(() => GameManager.Instance.StartNewGame());
        quitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        restartButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
        menuButton.onClick.AddListener(() => GameManager.Instance.ReturnToMainMenu());
    }

    public void ShowMainMenu(bool show)
    {
        mainMenuPanel.SetActive(show);
        if (show)
        {
            gameOverPanel.SetActive(false);
            loadingScreen.SetActive(false);
        }
    }

    public void ShowLoadingScreen(bool show)
    {
        loadingScreen.SetActive(show);
        if (show) loadingProgress.value = 0f;
    }

    public void UpdateLoadingProgress(float progress)
    {
        loadingProgress.value = progress;
        loadingText.text = $"加载中... {Mathf.Round(progress * 100)}%";
    }

    public void ShowGameOverScreen(bool show)
    {
        gameOverPanel.SetActive(show);
    }
}