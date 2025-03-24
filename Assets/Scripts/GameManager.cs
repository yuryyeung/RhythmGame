using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public string beatmap;
    public float speed = 8.5f;
    public float LineLength = 0;

    public List<ObjectBase> ObjectList = new List<ObjectBase>();
    public double TimeCount;

    [Header("GameObject")] 
    public float Prespawn;
    public GameObject GeneralNote;
    public Transform PrespawnArea;
    public List<Transform> Lines = new List<Transform>();
    public AudioSource AudioSource;
    public MapDisplayManager MapDisplayManager;
    public TouchManager TouchManager;

    [SerializeField] 
    private List<ObjectBase> usingNote = new List<ObjectBase>();
    [SerializeField] 
    private double startTime;
    [SerializeField] 
    private bool isStart = false;
    //[SerializeField]
    //private float _speed = 0;

    [Header("Debug Element")]
    public float CurrentFps = 0;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        //QualitySettings.vSyncCount = 1;
        SetNote();
        this.LineLength = Lines[0].transform.parent.GetComponent<RectTransform>().sizeDelta.y - 100;
        float lineSpeed = speed.Remap(1, 11, LineLength / 10, LineLength / 2);
        Debug.Log((lineSpeed / LineLength) + " " +
            Time.deltaTime * lineSpeed + " " +
            CommonMethods.PreSpawnCalculation(lineSpeed, LineLength) + " " + 
            (1/Time.deltaTime));
        //_speed = speed.Remap(1, 11, 2462.5f, 7387.5f);
        //BMSManager.Initialize(Resources.Load<TextAsset>("Beatmaps/" + beatmap).text, AudioSource);
    }

    public void Initialize(string value)
    {
        Debug.Log("Start Loading: " + value);
        BMSManager.Initialize(Resources.Load<TextAsset>("Beatmaps/" + value).text, AudioSource);
    }

    void SetNote()
    {
        List<Note> notes = new List<Note>();
        for (int i = 0; i < Prespawn + 1; i++)
        {
            GameObject spawnObj = GameObject.Instantiate(GeneralNote);
            spawnObj.name += i.ToString("000");
            spawnObj.transform.SetParent(PrespawnArea, false);
            notes.Add(new Note(spawnObj, PrespawnArea));
        }
        NoteManager.Initialize(notes);
    }

    IEnumerator delayStart(System.Action action)
    {
        yield return new WaitForSeconds(1);
        action?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentFps = 1 / Time.deltaTime;
        TimeCount = AudioSettings.dspTime;
        if (BMSManager.IsReaded && !isStart)
        {
            ObjectList = BMSManager.BMSData.NoteList;
            startTime = AudioSettings.dspTime;
            MapDisplayManager.Initialize(ObjectList, speed, LineLength);
            TouchManager.Initialize(ObjectList, startTime);

            isStart = true;
            StartCoroutine(delayStart(() => {
                MapDisplayManager.Play();
            }));
            return;
        }

        // Pause for note checking
        //UnityEditor.EditorApplication.isPaused = true;
    }

    public void Reset()
    {
        ObjectList.Clear();
        BMSManager.Reset();
        TouchManager.Reset();
        MapDisplayManager.Reset();
        AudioSource.Stop();
        isStart = false;
    }
}
