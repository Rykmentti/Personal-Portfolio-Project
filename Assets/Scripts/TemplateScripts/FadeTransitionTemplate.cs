using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FadeTransitionTemplate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        StartCoroutine(FadeOutSequence());
    }
    IEnumerator FadeOutSequence()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Insert Scene Name Here"); ;
        // If you use this FadeOut IEnumerator to end a scene, remember to add SceneManager method. SceneManager.LoadScene("SceneName") 
        // Remember to add the scene name to build settings.
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

    IEnumerator FadeOut()
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
    }
}