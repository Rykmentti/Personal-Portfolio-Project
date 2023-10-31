using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BootUpSequenceManager : MonoBehaviour
{
    private string currentSceneName;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        if(currentSceneName == "BootUpScene")
        {
            StartCoroutine(DefaultBootUpSequence());
        }
        if (currentSceneName == "MadeByRykmenttiScene")
        {
            StartCoroutine(FadeIn());
            StartCoroutine(MadeByRykmenttiSceneBootSequence());
        }
        if (currentSceneName == "GameStartScene")
        {
            StartCoroutine(FadeIn());
        }        
    }
    IEnumerator MadeByRykmenttiSceneBootSequence()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Assets/Scenes/BootUpSequence/GameStartScene.unity");
    }
    IEnumerator DefaultBootUpSequence()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Assets/Scenes/BootUpSequence/MadeByRykmenttiScene.unity");
    }
    public IEnumerator FadeIn()
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
        // Jos k‰yt‰t t‰t‰ FadeOut Ienumeraattoria lopettamaan scenen, niin lis‰‰ t‰h‰n kohtaan koodi SceneManager funktio > SceneManager.LoadScene("haluamasi scene path");
        // Muista lis‰t‰ build settingsin scene in build listalle.
    }
    //Vanha Fade In/Fade Out Mekaniikka. Ei mit‰‰n j‰rke‰ tehd‰ n‰in monimutkasta fade In ja Fade Out mekaniikka. Ainakin opin miten Arrayt, For loopit ja foreach loop toimii.
    //Paljon yksin kertaisempaa tehd‰ vaa musta gameObject, joka on kameran kokonen nelikulmio, joka on kameran "p‰‰ll‰" (eli layer on korkein kaikista rendererist‰) ja siit‰ sitten alphaa nostaa ja laskee. Yl‰puolella parempi versio.
    /*
    private SpriteRenderer[] spriteRenderers;
    //Disabloi kaikki sprite rendererit scenest‰, muuttaa niitten alphaa, jotta niist‰ tulee transparent ja kamerassa on default background musta, luoden Fade In ja Fade Out mekaniikan.

    IEnumerator FadeIn()
    {
        spriteRenderers = new SpriteRenderer[10];
        spriteRenderers = FindObjectsOfType<SpriteRenderer>();

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color baseColor = spriteRenderer.color;
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
            for (float i = 0; i < 1; i += (float)0.1)
            {
                spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, i);
                Debug.Log("transparency is = " + i);
                yield return new WaitForSeconds(.1f);
            }
        }
    }
    IEnumerator FadeOut()
    {
        spriteRenderers = new SpriteRenderer[10];
        spriteRenderers = FindObjectsOfType<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            Color baseColor = spriteRenderer.color;
            spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
            for (float i = 1; i > 0; i -= (float)0.1)
            {
                spriteRenderer.color = new Color(baseColor.r, baseColor.g, baseColor.b, i);
                Debug.Log("transparency is = " + i);
                yield return new WaitForSeconds(.1f);
            }
        }

    }
    */
}
