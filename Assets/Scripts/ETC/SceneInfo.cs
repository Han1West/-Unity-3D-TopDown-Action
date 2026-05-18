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
}
