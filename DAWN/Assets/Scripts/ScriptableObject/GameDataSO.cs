using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSO", menuName = "GameDataSO")]
public class GameDataSO : ScriptableObject
{
    public int currentBalloon = 0;
    public void Init()
    {
        currentBalloon = 0;
    }
}
