using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class OSButtonScript : MonoBehaviour
{

    Image img;

    public Color defaultColor;
    public Color hoveredColor;
    public Color clickedColor;
    public float colorLerpSpeed = 10;

    OSCanvasRaycaster oscr;
    public UnityEvent clickEvent;
    public UnityEvent releaseEvent;
    public UnityEvent hoveredEvent;
    public UnityEvent notHoveredEvent;

    public Sprite swappedSprite;
    Sprite defaultSprite;


    private void Start()
    {
        img = GetComponent<Image>();
        oscr = OSCanvasRaycaster.oscr;
        oscr.onClick.AddListener(onClick);
        oscr.onRelease.AddListener(onRelease);
        defaultSprite = img.sprite;
    }

    private void Update()
    {
        if (this.gameObject == oscr.hoveredOBJ) 
        { 
            img.color = Color.Lerp(img.color, hoveredColor, Time.deltaTime * colorLerpSpeed); 
            if (swappedSprite) { img.sprite = swappedSprite; }
            hoveredEvent.Invoke();
        }
        else 
        { 
            img.color = Color.Lerp(img.color, defaultColor, Time.deltaTime * colorLerpSpeed);
            if (swappedSprite) { img.sprite = defaultSprite; }
            notHoveredEvent.Invoke();
        }
    }

    void onClick()
    {
        if (oscr.hoveredOBJ != this.gameObject) { return; }
        img.color = clickedColor;
        clickEvent.Invoke();
    }

    void onRelease()
    {
        if (oscr.hoveredOBJ != this.gameObject) { return; }
        releaseEvent.Invoke();
    }

    #region closeApplication
    public void closeApp()
    {
        Application.Quit();
    }
    #endregion

    #region Maximize Method
    Vector2 initSize;
    Vector2 initPos;
    bool maximized = false;
    public void maximizeWindowToggle()
    {
        maximized = !maximized;
        BaseWindowController bwc = null;
        Transform tmpTrans = transform;

        while (bwc == null || tmpTrans == null)
        {
            tmpTrans = tmpTrans.parent;
            try { bwc = tmpTrans.gameObject.GetComponent<BaseWindowController>(); }
            catch (System.Exception) { }
        }

        bwc.setToTop();

        if (maximized)
        {
            initSize = bwc.RT.sizeDelta;
            initPos = bwc.RT.anchoredPosition;
        }
        StartCoroutine(resizeWindow(maximized, bwc));
    }
    IEnumerator resizeWindow(bool makeBig, BaseWindowController bwc)
    {
        float timer = 0;

        if (makeBig)
        {
            while (timer < 1)
            {
                timer += Time.deltaTime * 10;
                timer = Mathf.Clamp(timer, 0, 1);
                bwc.RT.sizeDelta = Vector2.Lerp(initSize, new Vector2(Screen.width, Screen.height), timer);
                bwc.RT.anchoredPosition = Vector2.Lerp(initPos, new Vector2(0, Screen.height), timer);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            while (timer < 1)
            {
                timer += Time.deltaTime * 10;
                timer = Mathf.Clamp(timer, 0, 1);
                bwc.RT.sizeDelta = Vector2.Lerp(new Vector2(Screen.width, Screen.height), initSize, timer);
                bwc.RT.anchoredPosition = Vector2.Lerp(new Vector2(0, Screen.height), initPos, timer);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
    #endregion

    #region Close Method
    public void CloseWindow()
    {
        BaseWindowController bwc = null;
        Transform tmpTrans = transform;

        while (bwc == null || tmpTrans == null)
        {
            tmpTrans = tmpTrans.parent;
            try { bwc = tmpTrans.gameObject.GetComponent<BaseWindowController>(); }
            catch (System.Exception) { }
        }

        Destroy(bwc.gameObject);
    }
    #endregion

    #region Pause Video method
    public void pauseVideo()
    {
        App_Video vid = null;
        Transform tmpTrans = transform;

        while (vid == null || tmpTrans == null)
        {
            tmpTrans = tmpTrans.parent;
            try { vid = tmpTrans.gameObject.GetComponent<App_Video>(); }
            catch (System.Exception) { }
        }

        vid.paused = !vid.paused;

        if (vid.paused)
        {
            vid.player.Pause();
        }
        else
        {
            vid.player.Play();
        }

    }
    #endregion

    #region Pause Audio method
    public void pauseAudio()
    {
        App_Audio audio = null;
        Transform tmpTrans = transform;

        while (audio == null || tmpTrans == null)
        {
            tmpTrans = tmpTrans.parent;
            try { audio = tmpTrans.gameObject.GetComponent<App_Audio>(); }
            catch (System.Exception) { }
        }

        audio.paused = !audio.paused;

        if (audio.paused)
        {
            audio.AS.Pause();
        }
        else
        {
            audio.AS.UnPause();
        }

    }
    #endregion

    #region calculator Button Methods

    [HideInInspector] public App_Calculator calculator;
    string operatorChars = "�x-+";

    public void calculatorInput()
    {
        if (calculator.outputText.text.Length > 0 && calculator.numbersString.Length == 0) { calculator.outputText.text = ""; }
        
        char inputChar = GetComponentInChildren<TextMeshProUGUI>().text[0];

        if (inputChar == 'C')
        {
            calculator.outputText.text = "";
            calculator.numbersString = "";
            calculator.operatorsString = "";
        }
        else if (inputChar == '=')
        {
            print("calculating...");
            calculateFromString();
        }
        else
        {
            bool isNumeric = !(operatorChars.Contains(inputChar));

            if (!isNumeric && calculator.outputText.text.Length==0) { return; }

            if (isNumeric)
            {
                calculator.numbersString += inputChar;
                if (calculator.operatorsString.Length != 0 && calculator.operatorsString[calculator.operatorsString.Length-1]!=':') { calculator.operatorsString += ':'; }
            }
            else
            {
                calculator.numbersString += ':';
                if (inputChar == '�') { inputChar = '/'; }
                if (inputChar == 'x') { inputChar = '*'; }
                calculator.operatorsString += inputChar;
            }
            
            calculator.outputText.text += inputChar;
        }
    }

    public void calculateFromString()
    {
        string expression = "";

        for (int i = 0; i < calculator.numbersString.Split(':').Length; i++)
        {
            expression += calculator.numbersString.Split(':')[i];
            expression += calculator.operatorsString.Split(':')[i];
        }

        DataTable dt = new DataTable();
        var answer = dt.Compute(expression, "").ToString();
        
        calculator.outputText.text = answer; //print solution

        //cleanup
        calculator.operatorsString = "";
        calculator.numbersString = "";
    }

    #endregion

    #region paint methods
    public void changeColor()
    {
        App_Paint paint = null;
        Transform tmpTrans = transform;

        while (paint==null || tmpTrans==null)
        {
            tmpTrans = tmpTrans.parent;
            try { paint = tmpTrans.gameObject.GetComponent<App_Paint>(); }
            catch (System.Exception) { }
        }

        paint.tipColor = defaultColor;
    }

    public void clearCanvas()
    {
        App_Paint paint = null;
        Transform tmpTrans = transform;

        while (paint == null || tmpTrans == null)
        {
            tmpTrans = tmpTrans.parent;
            try { paint = tmpTrans.gameObject.GetComponent<App_Paint>(); }
            catch (System.Exception) { }
        }

        foreach (Transform stroke in paint.GetComponentsInChildren<Transform>())
        {
            if (stroke.tag == "paintStroke") { Destroy(stroke.gameObject); }
        }

    }
    #endregion

    #region Email Client Methods

    public void DisplayEmail()
    {
        App_Email email = null;
        Transform tmpTrans = transform;

        while (email == null || tmpTrans == null)
        {
            tmpTrans = tmpTrans.parent;
            try { email = tmpTrans.gameObject.GetComponent<App_Email>(); }
            catch (System.Exception) { }
        }

        email.textBox.text = email.emails[transform.GetSiblingIndex()].text.Split('~')[1];
    }

    #endregion

    #region sceneManagment Methods
    public void setSceneIndex(int index)
    {
        SceneManagerScript.setIndex(index);
    }
    public void setSceneManagerOnOpaque()
    {
        FadeControllerScript.fade.onOpaque.AddListener(SceneManagerScript.loadSceneImmediate);
    }
    #endregion

    #region Button hovered Methods

    public void LerpToScale(string floatStr)
    {
        int x = int.Parse(floatStr.Split(',')[0]);
        int y = int.Parse(floatStr.Split(',')[1]);
        int spd = int.Parse(floatStr.Split(',')[2]);

        GetComponent<Image>().rectTransform.sizeDelta = Vector2.Lerp(GetComponent<Image>().rectTransform.sizeDelta, new Vector2(x, y), Time.deltaTime * spd);
    }

    public void setGlowOpacity(float f)
    {
        opacity = f;
    }
    float opacity;
    public void LerpImageTransparent(Image img)
    {
        img.color = Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, opacity), Time.deltaTime * 10);
    }

    float txtLerpSpd;
    float txtLerpOpacity;
    public void setTextLerpSpd(float f)
    {
        txtLerpSpd = f;
    }
    public void setTextLerpOpacity(float f)
    {
        txtLerpOpacity = f;
    }

    public void lerpTextColor(TextMeshProUGUI text)
    { 
        text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, txtLerpOpacity), Time.deltaTime * txtLerpSpd);
    }

    float txtCharacterSpacing;
    public void setTextCharacterSpacing(float f)
    {
        txtCharacterSpacing = f;
    }

    public void lerpTextSpacing(TextMeshProUGUI text)
    {
        text.characterSpacing = Mathf.Lerp(text.characterSpacing, txtCharacterSpacing, Time.deltaTime * txtLerpSpd);
    }

    #endregion

    #region Button click events
    public void setScale(float scale)
    {
        GetComponent<Image>().rectTransform.sizeDelta = GetComponent<Image>().rectTransform.sizeDelta * scale;
    }
    #endregion

    #region text methods
    public TextMeshProUGUI optionalSetText;

    public void setText(string str)
    {
        if (optionalSetText) { optionalSetText.text = str; }
    }
    #endregion

    #region taskbar tab methods
    public void focusWindow()
    {
        transform.parent.GetComponent<TaskBarTaskScript>().assignedWindow.transform.SetAsLastSibling();
    }
    #endregion

    public void enableBios()
    {
        GameObject.FindGameObjectWithTag("BIOSCanvas").GetComponent<BIOSTransfer>().activate();
    }
}
