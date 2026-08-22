using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;
using static UnityEngine.Rendering.DebugUI.MessageBox;
public class MainMenu : MonoBehaviour   //Main menu scene
{
    public UIDocument document;
    private Button startGameButton;
    private Button optionsButton;
    private Button quitGameButton;

    private void Start()
    {
        VisualElement mainMenuRoot = document.rootVisualElement;

        startGameButton = mainMenuRoot.Q<Button>("Start");
        optionsButton = mainMenuRoot.Q<Button>("Options");
        quitGameButton = mainMenuRoot.Q<Button>("Exit");

        startGameButton.clicked += OnStartGameButtonClicked;
        optionsButton.clicked += OnOptionsButtonClicked;
        quitGameButton.clicked += OnQuitButtomClicked;
    }
    private void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("KeagMain");
    }
    private void OnOptionsButtonClicked()
    {
        Debug.Log("options clicked");
    }


    private void OnQuitButtomClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
