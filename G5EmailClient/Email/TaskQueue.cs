using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MailKit;

namespace G5EmailClient.Email
{
    internal class ToggleReadQueue
    {
        Queue<TPAction<int, int, int>> taskQueue = new();

        public ToggleReadQueue()
        {
            TaskComplete += RunNext;
        }

        public delegate void TPAction<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);

        public void Add<T1, T2, T3>(TPAction<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {

        }

        public void RunNext(object? sender, EventArgs? e)
        {
            if(taskQueue.Count > 0)
                ThreadPool.QueueUserWorkItem(state => Run(taskQueue.Dequeue()));
        }

        private void Run(Action method)
        {
            var test = method;
            method();

            this.TaskComplete(null, EventArgs.Empty);
        }
        event EventHandler TaskComplete;
    }
}
