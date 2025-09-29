using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("场景设置")]
    public string mainMenuScene = "MainMenu";
    public string gameScene = "DemoScene";
    public float sceneLoadDelay = 0.1f;
    public float deathDelay = 1f;

    private Health currentPlayer;
    private bool isGameOver = false;

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
        }
    }

    public void StartNewGame()
    {
        isGameOver = false;
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        UIManager.Instance.ShowMainMenu(false);
        UIManager.Instance.ShowLoadingScreen(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameScene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            UIManager.Instance.UpdateLoadingProgress(progress);

            if (progress >= 1f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        InitializePlayer();

        yield return new WaitForSeconds(sceneLoadDelay);
        UIManager.Instance.ShowLoadingScreen(false);
    }

    private void InitializePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            currentPlayer = player.GetComponent<Health>();
            if (currentPlayer != null)
            {
                currentPlayer.OnDeath += HandlePlayerDeath;
            }
        }
    }

    private void HandlePlayerDeath()
    {
        if (isGameOver) return;
        
        StartCoroutine(ShowGameOverScreen());
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(ReloadGameScene());
    }

    private IEnumerator ReloadGameScene()
    {
        isGameOver = false;
        UIManager.Instance.ShowGameOverScreen(false);
        UIManager.Instance.ShowLoadingScreen(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameScene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            UIManager.Instance.UpdateLoadingProgress(progress);

            if (progress >= 1f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
        InitializePlayer();
        yield return new WaitForSeconds(sceneLoadDelay);
        UIManager.Instance.ShowLoadingScreen(false);
    }

    private IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(deathDelay);
        isGameOver = true;
        UIManager.Instance.ShowGameOverScreen(true);
        Time.timeScale = 0f;
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
        UIManager.Instance.ShowMainMenu(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}