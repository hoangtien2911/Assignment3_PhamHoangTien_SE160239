using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockApplicationService;

public class AlarmService
{
    private List<DateTime> alarmTimes;
    private Thread alarmThread;
    private bool isRunning;

    public event EventHandler AlarmTriggered;

    public AlarmService()
    {
        alarmTimes = new List<DateTime>();
        isRunning = false;        
    }

    public void ResetAlarmTrigger()
    {
        AlarmTriggered = null;
        alarmTimes = new List<DateTime>();
        alarmThread = null;
        isRunning = false;
    }

    public void SetAlarm(List<DateTime> timeList)
    {
        alarmTimes.AddRange(timeList);
        alarmTimes.Sort();
        Start();
    }

    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            alarmThread = new Thread(AlarmLoop);
            alarmThread.Start();
        }
    }

    public void Stop()
    {
        isRunning = false;
        if (alarmThread != null && alarmThread.IsAlive)
        {
            alarmThread.Join();
        }
    }

    public void Reset()
    {
        alarmTimes.Clear();
    }

    private void AlarmLoop()
    {
        while (isRunning)
        {
            DateTime now = DateTime.Now;
            foreach (var time in alarmTimes)
            {
                if (now.Year == time.Year && now.Month == time.Month && now.Day == time.Day && now.Hour == time.Hour && now.Minute == time.Minute)
                {
                    AlarmTriggered?.Invoke(this, EventArgs.Empty);
                    alarmTimes.Remove(time);
                    break;
                }
            }
            Thread.Sleep(1000); // Sleep for one second
        }
    }
}
