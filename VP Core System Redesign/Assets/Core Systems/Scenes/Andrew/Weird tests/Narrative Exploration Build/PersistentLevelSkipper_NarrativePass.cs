using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentLevelSkipper_NarrativePass : MonoBehaviour
{   
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    bool inOS;
    static int buildIndex;
    // Update is called once per frame
    void Update()
    {
        inOS = (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 6);

        if (inOS && Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneManagerScript.loadSceneImmediate(buildIndex);
        }
    }

    public static void setNextOSIndex(int indx)
    {
        buildIndex = indx;
    }

    private void OnGUI()
    {
        if (!inOS) { return; }

        var titleStyle = new GUIStyle();
        titleStyle.fontSize = 30;

        //Title
        string str = "Narrative Blockout Mode. Press Enter to progress to the next level.";
        titleStyle.normal.textColor = Color.red;

        GUI.Label(new Rect(12, 12, Screen.width, Screen.height), str, titleStyle);

    }
}
