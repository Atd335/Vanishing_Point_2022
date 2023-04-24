using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UpdateDriver : MonoBehaviour
{
    public bool displayGizmos;

    public static UpdateDriver ud;
    public static int layerMask = ~(1 << 7);

    InteractableObjectLoader objectLoader;
    ImageCapture imgCapture;
    CharacterController2D cc2d;
    AnimationStateController animStates;
    SpeechScript speech;
    CharacterController3D cc3d;
    SFXScript sfx;
    MusicScript music;
    ModeSwitcher switcher;
    FreeMouseEnabler mouseEnabler;
    DeveloperTools devTools;
    OverlayController overlay;

    PauseSettingsScript pause;


    public int returnScene;
    public bool mouseLocked;
    public AudioClip[] phones;
    public AudioClip song;
    public EchoDialogueObject_3D[] dialogueObjs;

    public int frameLimit;
    public bool limitFrameRate;

    public bool hidePlayer = false;

    void Awake()
    {
        ud = this;

        objectLoader = GetComponent<InteractableObjectLoader>();

        imgCapture = GetComponent<ImageCapture>();

        cc2d = GetComponent<CharacterController2D>();

        animStates = GetComponent<AnimationStateController>();

        speech = GetComponent<SpeechScript>();

        cc3d = GetComponent<CharacterController3D>();


        music = GetComponent<MusicScript>();

        switcher = GetComponent<ModeSwitcher>();

        mouseEnabler = GetComponent<FreeMouseEnabler>();

        devTools = GetComponent<DeveloperTools>();

        overlay = GetComponent<OverlayController>();

        pause = GetComponent<PauseSettingsScript>();

        sfx = GetComponent<SFXScript>();
        TriggerStarts();
    }

    // Start is called before the first frame update
    void TriggerStarts()
    {
        objectLoader._Start();
        imgCapture._Start();
        cc2d._Start();
        animStates._Start();
        speech._Start();
        cc3d._Start();
        music._Start();
        switcher._Start();
        overlay._Start();
        sfx._Start();

        pause._Start();

        mouseEnabler.toggleMouse(mouseLocked);
        mouseEnabler._Start();
        devTools._Start();
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = (frameLimit * System.Convert.ToInt32(limitFrameRate)) + (0-System.Convert.ToInt32(limitFrameRate));

        if (!devTools.devModeEnabled)
        {
            if (!pause.gamePaused)
            {
                cc2d._Update();
                animStates._Update();
                speech._Update();
                cc3d._Update();
                switcher._Update();
                overlay._Update();
            }
            pause._Update();
        }
        music._Update();
        mouseEnabler._Update();
        devTools._Update();
    }

    public void speak(int id)
    {
        speech.Speak(dialogueObjs[id]);
    }

    public void easySceneChange(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void printStr(string str)
    {
        print(str);
    }
}
