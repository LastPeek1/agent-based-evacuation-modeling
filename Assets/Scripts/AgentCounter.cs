using UnityEngine;
using UnityEngine.UI;

public class AgentCounter : MonoBehaviour
{
    // ������ �� UI ��������
    [SerializeField] private Text counterText;
    
    // ���������� ��� �������� ������
    private int initialAgents;
    private int currentActiveAgents;

    // �������������
    void Start()
    {
        UpdateCounterText(); // �������������� ����������
    }

    // ���������� ������
    public void UpdateCounterText()
    {
        counterText.text = $"{currentActiveAgents}/{initialAgents}";
    }

    // �������� ��� ����������� ������� � ����������
    public int InitialAgents
    {
        get => initialAgents;
        set
        {
            initialAgents = value;
            UpdateCounterText(); // �������������� ���������� UI ��� ���������
        }
    }

    public int CurrentActiveAgents
    {
        get => currentActiveAgents;
        set
        {
            currentActiveAgents = value;
            UpdateCounterText(); // �������������� ���������� UI ��� ���������
        }
    }
}