using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManagerTitleScreenScene : MonoBehaviour
{
    [SerializeField] string nextSceneName;

    [SerializeField] Button startButton; // Assign in Editor
    [SerializeField] Button exitButton; // Assign in Editor

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(() => { StartGame(); });
        exitButton.onClick.AddListener(() => { QuitGame(); });
    }

    void StartGame()
    {
        FadeSceneTransitionTemplate fadeSceneTransitionTemplate = GetComponent<FadeSceneTransitionTemplate>();
        fadeSceneTransitionTemplate.SetTransitionSceneName(nextSceneName);
        StartCoroutine(fadeSceneTransitionTemplate.FadeOutTransitionSequence());
        TitleScreenSceneAudioManager.titleScreenSceneAudioManager.StartCoroutine(TitleScreenSceneAudioManager.titleScreenSceneAudioManager.GraduallyDecreaseVolume());
    }
    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_ANDROID || UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
