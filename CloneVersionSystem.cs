using System.Collections.Generic;

namespace Clones
{
    public class CloneVersionSystem : ICloneVersionSystem
    {
        private readonly List<Clone> clones = new List<Clone> { new Clone() };

        public string Execute(string query)
        {
            var queries = query.Split();
            var id = int.Parse(queries[1]) - 1;
            var clone = clones[id];
            switch (queries[0])
            {
                case "learn":
                    clone.Learn(int.Parse(queries[2]));
                    return null;
                case "rollback":
                    clone.Rollback();
                    return null;
                case "relearn":
                    clone.Relearn();
                    return null;
                case "check":
                    return clone.Check();
                case "clone":
                    clones.Add(clone.GetClone());
                    return null;
                default:
                    return null;
            }
        }
    }

    public class Clone
    {
        private Stack<int> learned;
        private Stack<int> cancelled;

        public Clone()
        {
            learned = new Stack<int>();
            cancelled = new Stack<int>();
        }

        public Clone(Stack<int> learned, Stack<int> cancelled)
        {
            this.learned = learned;
            this.cancelled = cancelled;
        }

        public void Learn(int program)
        {
            if (!learned.Contains(program))
            {
                cancelled.Clear();
                learned.Push(program);
            }
        }

        public void Rollback()
        {
            if (learned.Count != 0)
                cancelled.Push(learned.Pop());
        }

        public void Relearn()
        {
            if (cancelled.Count != 0)
                learned.Push(cancelled.Pop());
        }

        public Clone GetClone() =>
            new Clone(learned.Clone(), cancelled.Clone());

        public string Check() =>
                learned.Count == 0 ? "basic" : learned.Peek().ToString();
    }

    public class Stack<T>
    {
        private StackNode last;
        public int Count { get; private set; }

        public T Peek()
        {
            return last.Value;
        }

        public T Pop()
        {
            var value = last.Value;
            last = last.Previous;
            Count--;
            return value;
        }

        public void Push(T element)
        {
            var next = new StackNode { Value = element };
            Count++;
            if (last != null) next.Previous = last;
            last = next;
        }

        public Stack<T> Clone()
        {
            return new Stack<T> { last = last, Count = Count };
        }

        public void Clear()
        {
            Count = 0;
            last = null;
        }

        public bool Contains(T data)
        {
            var current = last;
            while (current != null)
            {
                if (current.Value.Equals(data))
                    return true;
                current = current.Previous;
            }
            return false;
        }

        private class StackNode
        {
            public StackNode Previous;
            public T Value;
        }
    }
}