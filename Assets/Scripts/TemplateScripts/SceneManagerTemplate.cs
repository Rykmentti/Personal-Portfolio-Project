using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerTemplate : MonoBehaviour
{
    [SerializeField] Button startGameButton; // Add more buttons as necessary, or whatever you need.
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        startGameButton.onClick.AddListener(() => { StartCoroutine(FadeOut("Write scene name here.")); });
    }
    IEnumerator FadeIn()
    {
        GameObject fadeScreen = new GameObject();
        fadeScreen.name = "Fade Screen";
        fadeScreen.AddComponent<Canvas>();
        Canvas fadeScreenCanvas = fadeScreen.GetComponent<Canvas>();
        fadeScreenCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeScreenCanvas.sortingOrder = 1000;
        fadeScreen.AddComponent<CanvasScaler>();
        fadeScreen.AddComponent<GraphicRaycaster>();
        fadeScreen.AddComponent<Image>();
        Image fadeScreenImage = fadeScreen.GetComponent<Image>();

        fadeScreenImage.color = new Color(0, 0, 0, 1);
        for (float i = 1; i > 0; i -= (float)0.01)
        {
            fadeScreenImage.color = new Color(0, 0, 0, i);
            Debug.Log("transparency is = " + i);
            yield return new WaitForSeconds(.01f);
        }
        Destroy(fadeScreen);
    }

    IEnumerator FadeOut(string sceneName)
    {
        GameObject fadeScreen = new GameObject();
        fadeScreen.name = "Fade Screen";
        fadeScreen.AddComponent<Canvas>();
        Canvas fadeScreenCanvas = fadeScreen.GetComponent<Canvas>();
        fadeScreenCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeScreenCanvas.sortingOrder = 1000;
        fadeScreen.AddComponent<CanvasScaler>();
        fadeScreen.AddComponent<GraphicRaycaster>();
        fadeScreen.AddComponent<Image>();
        Image fadeScreenImage = fadeScreen.GetComponent<Image>();
        fadeScreenImage.color = new Color(0, 0, 0, 0);
        for (float i = 0; i < 1; i += (float)0.01)
        {
            fadeScreenImage.color = new Color(0, 0, 0, i);
            Debug.Log("transparency is = " + i);
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(1f);
        ChangeSceneWithSceneName(sceneName); // Use parameter, which scene to change.
    }

    void ChangeSceneWithSceneName(string sceneName) // Change scene method with name (string) as a parameter.
    {
        SceneManager.LoadScene(sceneName);
    }
}
