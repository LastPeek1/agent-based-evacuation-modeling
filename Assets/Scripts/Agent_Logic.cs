using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent_Logic : Agent
{
    //ссылки на скрипты
    private LevelManager LevelManager;
    AgentSettings AgentSettings;

    //получение компонента физики
    private Rigidbody m_AgentRb;

    // Инициализация компонентов
    public override void Initialize()
    {
        LevelManager = GetComponentInParent<LevelManager>();
        AgentSettings = FindObjectOfType<AgentSettings>();
        m_AgentRb = GetComponent<Rigidbody>();

        //Замораживаем все вращения и движение по Y
        m_AgentRb.constraints =
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezePositionY;
        
    }

    // Сбор данных для нейросети
    public override void CollectObservations(VectorSensor sensor)
    {

    }

    //Обработка выходных данных модели
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }

    //Перемещение агента моделью
    public void MoveAgent(ActionSegment<int> act)
    {
        //получение действия от модели
        var action = act[0];

        // Инициализация направлений
        float vertical = 0f;
        float horizontal = 0f;

        switch (action)
        {
            case 1: // Вперед
                vertical = 1f;
                break;
            case 2: // Назад
                vertical = -0.5f;
                break;
            case 3: // Поворот вправо
                horizontal = 1f;
                break;
            case 4: // Поворот влево
                horizontal = -1f;
                break;
            case 5: // Влево (боковое)
                horizontal = -1f;
                vertical = 0.75f; // Комбинированное движение
                break;
            case 6: // Вправо (боковое)
                horizontal = 1f;
                vertical = 0.75f;
                break;
            case 7:
                //попытка откртия двери
                TryInteractWithDoor();
                break;
        }

        if (action != 7)
        {
            // Применение движения
            Vector3 movement = transform.forward * vertical * AgentSettings.agentRunSpeed * Time.fixedDeltaTime;
            m_AgentRb.MovePosition(m_AgentRb.position + movement);

            // Применение поворота
            Quaternion deltaRotation = Quaternion.Euler(0f, horizontal * AgentSettings.agentRotSpeed * Time.fixedDeltaTime, 0f);
            m_AgentRb.MoveRotation(m_AgentRb.rotation * deltaRotation);
        }
    }

    // Попытка взаимодействия с дверью
    private void TryInteractWithDoor()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, AgentSettings.interactionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Door>(out Door door))
            {
                var result = door.Interact();
                ProcessInteractionResult(result, door);
            }
            else
            {
                AddReward(AgentSettings.rewardForUnnecessaryAttemptsOpenDoor);
                if (AgentSettings.showLog) Debug.Log($"Открыл воздух {AgentSettings.rewardForUnnecessaryAttemptsOpenDoor}");
            }
        }
    }

    // Обработка результатов взаимодействия
    private void ProcessInteractionResult(Door.InteractionResult result, Door door)
    {
        switch (result)
        {
            case Door.InteractionResult.Opened:
                AddReward(AgentSettings.rewardForOpenDoor);
                if (AgentSettings.showLog) Debug.Log($"Открыл дверь: {AgentSettings.rewardForOpenDoor}");
                break;

            case Door.InteractionResult.PermanentlyLocked:
                AddReward(AgentSettings.rewardForOpenDoor);
                if (AgentSettings.showLog) Debug.Log($"Перовая попытка открытия двери: {AgentSettings.rewardForOpenDoor}");
                break;

            case Door.InteractionResult.AlreadyOpen:
                if (door.WasInteracted)
                {
                    AddReward(AgentSettings.rewardForReOpenCloseDoor);
                    if (AgentSettings.showLog) Debug.Log($"Повторная попытка открытия закрытой двери: {AgentSettings.rewardForReOpenCloseDoor}");
                }
                else
                {
                    AddReward(AgentSettings.rewardForOpenDoor);
                    if (AgentSettings.showLog) Debug.Log($"Первая попытка открытия закртой двери: {AgentSettings.rewardForOpenDoor}");
                }
                break;
        }
    }

    // Обработка столкновений
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Exit"))
        {
            LevelManager.TouchedExit(this);
        }
    }

    // Обработка столкновений каждый кадр
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            AddReward(AgentSettings.rewardForWall_Сollision);
            if (AgentSettings.showLog) Debug.Log($"Cоприкосновение с стеной: {AgentSettings.rewardForWall_Сollision}");
        }

        if (collision.gameObject.CompareTag("CloseDoor"))
        {
            AddReward(AgentSettings.rewardForCloseDoor_Сollision);
            if (AgentSettings.showLog) Debug.Log($"Cоприкосновение с закрытой дверью: {AgentSettings.rewardForCloseDoor_Сollision}");
        }

        else if (collision.gameObject.CompareTag("agent"))
        {
            AddReward(AgentSettings.rewardForAgent_Сollision);
            if (AgentSettings.showLog) Debug.Log($"Cоприкосновение с агентом: {AgentSettings.rewardForAgent_Сollision}");
        }
    }

    // Ручное управление
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            discreteActionsOut[0] = 7;
        }

    }
}
