using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public GameObject timeDisplay;
    public GameObject dayDisplay;
    public float startingTime = 240;
    public bool timeFlowing;
    public int day { get; private set; } = 1;

    List<Plantable> plants = new List<Plantable>();
    const int secondsPerDay = 1440;
    float time;
    int lastTick;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeFlowing = true;
        time = startingTime;
    }

    private void Update()
    {
        if (timeFlowing)
        {
            time += Time.deltaTime;

            if ((int)time > lastTick)
            {
                lastTick = (int)time;
                
                UpdateTime();
            }
        }
    }

    void UpdateTime()
    {
        int hours = (int)time / 60;
        int minutes = (int)time % 60;
        string meridiem = "AM";

        if (hours > 12)
        {
            hours -= 12;
            meridiem = "PM";
        }

        timeDisplay.GetComponent<Text>().text = hours + ":" + minutes.ToString("00") + " " + meridiem;
    }

    public void EndDay()
    {
        StartCoroutine(NextDay());
    }

    IEnumerator NextDay()
    {
        GameStateController.instance.SetState("Sleeping");
        UiController.instance.FadeToBlack(3f);
        yield return new WaitForSeconds(1);
        day++;
        DailyChanges();
        time = startingTime;
        lastTick = (int)time;
        UpdateTime();
        dayDisplay.GetComponent<Text>().text = "Day " + day;
        yield return new WaitForSeconds(1);
        UiController.instance.FadeIn(2f);
        GameStateController.instance.SetState("Live");
    }

    void DailyChanges()
    {
        foreach (Plantable plant in plants)
        {
            plant.Grow();
        }
    }

    public void AddPlant(Plantable newPlant)
    {
        plants.Add(newPlant);
    }

    public void RemovePlant(Plantable newPlant)
    {
        plants.Remove(newPlant);
    }
}
