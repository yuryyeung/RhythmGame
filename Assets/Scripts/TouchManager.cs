using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{
    [Min(0.1f)]
    public float hitSecond = 1;
    public List<NoteObject> NoteList = new List<NoteObject>();
    public GameManager GameManager;
    public UIManager UIManager;
    public MapDisplayManager MapDisplayManager;
    [SerializeField]
    private List<NoteObject> SpawnedNotes = new List<NoteObject>();

    [SerializeField]
    private double startTime;

    void Start()
    {
        
    }

    public void Initialize(List<ObjectBase> rawList, double startTime)
    {
        List<ObjectBase> newList = rawList;
        newList.RemoveAll(_sloting);
        this.startTime = startTime;
        for (int i = 0; i < newList.Count; i++) NoteList.Add((NoteObject)newList[i]);
    }

    bool _sloting(ObjectBase obj) { return obj is ActionObject; }

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length > 0)
        {
            Debug.Log("Running with " + Input.touches.Length + " Touche(s)");
            for (int i=0; i<Input.touches.Length; i++)
            {
                Touch touch = Input.touches[i];
                List<RaycastResult> results = GetPointerEvent(touch);
                if (results.Count == 0) continue;
                Debug.Log("Hit Result: " + results.Count);
                foreach (RaycastResult result in results)
                {
                    GameObject hitObject = result.gameObject;
                    if (result.gameObject.name.Contains("TouchArea")) hitObject = hitObject.transform.parent.gameObject;

                    Debug.Log(hitObject.name);
                    int line = int.Parse(hitObject.name.Remove(0, hitObject.name.IndexOf('_') + 1));
                    
                    for (int j=0; j<SpawnedNotes.Count; j++)
                    {
                        if (SpawnedNotes[j].Line != line) continue;
                        if (MapDisplayManager.TimeCount < (startTime + SpawnedNotes[j].Timeslap + hitSecond / 2) &&
                            MapDisplayManager.TimeCount > (startTime + SpawnedNotes[j].Timeslap - hitSecond / 2))
                        {
                            Debug.LogWarning("Hit Success");
                            Judgement(SpawnedNotes[j]);
                            RemoveNote(SpawnedNotes[j]);
                        }
                        //if (SpawnedNotes[j].Line);
                    }
                    break;
                }
            }
        }
    }

    public void Judgement(NoteObject note)
    {
        switch (note.Type)
        {
            case NoteObject.NoteType.Normal:
                if (MapDisplayManager.TimeCount < (startTime + note.Timeslap + hitSecond / 1) &&
                    MapDisplayManager.TimeCount > (startTime + note.Timeslap - hitSecond / 1))
                {
                    Debug.LogWarning("Hit Success");
                    UIManager.ShowJudge(0);
                    if (MapDisplayManager.TimeCount < (startTime + note.Timeslap + hitSecond / 1.5) &&
                        MapDisplayManager.TimeCount > (startTime + note.Timeslap - hitSecond / 1.5))
                    {
                        UIManager.ShowJudge(1);
                        UIManager.comboCount++;
                        if (MapDisplayManager.TimeCount < (startTime + note.Timeslap + hitSecond / 2) &&
                            MapDisplayManager.TimeCount > (startTime + note.Timeslap - hitSecond / 2))
                        {
                            UIManager.ShowJudge(2);
                        }
                    }
                }
                break;
            case NoteObject.NoteType.Slide:
            case NoteObject.NoteType.SlideEnd:
            case NoteObject.NoteType.SlideEndFlick:
                if (MapDisplayManager.TimeCount < (startTime + note.Timeslap + hitSecond / 2) &&
                    MapDisplayManager.TimeCount > (startTime + note.Timeslap - hitSecond / 2))
                {
                    Debug.LogWarning("Hit Success");
                    UIManager.comboCount++;
                    UIManager.ShowJudge(2);
                }
                break;
            case NoteObject.NoteType.Flick:
                if (MapDisplayManager.TimeCount < (startTime + note.Timeslap + hitSecond / 1) &&
                    MapDisplayManager.TimeCount > (startTime + note.Timeslap - hitSecond / 1))
                {
                    Debug.LogWarning("Hit Success");
                    UIManager.ShowJudge(0);
                    if (MapDisplayManager.TimeCount < (startTime + note.Timeslap + hitSecond / 1.5) &&
                        MapDisplayManager.TimeCount > (startTime + note.Timeslap - hitSecond / 1.5))
                    {
                        UIManager.ShowJudge(1);
                        UIManager.comboCount++;
                        if (MapDisplayManager.TimeCount < (startTime + note.Timeslap + hitSecond / 2) &&
                            MapDisplayManager.TimeCount > (startTime + note.Timeslap - hitSecond / 2))
                        {
                            UIManager.ShowJudge(2);
                        }
                    }
                }
                break;
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < NoteList.Count; i++)
        {
            if (MapDisplayManager.TimeCount >= (NoteList[i].Timeslap + startTime - hitSecond * 2))
            {
                SetNote(NoteList[i]);
            }
        }
        for (int j = 0; j < SpawnedNotes.Count; j++)
        {
            if (MapDisplayManager.TimeCount >= (SpawnedNotes[j].Timeslap + startTime + hitSecond * 2))
            {
                RemoveNote(SpawnedNotes[j]);
            }
        }
    }

    public void SetNote(NoteObject note)
    {
        if (!NoteList.Contains(note)) return;
        NoteList.Remove(note);
        SpawnedNotes.Add(note);
    }

    public void RemoveNote(NoteObject note)
    {
        if (!SpawnedNotes.Contains(note)) return;
        SpawnedNotes.Remove(note);
    }

    List<RaycastResult> GetPointerEvent(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results;
    }

    public void Reset()
    {
        NoteList.Clear();
        SpawnedNotes.Clear();
    }
}
