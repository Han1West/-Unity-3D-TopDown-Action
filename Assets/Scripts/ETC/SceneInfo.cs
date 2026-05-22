using UnityEngine;

public enum SceneType
{
    Lobby,    
    InGamePlay,
    CutScene,
}

public class SceneInfo : MonoBehaviour
{
    [SerializeField] public SceneType sceneType;
    [SerializeField] AudioClip bgm;

    void Start()
    {
        if (bgm)
            AudioManager.Instance.PlayBGM(bgm);
        else
            AudioManager.Instance.StopBGM();
    }
}
