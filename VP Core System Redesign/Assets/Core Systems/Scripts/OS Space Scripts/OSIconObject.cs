using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OSIconObject : MonoBehaviour
{
    OSCanvasRaycaster oscr;

    RectTransform rt;

    public FileIcon file;
    public Image iconIMG;
    public Image highlightIMG;
    public TextMeshProUGUI tmp;
    public GameObject window;
    public GameObject lastWindowSpawned;
    public GameObject taskBarTab;
    public Object content;

    public bool highlighted;
    public bool onlySpawnOne;

    private void OnValidate()
    {
        if (file != null)
        {
            updateIcon();
        }
    }

    private void Start()
    {
        rt = GetComponent<RectTransform>();

        oscr = OSCanvasRaycaster.oscr;
        oscr.onClick.AddListener(onClick);
        oscr.onHold.AddListener(onHold);

        if (file.openWindowOnStart) { openWindow(); }
        //print(file.level);
    }

    void onClick()
    {
        if (oscr.hoveredOBJ == this.gameObject)
        {
            if (highlighted)
            {
                print($"Opening {this.gameObject.name}'s window");
                openWindow();               
                return;
            }
            highlighted = true;
            highlightIMG.enabled = (highlighted);
        }
        else
        {
            highlighted = false;
            highlightIMG.enabled = (highlighted);
        }
    }

    void onHold()
    {
        //if (highlighted) { rt.anchoredPosition = Input.mousePosition; }
    }

    void createTaskbarTab()
    {
        GameObject tab = Instantiate(taskBarTab, GameObject.FindGameObjectWithTag("TabContainer").transform);
        tab.GetComponent<TaskBarTaskScript>().assignedWindow = lastWindowSpawned;
        tab.GetComponentInChildren<TextMeshProUGUI>().text = tmp.text;
        tab.name += $"[{tmp.text}]";
        
        //this is an insane way of finding the proper icon. you really need to have some array of icons that all of these prefabs pull from. 
        tab.GetComponentsInChildren<Image>()[tab.GetComponentsInChildren<Image>().Length - 1].sprite = 
            lastWindowSpawned.GetComponentsInChildren<Image>()[lastWindowSpawned.GetComponentsInChildren<Image>().Length - 1].sprite;
    }

    public void openWindow()
    {
        if (lastWindowSpawned != null && onlySpawnOne) { return; }

        GameObject win = Instantiate(window,oscr.appCanvas);
        BaseWindowController bwc = win.GetComponent<BaseWindowController>();
        win.GetComponentInChildren<WindowTitleTag>().GetComponent<TextMeshProUGUI>().text = file.name;
        bwc.RT.anchoredPosition = file.windowPosition;
        bwc.RT.sizeDelta = file.windowScale;
        bwc.lockedRatioSize = rt.sizeDelta;
        lastWindowSpawned = win;
        win.GetComponent<IContentLoader>().loadContent(content);

        highlighted = false;
        highlightIMG.enabled = (highlighted);

        if (file.islevelPortal)
        {
            SceneLoaderEasy.sceneToLoad = file.level;
            print($"setting level to {file.level}...");
            win.AddComponent<TimedEvent>();
            
            var timedEventInst = win.GetComponent<TimedEvent>();
            timedEventInst.time = 1.5f;
            timedEventInst.timedEvent = new UnityEngine.Events.UnityEvent();
            timedEventInst.timedEvent.AddListener(openErrorWindow);            
        }

        createTaskbarTab();
    }

    void openErrorWindow()
    {
        print("spawning error window...");
        GameObject win = Instantiate(file.errorWindow, oscr.appCanvas);
        BaseWindowController bwc = win.GetComponent<BaseWindowController>();
        win.GetComponentInChildren<WindowTitleTag>().GetComponent<TextMeshProUGUI>().text = $"Error with {file.name}!";
        bwc.RT.anchoredPosition = new Vector2((Screen.width / 2)- (bwc.RT.sizeDelta.x / 2), (Screen.height / 2)+(bwc.RT.sizeDelta.y/2));
    }

    void setLevelIndex()
    {
        SceneLoaderEasy.sceneToLoad = file.level;
    }

    public void updateIcon()
    {
        if (file != null)
        {         
            tmp.text = file.name + file.extension;
            iconIMG.sprite = file.icon;
            content = file.content;
            this.gameObject.name = "ICON_" + tmp.text;
            if (!file.autoPlace) { GetComponent<RectTransform>().anchoredPosition = file.positionOverride; }
            window = file.window;
            onlySpawnOne = file.limitToOneInstance;

            if (file.glitchIcon) { GetComponent<ImageGlitchScript>().spriteArray = file.glitchedIcons; }
            GetComponent<ImageGlitchScript>().enabled = file.glitchIcon;
        }
    }


}
