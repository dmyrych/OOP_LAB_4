using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OOP_LAB_4
{
    // Клас, що містить дані, які передаються в подію
    public class DataEventArgs : EventArgs
    {
        public string Data { get; set; }

        public DataEventArgs(string data)
        {
            Data = data;
        }
    }
    // Клас підписника з пріоритетом
    public class PrioritySubscriber
    {
        public int Priority { get; set; }
        public EventManager.EventHandler Handler { get; set; }

        public PrioritySubscriber(int priority, EventManager.EventHandler handler)
        {
            Priority = priority;
            Handler = handler;
        }
    }

    // Клас, що реалізує користувацьку шину подій з обмеженням швидкості надсилання подій
    public class EventManager
    {
        private List<PrioritySubscriber> _subscribers = new List<PrioritySubscriber>();
        public List<EventHandler> GetHandlersByPriority(int priority)
        {
            List<EventHandler> handlers = new List<EventHandler>();
            foreach (EventHandler handler in DataEvent.GetInvocationList())
            {
                if (handler.Target is Subscriber subscriber && subscriber.Priority == priority)
                {
                    handlers.Add(handler);
                }
            }
            return handlers;
        }
        // Делегат для обробки подій
        public delegate void EventHandler(object sender, DataEventArgs args);

        // Подія для надсилання даних
        public event EventHandler DataEvent;

        // Обмеження швидкості надсилання подій (в мс)
        private readonly int _eventInterval;

        // Час останньої надісланої події
        private DateTime _lastEventTime;

        // Час очікування між подіями
        private int EventDelay => _eventInterval - (DateTime.Now - _lastEventTime).Milliseconds;

        public EventManager(int eventInterval)
        {
            _eventInterval = eventInterval;
            _lastEventTime = DateTime.Now;
        }

        // Метод для надсилання даних через подію
        public void SendData(string data, int priority)
        {
            // Перевірка часу очікування між подіями
            if (EventDelay > 0)
            {
                Thread.Sleep(EventDelay);
            }

            // Відправлення подій з даними
            List<EventHandler> handlers = GetHandlersByPriority(priority);
            foreach (EventHandler handler in handlers)
            {
                handler?.Invoke(this, new DataEventArgs(data));
            }

            // Оновлення часу останньої надісланої події
            _lastEventTime = DateTime.Now;
        }
        public void AddEventHandler(EventHandler handler)
        {
            DataEvent += handler;
        }
        public void RemoveEventHandler(EventHandler handler)
        {
            DataEvent -= handler;
        }
        // Знизу написані методи для другого завдання
        public Dictionary<int, List<EventHandler>> GetHandlersByPriority()
        {
            var subscribersByPriority = new Dictionary<int, List<EventHandler>>();

            // Пройти по всіх підписниках і додати їх до відповідного списку за пріорітетом
            foreach (var subscriber in _subscribers)
            {
                // Перевірка існування підписників для даного пріорітета
                if (!subscribersByPriority.ContainsKey(subscriber.Priority))
                {
                    // Якщо список не існує - створити
                    subscribersByPriority[subscriber.Priority] = new List<EventHandler>();
                }

                // Додавання підписника до списку за пріорітетом
                subscribersByPriority[subscriber.Priority].Add(subscriber.Handler);
            }

            // Відсортувати списки підписників за пріорітетом
            foreach (var priority in subscribersByPriority.Keys)
            {
                subscribersByPriority[priority].Sort((x, y) => -x.Method.Name.CompareTo(y.Method.Name));
            }

            return subscribersByPriority;
        }
    }
    public class Subscriber
    {
        // Назва підписника
        public string Name { get; set; }

        // Пріоритет підписника
        public int Priority { get; set; }

        // Конструктор з назвою та пріоритетом підписника
        public Subscriber(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }

        // Обробник подій
        public void HandleEvent(object sender, DataEventArgs args)
        {
            Console.WriteLine($"{Name} ({Priority}) received data: {args.Data}");
        }
    }
}
