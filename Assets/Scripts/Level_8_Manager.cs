using UnityEngine;

public class Level_8_Manager : LevelManager
{
    private void Awake()
    {
        // ���������� ��������� ��� ������ 1
        spawnAgentAreaMin = new Vector3(-1.49000001f, 0.92f, 2.55999994f);
        spawnAgentAreaMax = new Vector3(28.2099991f, 0.92f, -5.61999989f);
    }
}