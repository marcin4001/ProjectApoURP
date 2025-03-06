using System.Collections.Generic;
using UnityEngine;

public class OutlineList : MonoBehaviour
{
    public List<Outline> outlines = new List<Outline>();


    public void Show(bool show)
    {
        foreach (Outline outline in outlines)
        {
            outline.enabled = show;

        }
    }
}
