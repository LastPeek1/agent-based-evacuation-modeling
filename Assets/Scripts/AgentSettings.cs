using UnityEngine;

public class AgentSettings : MonoBehaviour
{
    //Настройки движения
    public readonly float agentRunSpeed = 3f;
    public readonly float agentRotSpeed = 150f;

    //систма вознагражения действий
    public readonly float rewardForOpenDoor = 0f; //за открытие двери 
    public readonly float rewardForReOpenCloseDoor = 0f; // за НЕ первые попытки открытия заблокированной двери 
    public readonly float rewardForUnnecessaryAttemptsOpenDoor = 0f; //за попытки открыть "воздух"

    //систма вознагражения за соприкосновения
    public readonly float rewardForWall_Сollision = -0.0001f;
    public readonly float rewardForAgent_Сollision = -0.0001f;
    public readonly float rewardForCloseDoor_Сollision = -0.0001f;
    public readonly float rewardForSpeedExit_Collision = 1f;

    //режим отладки
    public readonly bool showLog = false;

    //радиус взаимодествия с дверьми
    public readonly float interactionRadius = 0.5f;

    //радус проверки на наличие перпядствий вокруг
    public readonly float spawnRadius = 0.5f;

    //время эпизода в секндах равно MaxEnvironmentSteps / 50 
    public readonly int maxEnvironmentSteps = 6000;

}
