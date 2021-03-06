﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    private void Start() {
        playButton.onClick.AddListener(delegate {
            GameManager.Instance.ChangeScene("OptionsMenu");
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
