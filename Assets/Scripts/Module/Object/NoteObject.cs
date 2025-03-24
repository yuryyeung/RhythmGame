using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteObject : ObjectBase
{
    public Note Note;
    public NoteType Type;
    public int Line;
    public bool Fever = false;
    public bool Skill = false;
    public int Slide = 0;
    public NoteObject LinkedSlide;

    public double SpawnTime;

    public enum NoteType
    {
        Action, Normal, Slide, SlideEnd, SlideEndFlick, Flick
    }

    public NoteObject(int measure, string channel, int point, int lengthOfBeat, string key)
    {
        
        this.Channel = channel;
        this.Measure = measure;
        this.Point = point;
        this.Key = key;
        this.LengthOfBeat = lengthOfBeat;
        MapType(Channel, Key);
    }

    public void SetMusicDetails(float bpm)
    {
        Timeslap = ((60 / (double)bpm * 4) * Measure) + (60 / (double)bpm * 4) * ((double)Point / LengthOfBeat);
        Debug.Log(Measure + " " + Type + "_" + Line + ": [" + Point + "/" + LengthOfBeat + "] - " + Timeslap + " " + Key);
    }

    public void MapType(string channel, string key)
    {
        Type = getType(int.Parse(channel[0].ToString()), key);
        Line = getLine(int.Parse(channel[1].ToString()));
        Debug.Log("Channel: " + channel + " | Key: " + key);
        if (Type != NoteType.Action) return;

    }

    NoteType getType(int id, string key)
    {
        NoteType type = default;
        switch (id)
        {
            case 0: type = NoteType.Action; break;
            case 1: type = NoteType.Normal; break;
            case 5: type = NoteType.Slide; break;
        }

        for (int i=0; i<15; i++)
        {
            if (key.Equals(BMSManager.ElementList[i].key))
            {
                switch (type)
                {
                    case NoteType.Normal:
                        switch (BMSManager.ElementList[i].value)
                        {
                            case BMSElement.ValueType.bd: type = NoteType.Normal; break;
                            case BMSElement.ValueType.flick: type = NoteType.Flick; break;
                            case BMSElement.ValueType.fever_note: Fever = true; type = NoteType.Normal; break;
                            case BMSElement.ValueType.fever_note_flick: Fever = true; type = NoteType.Flick; break;
                            case BMSElement.ValueType.slide_a: Slide = 1; type = NoteType.Slide; break;
                            case BMSElement.ValueType.slide_end_a: Slide = 1; type = NoteType.SlideEnd; break;
                            case BMSElement.ValueType.slide_b: Slide = 2; type = NoteType.Slide; break;
                            case BMSElement.ValueType.slide_end_b: Slide = 2; type = NoteType.SlideEnd; break;
                            case BMSElement.ValueType.skill: Skill = true; break;
                            case BMSElement.ValueType.slide_end_flick_a: Slide = 1; type = NoteType.SlideEndFlick; break;
                            case BMSElement.ValueType.slide_end_flick_b: Slide = 2; type = NoteType.SlideEndFlick; break;
                        }
                        break;
                    case NoteType.Slide:
                        Skill = true;
                        if (BMSManager.ElementList[i].value == BMSElement.ValueType.skill)
                            Skill = true;
                        break;
                }
                break;
            }
        }
        return type;
    }

    int getLine(int id)
    {
        int line = 0;
        switch (id)
        {
            case 1: line = 2; break;
            case 2: line = 3; break;
            case 3: line = 4; break;
            case 4: line = 5; break;
            case 5: line = 6; break;
            case 6: line = 1; break;
            case 8: line = 7; break;
        }

        return line;
    }

    public void SetNote(Note note, float gameSpeed, float lineSpeed, Transform parent, double currentTime, double spawnTime, float lineLength, System.Action returnAct)
    {
        this.Note = note;
        this.SpawnTime = spawnTime;
        Note.NoteObj.transform.SetParent(parent, false);
        Note.NoteObj.GetComponent<NoteAction>().Initialize(Type, lineSpeed, lineLength, returnAct);
        float prespawn = CommonMethods.DistanceByTime(spawnTime, currentTime, gameSpeed);
        Debug.Log("Current Time: " + (currentTime - spawnTime) + " | Timeslap: " + Timeslap + " | Prespawn: " + prespawn);
        Vector2 position = new Vector2(Note.NoteObj.GetComponent<RectTransform>().anchoredPosition.x, -prespawn);
        Debug.Log("Spawn Position: " + position);
        Note.NoteObj.GetComponent<RectTransform>().anchoredPosition = position;
        if (LinkedSlide != null)
        {
            float dist_y = Note.NoteObj.GetComponent<RectTransform>().anchoredPosition.y - LinkedSlide.Note.NoteObj.GetComponent<RectTransform>().anchoredPosition.y;
            float dist_x = Note.NoteObj.transform.parent.GetComponent<RectTransform>().anchoredPosition.x - LinkedSlide.Note.NoteObj.transform.parent.GetComponent<RectTransform>().anchoredPosition.x;
            float tan = (dist_x == 0) ? 0 : dist_y / dist_x;
            float Atan = Mathf.Atan(tan);

            Debug.LogWarning("Distance: (" + dist_x + ", " + dist_y + ") | Angle: " + Atan);
            //LinkedSlide.Note.SlideObj.sizeDelta = new Vector2(this.Note.SlideObj.sizeDelta.x, dist_y);
            LinkedSlide.SetSlide(-tan, dist_y);
        }
    }

    public void SetSlide(float rotation, float distance)
    {
        this.Note.SlideObj.sizeDelta = new Vector2(this.Note.SlideObj.sizeDelta.x, distance);
        this.Note.SlideObj.localEulerAngles = new Vector3(0, 0, rotation);
        //UnityEditor.EditorApplication.isPaused = true;
    }

    public void SetReferenceSlide(NoteObject headNote)
    {
        this.LinkedSlide = headNote;
    }
}
