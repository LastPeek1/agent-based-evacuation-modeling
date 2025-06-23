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
        // ������������� ���������� ������
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        // ���������� �����
        currentTime = 0f;
        UpdateTimerDisplay();

        // ��������� ����� ������
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

    // ������ XX:YY:Z
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        // �������� ������� ���� ������� (���� �����)
        int tenths = Mathf.FloorToInt((currentTime * 10) % 10);

        timerText.text = $"{minutes:0}:{seconds:00}:{tenths}";
    }

    public float GetCurrentTime() => currentTime;
}