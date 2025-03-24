using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Object
{
    public class BMSObject : ScriptableObject
    {
        public int BPM = 0;
        public int Level = 0;
        public AudioClip BGM;
        public List<ObjectBase> NoteList = new List<ObjectBase>();
        public List<SoundObject> KeyList = new List<SoundObject>();
    }
}
