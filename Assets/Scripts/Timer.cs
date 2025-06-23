using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timerText;
    private Coroutine timerCoroutine;
    private float currentTime;

    public void StartTimer()
    {
        // Останавливаем предыдущий отсчет
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        // Сбрасываем время
        currentTime = 0f;
        UpdateTimerDisplay();

        // Запускаем новый отсчет
        timerCoroutine = StartCoroutine(RunTimer());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator RunTimer()
    {
        while (true)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }
    }

    // Формат XX:YY:Z
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        // Получаем десятые доли секунды (одна цифра)
        int tenths = Mathf.FloorToInt((currentTime * 10) % 10);

        timerText.text = $"{minutes:0}:{seconds:00}:{tenths}";
    }

    public float GetCurrentTime() => currentTime;
}