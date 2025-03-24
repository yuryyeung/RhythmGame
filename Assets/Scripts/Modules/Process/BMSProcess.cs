using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Object;

namespace Game.Process
{
    public static class BMSProcess
    {
        public static List<NoteObject> StraightSlideList = new List<NoteObject>();

        public static NoteObject AddNote(int measure, string channel, int point, int lengthOfBeat, string key, int bpm)
        {
            NoteObject note = new NoteObject(measure, channel, point, lengthOfBeat, key);

            note.SetMusicDetails(bpm);
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
                            NoteObject removeNote = StraightSlideList[i];
                            StraightSlideList.Remove(StraightSlideList[i]);
                            return note;
                        }
                    }
                    if (note.Channel[0].ToString().Equals("5")) { StraightSlideList.Add(note); }
                    return note;
                }
                else return note;
            }
            else
            {
                if (note.Channel[0].ToString().Equals("5")) { StraightSlideList.Add(note); }
                return note;
            }
        }

        public static ActionObject AddAction(int measure, string channel, int point, int lengthOfBeat, string key, int bpm)
        {
            ActionObject action = new ActionObject(measure, channel, point, lengthOfBeat, key);
            action.SetMusicDetails(bpm);
            return action;
        }
    }
}
