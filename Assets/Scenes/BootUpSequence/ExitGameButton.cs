using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitGameButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Button thisButton;
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => { ExitGame(); });
    }

    void ExitGame()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
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
        Application.Quit();
        Debug.Log("Game has terminated");
    }

}