using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    private Image loadingBar; // This will show the progress of the loading
    [SerializeField]
    private string sceneToLoad; // This is the scene that we will be loading dynamically
    [SerializeField]
    private GameObject loadingBarBackground;
    [SerializeField]
    private GameObject loadButton;
    [SerializeField]
    private GameObject canvasCamera; // This will be disabled when the new scene is loaded

    public void OnClickLoadGame()
    {
        // Swap out the objects being shown on the screen
        loadingBarBackground.SetActive(true);
        loadButton.SetActive(false);

        StartCoroutine(LoadSceneAsync());
    }

    // Start is called before the first frame update
    void Start()
    {
        // Swap out the objects being shown on the screen
        loadingBarBackground.SetActive(false);
        loadButton.SetActive(true);
        canvasCamera.SetActive(true);
    }

    private IEnumerator LoadSceneAsync()
    {
        // Reset the loading bar in case it isn't done
        loadingBar.fillAmount = 0;

        // Start the load of the scene and tell it to activate when done
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        sceneLoadOperation.allowSceneActivation = true;

        // Loop continously until the operation is complete
        while(!sceneLoadOperation.isDone)
        {
            // Update the progress bar and wait until the next frame
            loadingBar.fillAmount = sceneLoadOperation.progress;
            yield return null;
        }

        // Update the loading bar to full and wait half a second
        loadingBar.fillAmount = 1;
        yield return new WaitForSeconds(1f);

        // Find and activate the character controller
        CharController_Motor motor = FindObjectOfType<CharController_Motor>();
        motor.Initialise();

        // Disable the camera and the loading bar
        canvasCamera.SetActive(false);
        loadingBarBackground.SetActive(false);
    }
}
