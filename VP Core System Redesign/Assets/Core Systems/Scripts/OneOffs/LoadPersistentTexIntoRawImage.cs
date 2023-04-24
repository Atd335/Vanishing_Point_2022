using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPersistentTexIntoRawImage : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<RawImage>().texture = TextureHolder.th.StoredTex;
        print("Attempted to load screencap");
        Destroy(TextureHolder.th.gameObject);
    }
}
