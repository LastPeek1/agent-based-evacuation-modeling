using UnityEngine;
public class Level_2_Manager : LevelManager
{
    private void Awake()
    {
        // ”никальные настройки дл€ уровн€ 2
        spawnAgentAreaMin = new Vector3(-1.44f, 0.92f, -26.61f);
        spawnAgentAreaMax = new Vector3(22.48f, 0.92f, 2.4f);

        //spawnAgentAreaMin = new Vector3(13.2200003f, 0.899999976f, -12.9300003f);
        //spawnAgentAreaMax = new Vector3(22.5900002f, 0.899999976f, -14.6199999f);

    }

}