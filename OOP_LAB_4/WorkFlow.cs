using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_LAB_4
{
    public class WorkFlowEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
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
