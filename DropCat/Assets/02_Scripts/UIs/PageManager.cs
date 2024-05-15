using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public void EnablePage(ref GameObject[] pageList, int index)
    {
        int i = 0;
        foreach(GameObject page in pageList)
        {
            if (i == index)
                page.SetActive(true);
            else
                page.SetActive(false);
            ++i;
        }
    }

    public void DisablePage()
    {
        this.gameObject.SetActive(false);
    }
}
