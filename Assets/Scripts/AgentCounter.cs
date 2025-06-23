using UnityEngine;
using UnityEngine.UI;

public class AgentCounter : MonoBehaviour
{
    // Ссылки на UI элементы
    [SerializeField] private Text counterText;
    
    // Переменные для хранения данных
    private int initialAgents;
    private int currentActiveAgents;

    // Инициализация
    void Start()
    {
        UpdateCounterText(); // Первоначальное обновление
    }

    // Обновление текста
    public void UpdateCounterText()
    {
        counterText.text = $"{currentActiveAgents}/{initialAgents}";
    }

    // Свойства для безопасного доступа к переменным
    public int InitialAgents
    {
        get => initialAgents;
        set
        {
            initialAgents = value;
            UpdateCounterText(); // Автоматическое обновление UI при изменении
        }
    }

    public int CurrentActiveAgents
    {
        get => currentActiveAgents;
        set
        {
            currentActiveAgents = value;
            UpdateCounterText(); // Автоматическое обновление UI при изменении
        }
    }
}