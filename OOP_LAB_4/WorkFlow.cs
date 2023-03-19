using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LAB_4
{
    // Клас WorkFlowEventArgs наслідується від EventArgs і містить одне властивість Message.
    public class WorkFlowEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    // Клас WorkflowStep містить подію Completed, яка сповіщає, що крок виконаний.
    // Він містить метод OnCompleted(), який сприяє виклику події Completed.
    // Він містить метод Execute(), який виконує крок і сприяє виклику події Completed.
    public class WorkflowStep
    {
        public event EventHandler<WorkFlowEventArgs> Completed;

        public void OnCompleted(WorkFlowEventArgs e)
        {
            Completed?.Invoke(this, e);
        }

        public void Execute()
        {
            Console.WriteLine("Execute order 66...");
            Console.WriteLine($"Executing step {GetType().Name}...");
            OnCompleted(new WorkFlowEventArgs { Message = $"Step {GetType().Name} completed." });
        }
    }

    // Клас WorkflowEngine містить подію StepCompleted, яка сповіщає про завершення кроку.
    // Він містить метод Run(), який приймає кілька параметрів WorkflowStep, додає обробник події Completed
    // до кожного кроку і викликає метод Execute() першого кроку.
    // Він містить метод OnStepCompleted(), який сприяє виклику події StepCompleted.
    public class WorkflowEngine
    {
        public event EventHandler<WorkFlowEventArgs> StepCompleted;

        public void Run(params WorkflowStep[] steps)
        {
            foreach (var step in steps)
            {
                step.Completed += (sender, e) =>
                {
                    Console.WriteLine(e.Message);
                    OnStepCompleted(e);
                };
            }

            Console.WriteLine("Starting workflow...");
            steps[0].Execute();
        }

        public void OnStepCompleted(WorkFlowEventArgs e)
        {
            StepCompleted?.Invoke(this, e);
        }
    }
}
