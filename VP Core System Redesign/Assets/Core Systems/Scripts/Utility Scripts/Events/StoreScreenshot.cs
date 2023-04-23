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
}

//void OnPreRender()
//{
//    rt = RenderTexture.GetTemporary(mainrtdesc);
//    myCam.targetTexture = rt; // This makes the camera render to the rt RenderTexture
//}

//void OnPostRender()
//{
//    myCam.targetTexture = null;

//    // Do whatever Graphics.Blits you want here.
//    // Just make sure you do a final one to "null as RenderTexture"
//    // so it will write to the framebuffer.

//    RenderTexture.ReleaseTemporary(rt);
//}