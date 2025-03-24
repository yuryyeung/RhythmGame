using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplayManager : MonoBehaviour
{
    public bool IsStart = false;
    public float LineLength = 0;
    public List<ObjectBase> ObjectList = new List<ObjectBase>();

    [Header("GameObject")]
    public List<Transform> Lines = new List<Transform>();
    public AudioSource AudioSource;

    [Header("SpawnNote")]
    public List<NoteObject> spawnNote = new List<NoteObject>();

    [SerializeField]
    public double TimeCount = 0;
    [SerializeField]
    private List<ObjectBase> usingNote = new List<ObjectBase>();
    [SerializeField]
    private List<NoteObject> slideNote = new List<NoteObject>();
    [SerializeField]
    private int _startedslide = 0;
    [SerializeField]
    private double startTime;
    [SerializeField]
    private bool isStart = false;
    [SerializeField]
    private float _gameSpeed = 0;
    [SerializeField]
    private float _lineSpeed = 0;

    public void Initialize(List<ObjectBase> objList, float speed, float LineLength)
    {
        ObjectList.AddRange(objList.ToArray());
        this.LineLength = LineLength;
        _gameSpeed = speed;
        _lineSpeed = _gameSpeed.Remap(1, 11, LineLength / 10, LineLength / 2);
    }

    public void Play()
    {
        Debug.Log("Start Beatmap");
        startTime = AudioSettings.dspTime;
        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            TimeCount = AudioSettings.dspTime;
            int spawnCount = 0;
            float prespawnTime = CommonMethods.PreSpawnCalculation(_lineSpeed, LineLength);
            double currentTime = 0;
            for (int i = 0; i < ObjectList.Count; i++)
            {
                if (ObjectList[i] is NoteObject)
                {
                    currentTime = startTime + ObjectList[i].Timeslap - prespawnTime;
                    if (TimeCount >= currentTime)
                    {
                        spawnCount++;

                        Debug.Log("Current Time: " + TimeCount + " | Timeslap: " + currentTime);
                        NoteObject noteObj = (NoteObject)ObjectList[i];
                        Debug.LogWarning("SetNote Number: " + noteObj.Type.ToString() + "_" + i + " | Timeslap: " + noteObj.Timeslap + " | Current: " + TimeCount);
                        spawnNote.Add(noteObj);
                    }
                }

                if (ObjectList[i] is ActionObject)
                {
                    if (TimeCount > (startTime + ObjectList[i].Timeslap))
                    {
                        ActionObject actionObj = (ActionObject)ObjectList[i];
                        actionObj.DoAction();
                        usingNote.Add(actionObj);
                        ObjectList.Remove(ObjectList[i]);
                    }
                }
                if (startTime + ObjectList[i].Timeslap - prespawnTime > TimeCount + 5) break;
            }

            //Debug.Log("Spawned Note: " + spawnCount);
            //if (spawnCount > 1) UnityEditor.EditorApplication.isPaused = true;
            spawnCount = 0;
        }
    }

    private void LateUpdate()
    {
        float prespawnTime = CommonMethods.PreSpawnCalculation(_lineSpeed, LineLength);
        double currentTime = 0;
        for (int i=0; i< spawnNote.Count; i++)
        {
            NoteObject noteObj = spawnNote[i];
            currentTime = startTime + noteObj.Timeslap - prespawnTime;
            if (noteObj.Note == null)
            {
                noteObj.SetNote(NoteManager.GiveNote(), _gameSpeed, _lineSpeed, Lines[noteObj.Line - 1], TimeCount, currentTime, LineLength, () =>
                {
                    NoteManager.ReturnNote(noteObj.Note);
                    usingNote.Remove(noteObj);
                });
            }
            usingNote.Add(noteObj);
            if (noteObj.Type == NoteObject.NoteType.Slide)
            {
                _startedslide++;
                slideNote.Add(noteObj);
            }

            ObjectList.Remove(noteObj);
            /*if (_startedslide != 0)
            {
                for (int j = 0; j < ObjectList.Count; j++)
                {
                    NoteObject slideNote = (NoteObject)ObjectList[j];
                    if (slideNote.Type == NoteObject.NoteType.Slide ||
                        slideNote.Type == NoteObject.NoteType.SlideEnd ||
                        slideNote.Type == NoteObject.NoteType.SlideEndFlick)
                    {
                        if (noteObj.Slide == slideNote.Slide)
                        {
                            int line = noteObj.Line - slideNote.Line;
                            Debug.LogWarning("Current Detail: " + noteObj.Measure + " " + noteObj.Timeslap);
                            Debug.LogWarning("SlideNote Detail: " + slideNote.Measure + " " + slideNote.Timeslap);
                            float distance = Mathf.Abs(CommonMethods.DistanceByTime(noteObj.Timeslap, slideNote.Timeslap, _lineSpeed));
                            Debug.LogWarning("Line Different: " + line + " | Striaght Distance: " + distance);
                            noteObj.SetSlide(0, distance);
                            slideNote.SetReferenceSlide(noteObj);
                            ObjectList[j] = slideNote;
                            _startedslide--;
                            break;
                        }
                    }
                }
            }*/
        }
        spawnNote.Clear();
    }

    public void Reset()
    {
        isStart = false;
        usingNote.Clear();
        slideNote.Clear();
        ObjectList.Clear();
    }
}
