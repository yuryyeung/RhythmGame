using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InputField SpeedField;
    public Dropdown BeatmapDrop;
    public Button StartBtn, ResetBtn;
    public GameManager GameManager;
    public Text ComboCounter;
    public int comboCount;
    public float timeCount = 0;

    public List<Image> Judges = new List<Image>();

    [Header("Private Element")]
    [Range(1, 11)] private float _speed;

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR 
        BeatmapDrop.options.Clear();
        string path = Application.dataPath + "/Resources/Beatmaps";
        string[] filePaths = Directory.GetFiles(path);
        List<string> rawFiles = new List<string>();
        rawFiles.AddRange(filePaths);
        rawFiles.RemoveAll(_sortList);
        List<string> beatmaps = new List<string>();

        for (int i = 0; i < rawFiles.Count; i++)
        {
            string[] splits = rawFiles[i].Split('\\');
            string beatmap = splits[splits.Length - 1];
            beatmap = beatmap.Remove(beatmap.IndexOf('.'));
            beatmaps.Add(beatmap);
        }
        BeatmapDrop.AddOptions(beatmaps);
#endif

        //Game Speed
        float speed = float.Parse(SpeedField.text);
        speed = Mathf.Clamp(speed, 1, 11);
        _speed = speed;
        GameManager.speed = _speed;
    }

    private bool _sortList(string value) {return value.Contains(".meta");}

    void Start()
    {
        SpeedField.onValueChanged.AddListener((raw) => {
            float value = float.Parse(raw);
            value = Mathf.Clamp(value, 1, 11);
            _speed = value;
            GameManager.speed = _speed;
            SpeedField.text = value.ToString();
        });
        StartBtn.onClick.AddListener(() => {
            GameManager.Initialize(BeatmapDrop.options[BeatmapDrop.value].text);
        });
        ResetBtn.onClick.AddListener(() => { 
            GameManager.Reset();
            ShowJudge(4);
            Reset();
        });
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        ComboCounter.text = comboCount.ToString();
        if (timeCount > 3) ShowJudge(4);
    }

    public void ShowJudge(int status)
    {
        timeCount = 0;
        for (int i=0; i<Judges.Count; i++)
        {
            Judges[i].color = (i == status) ? Color.white : Color.clear;
        }
    }

    private void Reset()
    {
        comboCount = 0;
    }
}
