using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreScreenshot : MonoBehaviour
{
    //grab the main camera
    //assign it a render texure as an output texture. 
    //have it render to said texture (wait for onpostrender)
    //un-assign the RT
    //save the reference to the textureholder
    //load it in the next scene. 
    //

    Camera cam;
    public RenderTexture screencapRT;
    bool preTrigger;
    bool postTrigger;

    public void captureScreenshot()
    {
        cam = Camera.main;
        cam.targetTexture = screencapRT;
        cam.Render();
        cam.targetTexture = null;
        print("capturing Screenshot...");

        TextureHolder.th.StoredTex = screencapRT;
    }
}
