using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoteManager
{
    public static List<Note> NoteList = new List<Note>();

    public static void Initialize(List<Note> note)
    {
        NoteList = note;
    }
    
    public static Note GiveNote()
    {
        foreach (Note note in NoteList)
        {
            if (!note.InUsed)
            {
                note.InUsed = true;
                return note;
            }
        }
        return null;
    }
    
    public static void ReturnNote(Note note)
    {
        note.InUsed = false;
        note.NoteObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        note.NoteObj.transform.SetParent(note.returnPos, false);
        note.SlideObj.sizeDelta = new Vector2(note.SlideObj.sizeDelta.x, 0);
    }
}
