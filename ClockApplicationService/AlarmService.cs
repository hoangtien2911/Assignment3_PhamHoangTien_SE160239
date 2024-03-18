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
    private readonly object alarmLock = new object();
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
        lock (alarmLock)
        {
            alarmTimes.AddRange(timeList);
            alarmTimes.Sort();
        }
        Start();
    }

    public void AddAlarm(DateTime time)
    {
        lock (alarmLock)
        {
            alarmTimes.Add(time);
            alarmTimes.Sort();
        }
    }

    public void RemoveAlarm(DateTime time)
    {
        lock (alarmLock)
        {
            alarmTimes.Remove(time);
        }
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
        lock (alarmLock)
        {
            alarmTimes.Clear();
        }
    }

    private void AlarmLoop()
    {
        while (isRunning)
        {
            DateTime now = DateTime.Now;
            List<DateTime> triggeredAlarms = new List<DateTime>();

            lock (alarmLock)
            {
                foreach (var time in alarmTimes)
                {
                    if (now.Year == time.Year && now.Month == time.Month && now.Day == time.Day &&
                        now.Hour == time.Hour && now.Minute == time.Minute)
                    {
                        triggeredAlarms.Add(time);
                    }
                }
                foreach (var triggeredAlarm in triggeredAlarms)
                {
                    alarmTimes.Remove(triggeredAlarm);
                }
            }

            foreach (var triggeredAlarm in triggeredAlarms)
            {
                AlarmTriggered?.Invoke(this, EventArgs.Empty);
            }

            Thread.Sleep(1000); // Sleep for one second
        }
    }
}
