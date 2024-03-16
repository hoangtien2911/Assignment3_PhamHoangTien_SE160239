using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockApplicationService;

public class CountdownService
{
    private int initialHours;
    private int initialMinutes;
    private int initialSeconds;
    private int hours;
    private int minutes;
    private int seconds;
    private bool isRunning;
    private Thread countdownThread;

    public event EventHandler<string> TimeChanged;
    public event EventHandler CountdownFinished;

    public CountdownService(int hours, int minutes, int seconds)
    {
        this.initialHours = hours;
        this.initialMinutes = minutes;
        this.initialSeconds = seconds;
        Reset();
    }

    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            countdownThread = new Thread(Countdown);
            countdownThread.Start();
        }
    }

    public void Stop()
    {
        isRunning = false;
        countdownThread?.Join();
    }

    public void Reset()
    {
        hours = initialHours;
        minutes = initialMinutes;
        seconds = initialSeconds;
        TimeChanged?.Invoke(this, $"{hours:00}:{minutes:00}:{seconds:00}");
    }

    private void Countdown()
    {
        while (isRunning)
        {
            if (seconds == 0 && minutes == 0 && hours == 0)
            {
                CountdownFinished?.Invoke(this, EventArgs.Empty);
                break;
            }

            if (seconds == 0)
            {
                if (minutes == 0)
                {
                    if (hours != 0)
                    {
                        hours--;
                        minutes = 59;
                        seconds = 59;
                    }
                }
                else
                {
                    minutes--;
                    seconds = 59;
                }
            }
            else
            {
                seconds--;
            }

            TimeChanged?.Invoke(this, $"{hours:00}:{minutes:00}:{seconds:00}");

            Thread.Sleep(1000); // Sleep for one second
        }
    }
}
