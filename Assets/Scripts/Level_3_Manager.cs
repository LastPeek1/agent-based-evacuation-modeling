using UnityEngine;
public class Level_3_Manager : LevelManager
{
    private void Awake()
    {
        // ���������� ��������� ��� ������ 3
        spawnAgentAreaMin = new Vector3(-1.46000004f, 0.92f, 2.23000002f);
        spawnAgentAreaMax = new Vector3(9.06999969f, 0.92f, -8.5f);
    }

}