using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Object
{
    public class BMSElement
    {
        public string key;
        public enum ValueType
        {
            bgm, bd, flick, fever_note, fever_note_flick, cmd_fever_ready,
            cmd_fever_start, cmd_fever_checkpoint, cmd_fever_end, slide_a,
            slide_end_a, slide_b, slide_end_b, skill, slide_end_flick_a, slide_end_flick_b,
            tempo
        }

        public ValueType value;

        public BMSElement(string key, string value)
        {
            this.key = key;
            for (int i = 0; i <= 15; i++)
            {
                ValueType type = (ValueType)i;
                if (value.Contains(type.ToString()))
                {
                    this.value = type;
                    break;
                }
            }
        }
    }

}