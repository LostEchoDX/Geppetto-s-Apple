using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;       // Nom de la scène à charger
    public float delay = 2f;         // Délai avant le chargement de la scène
    public Slider loadingSlider;     // Référence au Slider pour afficher la progression du chargement
    public Canvas loadingCanvas;     // Référence au Canvas contenant l'écran de chargement

    private bool isLoading = false;  // Indique si le chargement est en cours


    public void LoadSceneWithLoading()
    {
        if (!isLoading)
        {
            StartCoroutine(LoadSceneCoroutine());
        }
    }

    IEnumerator LoadSceneCoroutine()
    {
        isLoading = true;
        loadingCanvas.gameObject.SetActive(true);

        float timeElapsed = 0f;

        while (timeElapsed < delay)
        {
            timeElapsed += Time.deltaTime;

            // Mettez à jour le Slider pour afficher la progression
            float progress = Mathf.Clamp01(timeElapsed / delay);
            loadingSlider.value = progress;

            yield return null;
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Empêche la scène de se charger automatiquement une fois le chargement asynchrone terminé
        asyncOperation.allowSceneActivation = false;

        // Tant que la scène n'est pas complètement chargée
        while (!asyncOperation.isDone)
        {
            // Mettez à jour le Slider pour afficher la progression
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingSlider.value = progress;

            // Une fois que la scène est chargée à 90%, vous pouvez activer la scène
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;

            }

            yield return null;
        }

        loadingCanvas.gameObject.SetActive(false);
        isLoading = false;
    }

    public void LoadScene() {
        SceneManager.LoadScene("Village");
    }
}
