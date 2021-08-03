using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
[System.Serializable]
public class GameLevel : ScriptableObject
{
    [System.Serializable]
    public struct Section
    {
        public string name;
        public string fullPath;
    }

    [SerializeField] public GameManager.LevelIndex index;

    [SerializeField] public string fullPath;
    [SerializeField] public string tagIdentifier;
    [SerializeField] public List<Section> sections = new List<Section> { };
}
