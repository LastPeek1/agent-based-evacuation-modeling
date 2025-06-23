using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] protected GameObject obstaclePrefab;
    [SerializeField] protected GameObject exitPrefab;

    [Header("����")]
    [SerializeField] private LayerMask wallLayer;

    [Header("UI")]
    [SerializeField] private AgentCounter agentCounter;
    [SerializeField] private Timer timer;

    //��������� ������(���������� � ����������� �������)
    protected Vector3 spawnAgentAreaMin;
    protected Vector3 spawnAgentAreaMax;

    //�������� �������� ������ ������������ �����
    private List<AgentInfo> AgentsList = new List<AgentInfo>();
    private List<Door> DoorsList = new List<Door>();

    //��������� ������
    private int numberOfRemainingAgents;
    private int resetTimer;
    private int countAllAgent;


    //������ ������� MLAgent
    private SimpleMultiAgentGroup AgentGroup;

    //���� �������� ������� 
    AgentSettings agentSettings;

    //��������� �������� ������ � ������ 
    public class AgentInfo
    {
        public Agent_Logic Agent;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
        [HideInInspector]
        public Rigidbody Rb;
        [HideInInspector]
        public Collider Col;
    }

    void Start()
    {
        FindAndRegisterAgents();

        //��������� �������� ������
        agentSettings = FindObjectOfType<AgentSettings>();

        numberOfRemainingAgents = AgentsList.Count;

        AgentGroup = new SimpleMultiAgentGroup();

        foreach (var item in AgentsList)
        {
            InitializeAgentInfo(item);
            AgentGroup.RegisterAgent(item.Agent);
        }
        ResetScene();
    }

    void FixedUpdate()
    {
        //����� �������
        resetTimer += 1;
        if (resetTimer >= agentSettings.maxEnvironmentSteps && agentSettings.maxEnvironmentSteps > 0)
        {
            // �������� ������������ ������ ������
            Transform levelParent = transform.parent;

            Agent_Logic[] foundAgents = levelParent.GetComponentsInChildren<Agent_Logic>(false);

            foreach (var agent in foundAgents)
            {
                agent.SetReward(-0.5f);
                Debug.Log($"������ ������� ������: {agent.GetCumulativeReward()}");
            }
            AgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    //����������� ���� ������� �� �����
    private void FindAndRegisterAgents()
    {
        AgentsList.Clear();

        // �������� ������������ ������ ������
        Transform levelParent = transform.parent;

        // ���� ���� ������� ������ ������������� ������� (������� ����������)
        Agent_Logic[] foundAgents = levelParent.GetComponentsInChildren<Agent_Logic>(true);

        foreach (var agent in foundAgents)
        {
            // ��������� ��������� ���������� ������������ ��������
            AgentInfo newInfo = new AgentInfo
            {
                Agent = agent,
                StartingPos = levelParent.InverseTransformPoint(agent.transform.position),
                StartingRot = Quaternion.Inverse(levelParent.rotation) * agent.transform.rotation,
                Rb = agent.GetComponent<Rigidbody>(),
                Col = agent.GetComponent<Collider>()
            };

            AgentsList.Add(newInfo);
        }
    }

    //��������� ����������� ������
    private void InitializeAgentInfo(AgentInfo item)
    {
        if (item.Rb == null) item.Rb = item.Agent.GetComponent<Rigidbody>();
        if (item.Col == null) item.Col = item.Agent.GetComponent<Collider>();
    }

    //������� �����
    public virtual void ResetScene()
    {
        resetTimer = 0;

        Transform levelParent = transform.parent;

        numberOfRemainingAgents = AgentsList.Count;

        foreach (var item in AgentsList)
        {
            // ������������ ��������� ���������� ������� � �������
            Vector3 worldPos = levelParent.TransformPoint(GetRandomSpawnPos());
            Quaternion worldRot = levelParent.rotation * GetRandomRot();

            item.Agent.transform.SetPositionAndRotation(worldPos, worldRot);
            item.Rb.velocity = Vector3.zero;
            item.Rb.angularVelocity = Vector3.zero;
            item.Agent.gameObject.SetActive(true);
            AgentGroup.RegisterAgent(item.Agent);
        }

        ResetAllDoors();

        agentCounter.InitialAgents = AgentsList.Count; // ��������� ����������
        agentCounter.CurrentActiveAgents = AgentsList.Count; // ������� ����������

        timer.StartTimer();

    }

    //��������� ������ ��� ������������ � �������
    public void TouchedExit(Agent_Logic agent)
    {
        agentCounter.CurrentActiveAgents--;
        numberOfRemainingAgents--;

        float timeBonus = CalculateReward(resetTimer);

        if (numberOfRemainingAgents == 0)
        {
            agent.AddReward(timeBonus);
            agent.gameObject.SetActive(false);
            AgentGroup.EndGroupEpisode();
            
            ResetScene();
        }
        else
        {
            agent.AddReward(timeBonus);
            Debug.Log($"������ ������� ������: {agent.GetCumulativeReward()}");
            agent.gameObject.SetActive(false);
        }
    }

    //���������� ������� �� ������ ���������� ����� ��� ���������� �������
    private float CalculateReward(int steps)
    {
        float initialReward = agentSettings.rewardForSpeedExit_Collision;
        // �������� ��������: 1 / 6000, ����� ������� �������� 0 �� 6000 ����
        float linearDecayRate = 1f / agentSettings.maxEnvironmentSteps;

        // �������� ���������� �������
        float reward = initialReward - linearDecayRate * steps;

        // ��������, ��� ������� �� ������ �������������
        reward = Mathf.Max(reward, 0f);

        return reward;
    }

    //����� ��������� �����
    public void ResetAllDoors()
    {
        Transform levelParent = transform.parent;
        if (levelParent != null)
        {
            // ���� ����� ������ ������ ����� Level
            DoorsList = new List<Door>(levelParent.GetComponentsInChildren<Door>(true));
        }

        if (DoorsList == null) return;

        foreach (Door door in DoorsList)
        {
            if (door != null)
            {
                door.ResetState();
            }
        }
    }

    //��������� ��������� ������� ������ � ������ ���������� 
    public Vector3 GetRandomSpawnPos()
    {
        Transform levelParent = transform.parent;

        bool positionFound;
        Vector3 newPosition;

        Vector3 localMin = levelParent.InverseTransformPoint(spawnAgentAreaMin);
        Vector3 localMax = levelParent.InverseTransformPoint(spawnAgentAreaMax);

        do
        {
            Vector3 localPosition = new Vector3(
                Random.Range(localMin.x, localMax.x),
                localMin.y,
                Random.Range(localMin.z, localMax.z)
            );

            // ������������ ��������� ������� � ������� ����������
            newPosition = levelParent.TransformPoint(localPosition);

            positionFound = !Physics.CheckSphere(newPosition, agentSettings.spawnRadius, wallLayer);
        } while (!positionFound);

        return newPosition;
    }

    //��������� ���������� ��������
    Quaternion GetRandomRot()
    {
        return Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
    }
}