using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBarTaskScript : MonoBehaviour
{

    public GameObject assignedWindow;
    GameObject tabContainer;

    RectTransform rectTransform;

    int tabContainerWidth;

    public float tabWidth = 250;

    // Start is called before the first frame update
    void Start()
    {
        tabContainer = transform.parent.gameObject;
        tabContainerWidth = (int)tabContainer.GetComponent<RectTransform>().sizeDelta.x;

        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        int tabCount = tabContainer.transform.childCount;

        tabWidth = tabContainerWidth / tabCount;
        tabWidth = Mathf.Clamp(tabWidth,38,250);

        rectTransform.anchoredPosition = new Vector2(transform.GetSiblingIndex() * tabWidth,0);

        Vector2 tabSizeDelta = new Vector2(tabWidth,rectTransform.sizeDelta.y);
        rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, tabSizeDelta, Time.deltaTime * 20);

        if (assignedWindow == null) { Destroy(this.gameObject); }
    }
}
