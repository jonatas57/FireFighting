using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    private void Start() {
        playButton.onClick.AddListener(delegate {
            GameManager.Instance.NewGame();
        });

        exitButton.onClick.AddListener(delegate {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        });
    }
}
