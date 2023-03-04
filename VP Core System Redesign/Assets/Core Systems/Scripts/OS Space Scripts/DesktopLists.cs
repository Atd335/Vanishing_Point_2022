using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopLists : MonoBehaviour
{

    public FileList[] lists;
    public static FileList listToLoad;

    public void setDesktopToLoad(int index)
    {
        listToLoad = lists[index];
    }

    public void setDesktopToLoad(FileList list)
    {
        listToLoad = list;
        print(listToLoad);
    }
}
