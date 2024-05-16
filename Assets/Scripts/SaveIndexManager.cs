using UnityEngine;

public class SceneIndexManager : MonoBehaviour
{
    private const string LAST_SCENE_INDEX_KEY = "last_scene_index";

    private int lastSceneIndex;

    [SerializeField] TimerScript timer;

    private void Awake()
    {
        SoundManager.Initialize();
        timer = GetComponent<TimerScript>();


    }

    private void Start()
    {
        LoadLastSceneIndex();
        GameManager.Instance.UpdateEnemyCount();
    }

    private void LoadLastSceneIndex()
    {
        lastSceneIndex = PlayerPrefs.GetInt(LAST_SCENE_INDEX_KEY, 0);
    }

    public int GetLastSceneIndex()
    {
        return lastSceneIndex;
    }

    public void SetLastSceneIndex(int sceneIndex)
    {
        lastSceneIndex = sceneIndex;
        PlayerPrefs.SetInt(LAST_SCENE_INDEX_KEY, lastSceneIndex);
        PlayerPrefs.Save();
        timer.getTime();
        timer.ResetTimer();
        timer.SetActivateTimer(true);

    }
}
