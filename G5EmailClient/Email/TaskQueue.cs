using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace G5EmailClient.Email
{
    internal class TaskQueue
    {

        List<Task> taskList = new();

        TaskFactory taskFactory = new();

        public void Add(Func<(object, object)> method)
        {
            Task task = taskFactory.StartNew(method);
            taskList.Add(task);
        }

        public void Add(Func<(object, object, object)> method)
        {
            Task task = taskFactory.StartNew(method);
            taskList.Add(task);
        }
    }
}
