using UnityEngine;
public class Level_4_Manager : LevelManager
{
    private void Awake()
    {
        // ���������� ��������� ��� ������ 4
        spawnAgentAreaMin = new Vector3(-1.46000004f, 0.92f, 2.23000002f);
        spawnAgentAreaMax = new Vector3(9.06999969f, 0.92f, -8.5f);
    }

    public override void ResetScene()
    {
        base.ResetScene();
        SpawnNewExit();
    }

    public void SpawnNewExit()
    {
        Transform levelParent = transform.parent;

        // ������� ������ Exit
        GameObject[] existingExits = GameObject.FindGameObjectsWithTag("Exit");
        foreach (GameObject exit in existingExits)
        {
            if (exit.transform.IsChildOf(levelParent)) Destroy(exit);
        }

        // ���������� ����� �������
        Vector3 localPosition = new Vector3(
            Random.Range(spawnAgentAreaMin.x, spawnAgentAreaMax.x),
            1f,
            Random.Range(spawnAgentAreaMin.z, spawnAgentAreaMax.z)
        );

        // ������������ ��������� ���������� � �������
        Vector3 worldPosition = levelParent.TransformPoint(localPosition);

        // ������� ����� Exit
        GameObject newExit = Instantiate(
            exitPrefab,
            worldPosition,
            Quaternion.identity,
            levelParent // ������ �������� �������� ������
        );

        newExit.tag = "Exit"; // ��������� ��� ������ �� ����� ���� �� ���������
    }

}