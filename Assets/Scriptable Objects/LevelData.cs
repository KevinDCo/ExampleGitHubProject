using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "SCRB_LevelData", menuName = "Data/LevelData", order = 1)]
public class LevelData : SerializedScriptableObject
{
    [Header("Level Settings")]
    [SerializeField] string levelTitle = "";
    public string LevelTitle => levelTitle;
    [SerializeField] string contextTitle = "";
    public string ContextTitle => contextTitle;
    [SerializeField] int levelID = 1;
    public int LevelID => levelID;
   
    public override bool Equals(object other){
        if (other == null || !GetType().Equals(other.GetType()))
            return false;
        else
        {
            LevelData otherLevel = (LevelData)other;
            return (otherLevel.levelTitle == levelTitle);
        }
    }

    public override int GetHashCode(){
        return levelTitle.GetHashCode();
    }
}