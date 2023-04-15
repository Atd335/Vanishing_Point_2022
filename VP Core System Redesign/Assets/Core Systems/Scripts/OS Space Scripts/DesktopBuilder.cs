using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopBuilder : MonoBehaviour
{
    public static DesktopBuilder desktopBuilder;

    public GameObject iconPrefab;
    [HideInInspector]
    public Transform iconCanvas;

    public FileList iconList;
    int iconCount = 0;

    public bool autoPlace = true;

    private void Awake()
    {
        if (DesktopLists.listToLoad) { iconList = DesktopLists.listToLoad; }
        desktopBuilder = this;
    }

    private void Start()
    {
        PlaceIcons();
    }

    void PlaceIcons()
    {
        iconCanvas = GameObject.FindGameObjectWithTag("Icon Canvas").transform;

        foreach (FileIcon icon in iconList.files)
        {
            placeIndividualIcon(icon);
        }
    }

    void placeIndividualIcon(FileIcon icon)
    {

        GameObject i = Instantiate(iconPrefab,iconCanvas);
        OSIconObject oi = i.GetComponent<OSIconObject>();
        oi.file = icon;
        oi.updateIcon();

        //set positiion
        if (oi.file.autoPlace)
        {
            float spacing = 120;

            float xPos = 75 + (spacing * iconCount) - (6* spacing * ((int)(iconCount / 6)));
            float yPos = -(((int)(iconCount / 6)) * spacing);

            yPos -= 75;

            oi.GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos,yPos);
        }

        //spawn it
        iconCount++;
    }

    void placeOpenExecutables()
    { 
        
    }

	public void _Start()
    {
        
    }
    public void _Update()
    {
        
    }
}
