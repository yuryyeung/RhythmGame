using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BMSManager
{
    enum ReadPhase
    {
        Null, Head, Data
    }

    public static List<BMSElement> ElementList = new List<BMSElement>();
    public static BMSObject BMSData;
    public static int BPM;
    public static bool IsReaded = false;
    public static AudioSource MusicSource;

    public static void Initialize(string text, AudioSource musicPlayer)
    {
        ReadPhase phase = ReadPhase.Null;
        MusicSource = musicPlayer;
        string[] textline = text.Split('\n');
        BMSData = new BMSObject();

        int lineCount = 0;
        foreach (string line in textline)
        {
            lineCount++;
            if (string.IsNullOrEmpty(line)) continue;
            if (line[0].Equals('*'))
            {
                if (line.Contains("HEADER FIELD"))
                {
                    phase = ReadPhase.Head;
                    continue;
                }
                if (line.Contains("MAIN DATA FIELD"))
                {
                    phase = ReadPhase.Data;
                    continue;
                }
            }

            if (line[0].Equals('#'))
            {
                switch (phase)
                {
                    case ReadPhase.Head:
                        if (line.Contains("BPM")) {
                            string[] splits = line.Split(' ');
                            BMSData.BPM = int.Parse(splits[1]); break;
                        }
                        if (line.Contains("#WAV"))
                        {
                            string[] element = line.Split(' ');
                            ElementList.Add(new BMSElement(element[0].Remove(0, 4), element[1].Remove(element[1].IndexOf('.'))));
                        }
                        if (line.Contains("WAV01")) { 
                            BMSData.BGM = Resources.Load<AudioClip>("Sound/" + line.Split(' ')[1].Remove(line.Split(' ')[1].IndexOf('.')));
                            MusicSource.clip = BMSData.BGM;
                        }

                        break;
                    case ReadPhase.Data:
                        string[] data = line.Split(':');
                        string measure = data[0].Substring(1, 3);
                        string channel = data[0].Substring(4, 2);
                        IEnumerable<string> notes = Enumerable.Range(0, data[1].Length / 2)
                            .Select(i => data[1].Substring(i * 2, 2));
                        for (int i = 0; i < notes.ToList().Count(); i++)
                        {
                            string point = notes.ToList()[i];
                            if (!point.Contains("00"))
                            {
                                for (int j=0; j< ElementList.Count; j++)
                                {
                                    if (ElementList[j].key.Equals(point))
                                    {
                                        if (ElementList[j].value == BMSElement.ValueType.bgm ||
                                            ElementList[j].value == BMSElement.ValueType.cmd_fever_ready || 
                                            ElementList[j].value == BMSElement.ValueType.cmd_fever_start || 
                                            ElementList[j].value == BMSElement.ValueType.cmd_fever_checkpoint || 
                                            ElementList[j].value == BMSElement.ValueType.cmd_fever_end)
                                        {
                                            BMSData.AddAction(int.Parse(measure), channel, i, data[1].Length / 2, point);
                                        } else BMSData.AddNote(int.Parse(measure), channel, i, data[1].Length / 2, point);
                                    }
                                }
                            }
                        }
                        break;
                }
            }

        }

        IEnumerable<ObjectBase> query = BMSData.NoteList.OrderBy(note => note.Timeslap);
        BMSData.NoteList = query.ToList();

        Debug.Log("Total Note Count: " + BMSData.NoteList.Count + " | Line Count: " + lineCount + "/" + (textline.Length));
        if (lineCount == textline.Length) IsReaded = true;
    }

    public static void Reset()
    {
        ElementList.Clear();
        BMSData = null;
        IsReaded = false;
    }
}
