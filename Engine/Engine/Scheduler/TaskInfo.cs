namespace Dive.Engine.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Holds information about a task. Do not queue the same TaskInfo object multiple times, or else you will have MAJOR problems.
    /// </summary>
    public class TaskInfo
    {
        /// <summary>
        /// Gets or sets the time until this task is executed.
        /// </summary>
        /// <value>
        /// The time until this task is executed.
        /// </value>
        public float ExecuteAfter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [repeating].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [repeating]; otherwise, <c>false</c>.
        /// </value>
        public bool Repeating
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the task delegate.
        /// </summary>
        /// <value>
        /// The task delegate.
        /// </value>
        public DiveScheduler.Task Task
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this task has been completed.
        /// <para>
        /// Setting this to true on a repeating task will stop the task.
        /// </para>
        /// <para>
        /// It is preferred to set this to true than to use RemoveTask, as RemoveTask involves a list search and this does not.
        /// </para>
        /// </summary>
        /// <value>
        ///   <c>true</c> if [completed]; otherwise, <c>false</c>.
        /// </value>
        public bool Completed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time left until execution. This will be overridden by the scheduler
        /// when you first schedule this task.
        /// </summary>
        /// <value>
        /// The time left until execution.
        /// </value>
        public float TimeLeft
        {
            get;
            set;
        }
    }
}
