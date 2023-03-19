using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OOP_LAB_4;
using System.Security.Cryptography.X509Certificates;

static void Main(string[] args)
{
    EventManager eventManager = new EventManager(1000);
    // Створення трьох підписників з різними пріоритетами
    Subscriber subscriber1 = new Subscriber("Subscriber 1", 1);
    Subscriber subscriber2 = new Subscriber("Subscriber 2", 2);
    Subscriber subscriber3 = new Subscriber("Subscriber 3", 1);

    // Додавання підписників до списку підписників менеджера подій
    eventManager.AddEventHandler(subscriber1.HandleEvent);
    eventManager.AddEventHandler(subscriber2.HandleEvent);
    eventManager.AddEventHandler(subscriber3.HandleEvent);

    // Надсилання даних через подію
    eventManager.SendData("Data 1", 1);
    eventManager.SendData("Data 2", 2);
    eventManager.SendData("Data 3", 1);

    // Виведення списку підписників за пріоритетом
    var subscribersByPriority = eventManager.GetHandlersByPriority();
    foreach (var priority in subscribersByPriority.Keys)
    {
        Console.WriteLine($"Priority {priority}:");
        foreach (var subscriber in subscribersByPriority[priority])
        {
            Console.WriteLine(subscriber.Method.Name);
        }
    }

    for (int i = 1; i < 10; i++)
    {
        if (i % 2 == 0)
        {
            eventManager.SendData($"Data {i}", 1);// цей код було змінено для другого завдання, тож тепер тут є передача аргументу priority
        }
    }
    //End of Task 1
    //Task 2 було зроблено паралельно зверху
    //Task 3 я не зміг навіть наблизитися до розуміння процесу, тож, на жаль, виконати його не зміг

    WorkflowEngine engine = new WorkflowEngine();
    WorkflowStep step1 = new WorkflowStep();
    WorkflowStep step2 = new WorkflowStep();
    WorkflowStep step3 = new WorkflowStep();

    engine.Run(step1);
    engine.StepCompleted += (sender, e) =>
    {
        if (e.Message.Contains(nameof(step2)))
        {
            step3.Execute();
        }
        else if (e.Message.Contains(nameof(step1)))
        {
            Console.WriteLine("Workflow completed. All of the Jedi has been executed");
        }
        engine.Run(step1, step2, step3);
    };
}
