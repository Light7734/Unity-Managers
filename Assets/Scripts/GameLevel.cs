using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct GameLevelScetion
{
    public string name;
    public string fullPath;
}


[CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
public class GameLevel : ScriptableObject
{
    [SerializeField] public GameLevelIndex index;

    [SerializeField] public string fullPath;
    [SerializeField] public string tagIdentifier;
    [SerializeField] public List<GameLevelScetion> sections = new List<GameLevelScetion> { };
}
