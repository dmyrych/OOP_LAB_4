using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OOP_LAB_4;

static void Main(string[] args)
{
    EventManager eventManager = new EventManager(1000);
    void DataEventHandler1(object sender, DataEventArgs args)
    {
        Console.WriteLine($"Event 1: {args}");
    }
    void DataEventHandler2(object sender, DataEventArgs args)
    {
        Console.WriteLine($"Event 2: {args}");
    }
    eventManager.AddEventHandler(DataEventHandler1);
    eventManager.AddEventHandler(DataEventHandler2);
    eventManager.DataEvent += DataEventHandler1;
    eventManager.DataEvent += DataEventHandler2;
    for (int i = 1; i < 10; i++)
    {
        if (i % 2 == 0)
        {
            eventManager.SendData($"Data {i}");
        }
    }
    //End of Task 1

}
