using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderEasy : MonoBehaviour
{
    public static int sceneToLoad = -1;
    public void setScene(int index)
    {
        print($"setting scene to {index}...");
        sceneToLoad = index;
    }
    public void loadscene()
    {
        //REMOVE THIS ASAP THIS IS NOT OKAY!!!!
        if (SceneManager.GetActiveScene().buildIndex == 8) { SceneManager.LoadScene(6); return; }

        print(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

}
