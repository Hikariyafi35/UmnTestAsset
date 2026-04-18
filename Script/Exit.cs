using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    [Header("UI Button")]
    public Button exitButton;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
    }

    public void ExitGame()
    {
        Debug.Log("Game Closed");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}