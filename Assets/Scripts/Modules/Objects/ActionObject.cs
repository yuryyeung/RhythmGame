using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Object
{
    public class ActionObject : ObjectBase
    {
        public ActionType Type;
        public FeverPhase Phase;
        public SoundObject Sound;

        public enum ActionType
        {
            BGM, Effect, Fever, Tempo
        }

        public enum FeverPhase
        {
            Null, Ready, Start, Checkpoint, End
        }

        public ActionObject(int measure, string channel, int point, int lengthOfBeat, string key)
        {
            this.Channel = channel;
            this.Measure = measure;
            this.Point = point;
            this.Key = key;
            this.LengthOfBeat = lengthOfBeat;
            MapType(Key);
        }

        public void MapType(string key)
        {
            Type = getType(key);
            if (Type == ActionType.Fever) Phase = getFever(key);
        }

        public void SetMusicDetails(float bpm)
        {
            Timeslap = ((60 / (double)bpm * 4) * Measure) + (60 / (double)bpm * 4) * ((double)Point / LengthOfBeat);
            Debug.Log(Measure + " " + Type + ": [" + Point + "/" + LengthOfBeat + "] - " + Timeslap + " " + Key);
        }

        ActionType getType(string key)
        {
            ActionType type = ActionType.Effect;
            if (key.Equals("01")) type = ActionType.BGM;
            else if (key.Equals("0C")) type = ActionType.Fever;
            else if (key.Equals("0D")) type = ActionType.Fever;
            else if (key.Equals("0E")) type = ActionType.Fever;
            else if (key.Equals("0F")) type = ActionType.Fever;
            return type;
        }

        FeverPhase getFever(string key)
        {
            FeverPhase type = FeverPhase.Null;
            if (key.Equals("0C")) type = FeverPhase.Ready;
            else if (key.Equals("0D")) type = FeverPhase.Start;
            else if (key.Equals("0E")) type = FeverPhase.Checkpoint;
            else if (key.Equals("0F")) type = FeverPhase.End;
            return type;
        }

        public void DoAction()
        {
            switch (Type)
            {
                case ActionType.BGM:
                    Debug.Log("Start BGM");
                    BMSManager.MusicSource.Play();
                    break;
                case ActionType.Fever:
                    break;
            }
        }
    }
}
