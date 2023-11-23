using UnityEngine;

[CreateAssetMenu(fileName = "Level Template", menuName = "Scriptable Object/Level Template", order = int.MaxValue)]
public class LevelTemplate : ScriptableObject
{
    public int levelNumber = -1;
    public string levelName = "Insert File Name Here";
    public int fullScore = 0;
}
