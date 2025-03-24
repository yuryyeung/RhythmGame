using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note
{
    public GameObject NoteObj;
    public RectTransform SlideObj;
    public Transform returnPos;

    public bool InUsed = false;

    public Note(GameObject note, Transform returnPos)
    {
        this.NoteObj = note;
        this.returnPos = returnPos;
        this.SlideObj = NoteObj.transform.GetChild(0).GetComponent<RectTransform>();
    }
}
