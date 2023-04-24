using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeBlockoutReader : MonoBehaviour
{

    public TextAsset jsonToRead;
    NarrativeEventList NList;
    // Start is called before the first frame update
    void Start()
    {
        NList = JsonUtility.FromJson<NarrativeEventList>(jsonToRead.text);
    }

    private void OnValidate()
    {
        if (NList != null) { return; }
        NList = JsonUtility.FromJson<NarrativeEventList>(jsonToRead.text);
    }

    private void OnDrawGizmos()
    {
        foreach (NarrativeBlockoutTimelineObject n in NList.list)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(n.characterPosition,new Vector3(1,2,1));
            Gizmos.DrawLine(n.characterPosition+new Vector3(0,.75f,0), n.characterPosition + new Vector3(0, .75f, 0) + (n.headRotation*Vector3.forward));
            UnityEditor.Handles.Label(n.characterPosition,n.description);
        }
    }
}

[System.Serializable]
public class NarrativeEventList
{
    public List<NarrativeBlockoutTimelineObject> list;
}

[System.Serializable]
public class NarrativeBlockoutTimelineObject
{
    public Vector3 characterPosition;
    public Quaternion headRotation;
    public string description;
}
