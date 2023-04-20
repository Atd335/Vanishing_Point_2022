using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class CCOnlyUpdateDriver : MonoBehaviour
{

    public CC3D_BareBones cc3d;
    public SFXScript sfx;
    public MusicScript music;
    public SceneLoaderEasy SLE;
    public FadeControllerScript fade;
    public FreeMouseEnabler FME;

    public int nextOSIndex;

    bool typingModeActive;
    string editString;

    NarrativeEventList eventList;

    private void Start()
    {
        cc3d = GetComponent<CC3D_BareBones>();
        sfx = GetComponent<SFXScript>();
        music = GetComponent<MusicScript>();
        SLE = GetComponent<SceneLoaderEasy>();
        FME = GetComponent<FreeMouseEnabler>();

        FME._Start();
        sfx._Start();
        cc3d._Start();
        music._Start();

        eventList = new NarrativeEventList();
        eventList.list = new List<NarrativeBlockoutTimelineObject>();

        editString = "";


        path = Application.persistentDataPath + @$"/NarrativeEventData/";
        fileName = @$"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}.json";
        PersistentLevelSkipper_NarrativePass.setNextOSIndex(nextOSIndex);
    }

    string path;
    string fileName;

    public void Update()
    {
        cc3d._Update();
        music._Update();
        FME._Update();

        //Narrative Controls

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            typingModeActive = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            saveDataToDisc();
        }

        FME.toggleMouse(!typingModeActive);
        cc3d.blockMovement = typingModeActive;

        DrawNarrativeSteps();
    }

    void saveDataToDisc()
    {
        var jsonStr = JsonUtility.ToJson(eventList);
        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }

        while (true)
        {
            try
            {
                File.WriteAllText(path + fileName, jsonStr);
                break;
            }
            catch (Exception) { }
        }
    }

    void TimelineBuilder(CC3D_BareBones cc, string desc)
    {
        NarrativeBlockoutTimelineObject n = new NarrativeBlockoutTimelineObject();
        n.characterPosition = cc.player3D.transform.position;
        n.headRotation = cc.head.rotation;
        n.description = desc;

        eventList.list.Add(n);
        saveDataToDisc();
    }
    
    public AudioClip ding;
    private void OnGUI()
    {
        var titleStyle = new GUIStyle();
        titleStyle.fontSize = 30;

        //Title
        string str = "Narrative Blockout Mode. Press Tab to add an event.";
        titleStyle.normal.textColor = Color.white;
        

        var textAreaStyle = new GUIStyle();
        textAreaStyle = new GUIStyle("TextArea");
        textAreaStyle.fontSize = 30;

        if (typingModeActive)
        {
            int screenX = Screen.width - 24;
            int screenY = Screen.height - (24 + 32 + 12);
            editString = GUI.TextArea(new Rect(12, 12, screenX, screenY), editString, textAreaStyle);

            bool escapeButton = (GUI.Button(new Rect(12, screenY + 24, 64, 32), "ESCAPE"));
            bool submitButton = (GUI.Button(new Rect(12 + 64 + 24, screenY + 24, 64, 32), "SUBMIT"));

            if (escapeButton) { editString = ""; typingModeActive = false; }
            if (submitButton) { TimelineBuilder(cc3d, editString); editString = ""; typingModeActive = false; sfx.playSFX(ding); }
        }
        else
        {
            GUI.Label(new Rect(12, 12, Screen.width, Screen.height), str, titleStyle);
        }
    }

    void DrawNarrativeSteps()
    {
        var Drawg = Drawing.Draw.ingame;
        Color color1 = Color.red;

        using (Drawg.WithColor(color1))
        {
            for (int i = 0; i < eventList.list.Count; i++)
            {
                Drawg.WireCapsule(eventList.list[i].characterPosition + new Vector3(0, -.5f, 0), eventList.list[i].characterPosition + new Vector3(0, .5f, 0), .5f);
                Drawg.ArrowheadArc(eventList.list[i].characterPosition + new Vector3(0, .5f, 0), eventList.list[i].headRotation * Vector3.forward, .6f);
                Drawg.ArrowheadArc(eventList.list[i].characterPosition + new Vector3(0, .6f, 0), eventList.list[i].headRotation * Vector3.forward, .6f);
                Drawg.Label3D(eventList.list[i].characterPosition + new Vector3(0, .75f, 0), Quaternion.LookRotation(eventList.list[i].characterPosition - cc3d.player3D.transform.position), eventList.list[i].description, .025f, Drawing.LabelAlignment.Center);
                
                if (i > 0) {
                    Drawg.Line(eventList.list[i-1].characterPosition, eventList.list[i].characterPosition);
                }
                
                Drawg.Line(eventList.list[i].characterPosition+new Vector3(0,.75f,0), (eventList.list[i].characterPosition + new Vector3(0, .75f, 0)) + eventList.list[i].headRotation * Vector3.forward);                
            }   
        }
    }
}

[Serializable]
public class NarrativeEventList
{ 
    public List<NarrativeBlockoutTimelineObject> list;
}

[Serializable]
public class NarrativeBlockoutTimelineObject
{
    public Vector3 characterPosition;
    public Quaternion headRotation;
    public string description;
}
