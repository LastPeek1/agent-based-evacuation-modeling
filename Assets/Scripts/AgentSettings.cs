using UnityEngine;

public class AgentSettings : MonoBehaviour
{
    //��������� ��������
    public readonly float agentRunSpeed = 3f;
    public readonly float agentRotSpeed = 150f;

    //������ ������������� ��������
    public readonly float rewardForOpenDoor = 0f; //�� �������� ����� 
    public readonly float rewardForReOpenCloseDoor = 0f; // �� �� ������ ������� �������� ��������������� ����� 
    public readonly float rewardForUnnecessaryAttemptsOpenDoor = 0f; //�� ������� ������� "������"

    //������ ������������� �� ���������������
    public readonly float rewardForWall_�ollision = -0.0001f;
    public readonly float rewardForAgent_�ollision = -0.0001f;
    public readonly float rewardForCloseDoor_�ollision = -0.0001f;
    public readonly float rewardForSpeedExit_Collision = 1f;

    //����� �������
    public readonly bool showLog = false;

    //������ ������������� � �������
    public readonly float interactionRadius = 0.5f;

    //����� �������� �� ������� ����������� ������
    public readonly float spawnRadius = 0.5f;

    //����� ������� � ������� ����� MaxEnvironmentSteps / 50 
    public readonly int maxEnvironmentSteps = 6000;

}
