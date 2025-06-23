using UnityEngine;

public class Level_1_Manager : LevelManager
{
    private void Awake()
    {
        // ”никальные настройки дл€ уровн€ 1
        spawnAgentAreaMin = new Vector3(-0.35f, 0.92f, -11.88f);
        spawnAgentAreaMax = new Vector3(9.09f, 0.92f, 1.59f);
    }
}