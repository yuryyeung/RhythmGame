using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BMSObject
{
    public int BPM = 0;
    public int Level = 0;
    public AudioClip BGM;
    public List<ObjectBase> NoteList = new List<ObjectBase>();
    public List<NoteObject> StraightSlideList = new List<NoteObject>();

    public BMSObject()
    {

    }

    public void AddNote(int measure, string channel, int point, int lengthOfBeat, string key)
    {
        NoteObject note = new NoteObject(measure, channel, point, lengthOfBeat, key);
        note.SetMusicDetails(BPM);
        if (StraightSlideList.Count > 0)
        {
            if (note.Channel[0].ToString().Equals("5"))
            {
                for (int i = 0; i < StraightSlideList.Count; i++)
                {
                    if (note.Channel == StraightSlideList[i].Channel)
                    {
                        Debug.Log("Change: " + measure + " " + note.Type + "_" + note.Line + " " + note.Timeslap + " to - SlideEnd");
                        note.Type = NoteObject.NoteType.SlideEnd;
                        NoteList.Add(note);
                        NoteObject removeNote = StraightSlideList[i];
                        StraightSlideList.Remove(StraightSlideList[i]);
                        Debug.Log(NoteList.Contains(removeNote));
                        return;
                    }
                }
                if (note.Channel[0].ToString().Equals("5")) { StraightSlideList.Add(note); }
                NoteList.Add(note);
            }
            else NoteList.Add(note);
        }
        else
        {
            if (note.Channel[0].ToString().Equals("5")) { StraightSlideList.Add(note); }
            NoteList.Add(note);
        }
    }

    public void AddAction(int measure, string channel, int point, int lengthOfBeat, string key)
    {
        ActionObject action = new ActionObject(measure, channel, point, lengthOfBeat, key);
        action.SetMusicDetails(BPM);
        NoteList.Add(action);
    }
}
