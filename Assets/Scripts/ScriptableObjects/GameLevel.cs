using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
[System.Serializable]
public class GameLevel : ScriptableObject
{
    [System.Serializable]
    public struct GameLevelScetion
    {
        public string name;
        public string fullPath;
    }

    [SerializeField] public GameLevelIndex index;

    [SerializeField] public string fullPath;
    [SerializeField] public string tagIdentifier;
    [SerializeField] public List<GameLevelScetion> sections = new List<GameLevelScetion> {};
}
