using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockApplicationService;

public class AlarmService
{
    private DateTime alarmTime;
    private bool isRunning;
    private Thread alarmThread;

    public event EventHandler AlarmTriggered;

    public AlarmService()
    {
        Reset();
    }

    public void SetAlarm(DateTime time)
    {
        alarmTime = time;
        Start();
    }

    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            alarmThread = new Thread(Alarm);
            alarmThread.Start();
        }
    }

    public void Stop()
    {
        isRunning = false;
        alarmThread?.Join();
    }

    public void Reset()
    {
        alarmTime = DateTime.MinValue;
    }

    private void Alarm()
    {
        while (isRunning)
        {
            if (DateTime.Now >= alarmTime)
            {
                AlarmTriggered?.Invoke(this, EventArgs.Empty);
                break;
            }

            Thread.Sleep(1000); // Sleep for one second
        }
    }
}
