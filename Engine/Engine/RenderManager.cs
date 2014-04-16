namespace Dive.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SFML.Graphics;
    using SFML.Window;

    /// <summary>
    /// Handles the rendering of objects.
    /// </summary>
    public class RenderManager
    {
        //// priority queue implemented as a binary heap
        private List<RenderJob> renderHeap = new List<RenderJob>();

        private int smallestDepth = int.MaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderManager"/> class.
        /// </summary>
        public RenderManager()
        {
        }

        /// <summary>
        /// Adds to the render queue.
        /// </summary>
        /// <param name="drawable">The drawable.</param>
        public void AddToRenderQueue(Drawable drawable)
        {
            this.AddToRenderQueue(drawable, this.smallestDepth - 1);
        }

        /// <summary>
        /// Adds to the render queue. Renders are done from the largest depth to the smallest.
        /// </summary>
        /// <param name="drawable">The drawable.</param>
        /// <param name="depth">The depth.</param>
        public void AddToRenderQueue(Drawable drawable, int depth)
        {
            if (depth < this.smallestDepth)
            {
                this.smallestDepth = depth;
            }

            RenderJob job = new RenderJob() { Drawable = drawable, Depth = depth };

            int jobIndex = this.renderHeap.Count;
            int parentIndex = (jobIndex - 1) / 2;
            this.renderHeap.Add(job);

            RenderJob parent = this.renderHeap[parentIndex];

            while (parent.Depth > job.Depth)
            {
                this.renderHeap[parentIndex] = job;
                this.renderHeap[jobIndex] = parent;
                jobIndex = parentIndex;
                parentIndex = (jobIndex - 1) / 2;
                parent = this.renderHeap[parentIndex];
            }
        }

        /// <summary>
        /// Draws and clears the render queue.
        /// </summary>
        public void Draw()
        {
            while (!this.IsRenderQueueEmpty())
            {
                RenderJob job = this.PopRenderJob();
                GameEngine.Instance.Window.Draw(job.Drawable);
            }

            this.smallestDepth = int.MaxValue;
        }

        private bool IsRenderQueueEmpty()
        {
            return this.renderHeap.Count == 0;
        }

        private RenderJob PopRenderJob()
        {
            RenderJob result = this.renderHeap[0];

            RenderJob job = this.renderHeap.Last();
            this.renderHeap.RemoveAt(this.renderHeap.Count - 1);

            if (this.IsRenderQueueEmpty())
            {
                return result;
            }

            this.renderHeap[0] = job;
            int currentIndex = 0;

            while (currentIndex < this.renderHeap.Count)
            {
                int leftIndex = (currentIndex * 2) + 1;
                int rightIndex = (currentIndex * 2) + 2;
                RenderJob left = leftIndex < this.renderHeap.Count ? this.renderHeap[leftIndex] : null;
                RenderJob right = rightIndex < this.renderHeap.Count ? this.renderHeap[rightIndex] : null;

                if (left != null && left.Depth < job.Depth)
                {
                    if (right != null && right.Depth < job.Depth)
                    {
                        if (left.Depth <= right.Depth)
                        {
                            this.renderHeap[leftIndex] = job;
                            this.renderHeap[currentIndex] = left;
                            currentIndex = leftIndex;
                        }
                        else
                        {
                            this.renderHeap[rightIndex] = job;
                            this.renderHeap[currentIndex] = right;
                            currentIndex = rightIndex;
                        }
                    }
                    else
                    {
                        this.renderHeap[leftIndex] = job;
                        this.renderHeap[currentIndex] = left;
                        currentIndex = leftIndex;
                    }
                }
                else if (right != null && right.Depth < job.Depth)
                {
                    this.renderHeap[rightIndex] = job;
                    this.renderHeap[currentIndex] = right;
                    currentIndex = rightIndex;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private class RenderJob
        {
            public int Depth
            {
                get;
                set;
            }

            public Drawable Drawable
            {
                get;
                set;
            }
        }
    }
}
