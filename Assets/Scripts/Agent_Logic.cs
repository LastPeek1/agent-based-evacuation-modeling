using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent_Logic : Agent
{
    //������ �� �������
    private LevelManager LevelManager;
    AgentSettings AgentSettings;

    //��������� ���������� ������
    private Rigidbody m_AgentRb;

    // ������������� �����������
    public override void Initialize()
    {
        LevelManager = GetComponentInParent<LevelManager>();
        AgentSettings = FindObjectOfType<AgentSettings>();
        m_AgentRb = GetComponent<Rigidbody>();

        //������������ ��� �������� � �������� �� Y
        m_AgentRb.constraints =
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ |
                         RigidbodyConstraints.FreezePositionY;
        
    }

    // ���� ������ ��� ���������
    public override void CollectObservations(VectorSensor sensor)
    {

    }

    //��������� �������� ������ ������
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }

    //����������� ������ �������
    public void MoveAgent(ActionSegment<int> act)
    {
        //��������� �������� �� ������
        var action = act[0];

        // ������������� �����������
        float vertical = 0f;
        float horizontal = 0f;

        switch (action)
        {
            case 1: // ������
                vertical = 1f;
                break;
            case 2: // �����
                vertical = -0.5f;
                break;
            case 3: // ������� ������
                horizontal = 1f;
                break;
            case 4: // ������� �����
                horizontal = -1f;
                break;
            case 5: // ����� (�������)
                horizontal = -1f;
                vertical = 0.75f; // ��������������� ��������
                break;
            case 6: // ������ (�������)
                horizontal = 1f;
                vertical = 0.75f;
                break;
            case 7:
                //������� ������� �����
                TryInteractWithDoor();
                break;
        }

        if (action != 7)
        {
            // ���������� ��������
            Vector3 movement = transform.forward * vertical * AgentSettings.agentRunSpeed * Time.fixedDeltaTime;
            m_AgentRb.MovePosition(m_AgentRb.position + movement);

            // ���������� ��������
            Quaternion deltaRotation = Quaternion.Euler(0f, horizontal * AgentSettings.agentRotSpeed * Time.fixedDeltaTime, 0f);
            m_AgentRb.MoveRotation(m_AgentRb.rotation * deltaRotation);
        }
    }

    // ������� �������������� � ������
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
                if (AgentSettings.showLog) Debug.Log($"������ ������ {AgentSettings.rewardForUnnecessaryAttemptsOpenDoor}");
            }
        }
    }

    // ��������� ����������� ��������������
    private void ProcessInteractionResult(Door.InteractionResult result, Door door)
    {
        switch (result)
        {
            case Door.InteractionResult.Opened:
                AddReward(AgentSettings.rewardForOpenDoor);
                if (AgentSettings.showLog) Debug.Log($"������ �����: {AgentSettings.rewardForOpenDoor}");
                break;

            case Door.InteractionResult.PermanentlyLocked:
                AddReward(AgentSettings.rewardForOpenDoor);
                if (AgentSettings.showLog) Debug.Log($"������� ������� �������� �����: {AgentSettings.rewardForOpenDoor}");
                break;

            case Door.InteractionResult.AlreadyOpen:
                if (door.WasInteracted)
                {
                    AddReward(AgentSettings.rewardForReOpenCloseDoor);
                    if (AgentSettings.showLog) Debug.Log($"��������� ������� �������� �������� �����: {AgentSettings.rewardForReOpenCloseDoor}");
                }
                else
                {
                    AddReward(AgentSettings.rewardForOpenDoor);
                    if (AgentSettings.showLog) Debug.Log($"������ ������� �������� ������� �����: {AgentSettings.rewardForOpenDoor}");
                }
                break;
        }
    }

    // ��������� ������������
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Exit"))
        {
            LevelManager.TouchedExit(this);
        }
    }

    // ��������� ������������ ������ ����
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            AddReward(AgentSettings.rewardForWall_�ollision);
            if (AgentSettings.showLog) Debug.Log($"C�������������� � ������: {AgentSettings.rewardForWall_�ollision}");
        }

        if (collision.gameObject.CompareTag("CloseDoor"))
        {
            AddReward(AgentSettings.rewardForCloseDoor_�ollision);
            if (AgentSettings.showLog) Debug.Log($"C�������������� � �������� ������: {AgentSettings.rewardForCloseDoor_�ollision}");
        }

        else if (collision.gameObject.CompareTag("agent"))
        {
            AddReward(AgentSettings.rewardForAgent_�ollision);
            if (AgentSettings.showLog) Debug.Log($"C�������������� � �������: {AgentSettings.rewardForAgent_�ollision}");
        }
    }

    // ������ ����������
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
