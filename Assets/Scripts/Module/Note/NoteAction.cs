using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NoteAction : MonoBehaviour
{
    public int Line;

    [Range(1, 11)]
    public float Speed;
    public bool IsStart = false;
    public float LineLength = 0;

    public NoteObject.NoteType NoteType;
    public Color[] NoteColor;

    public Action ReturnAction;

    private float time;

    public void Initialize(NoteObject.NoteType type, float speed, float lineLength, Action returnAct)
    {
        this.NoteType = type;
        this.Speed = speed;
        this.ReturnAction = returnAct;
        this.LineLength = lineLength;
        switch (NoteType) 
        {
            case NoteObject.NoteType.Normal:
                this.GetComponent<Image>().color = NoteColor[0];
                break;
            case NoteObject.NoteType.Slide:
                this.GetComponent<Image>().color = NoteColor[1];
                break;
            case NoteObject.NoteType.SlideEnd:
                this.GetComponent<Image>().color = NoteColor[1];
                break;
            case NoteObject.NoteType.SlideEndFlick:
                this.GetComponent<Image>().color = NoteColor[2];
                break;
            case NoteObject.NoteType.Flick:
                this.GetComponent<Image>().color = NoteColor[3];
                break;
        }
        IsStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStart) return;
        time += Time.deltaTime;
        this.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, Time.deltaTime * Speed);
        if (this.GetComponent<RectTransform>().anchoredPosition.y <= -LineLength - 500)
        {
            IsStart = false;
            //Debug.Log("Note Live: " + time + "s");
            time = 0;
            ReturnAction?.Invoke();
        }
    }
}
