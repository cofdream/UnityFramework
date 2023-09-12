using System;
using System.Collections.Generic;

namespace Cofdream.BaseFramework.Event
{
    public delegate void EventHandler(short type);
    public delegate void EventHandler<T>(short type, T msg);
    public class EventDispatcher : IDispatcher
    {
        private readonly Dictionary<short, List<Delegate>> events = null;
        private readonly Dictionary<short, List<Delegate>> eventsArg = null;
        private readonly List<List<Delegate>> removeHandles = null;
        public EventDispatcher(int count = 10, int argCount = 10)
        {
            events = new Dictionary<short, List<Delegate>>(count);
            eventsArg = new Dictionary<short, List<Delegate>>(argCount);
            removeHandles = new List<List<Delegate>>(count);
        }

        public void ClearAllHandle()
        {
            foreach (var delegates in events.Values)
            {
                delegates.Clear();
            }

            foreach (var delegates in eventsArg.Values)
            {
                delegates.Clear();
            }
            removeHandles.Clear();
        }
        public void ClearNullHandle()
        {
            int length = removeHandles.Count;
            for (int i = 0; i < length; i++)
            {
                var delegates = removeHandles[i];
                int oldLength = delegates.Count;
                int newLength = 0;
                for (int j = 0; j < oldLength; j++)
                {
                    if (delegates[j] != null)
                    {
                        if (newLength != j)
                        {
                            delegates[newLength] = delegates[j];
                        }
                        newLength++;
                    }
                }
                delegates.RemoveRange(newLength, oldLength - newLength);
            }
        }
        public void Subscribe(short type, EventHandler handler)
        {
            Delegate @delegate = handler;
            AddHandle(events, type, @delegate);
        }
        public void Unsubscribe(short type, EventHandler handler)
        {
            Delegate @delegate = handler;
            RemoveHandle(events, type, @delegate);
        }
        public void Subscribe<T>(short type, EventHandler<T> handler)
        {
            Delegate @delegate = handler;
            AddHandle(eventsArg, type, @delegate);
        }
        public void Unsubscribe<T>(short type, EventHandler<T> handler)
        {
            Delegate @delegate = handler;
            RemoveHandle(eventsArg, type, @delegate);
        }
        public void SendEvent(short type)
        {
            Send(events, type);
        }
        public void SendEvent<T>(short type, T msg)
        {
            Send(eventsArg, type, msg);
        }

        private static void AddHandle(Dictionary<short, List<Delegate>> dic, short type, Delegate handle)
        {
            if (dic.TryGetValue(type, out List<Delegate> delegates))
            {
                delegates.Add(handle);
            }
            else
            {
                dic.Add(type, new List<Delegate>() { handle });
            }
        }
        private void RemoveHandle(Dictionary<short, List<Delegate>> dic, short type, Delegate handle)
        {
            if (dic.TryGetValue(type, out List<Delegate> delegates))
            {
                if (delegates != null)
                {
                    int index = delegates.IndexOf(handle);
                    if (index != -1)
                    {
                        delegates[index] = null;
                    }

                    if (removeHandles.Contains(delegates) == false)
                    {
                        removeHandles.Add(delegates);
                    }
                }
            }
        }

        private void Send(Dictionary<short, List<Delegate>> dic, short type)
        {
             if (dic.TryGetValue(type, out List<Delegate> delegates))
            {
                int length = delegates.Count;
                for (int i = 0; i < length; i++)
                {
                    EventHandler handler = delegates[i] as EventHandler;
                    if (handler != null)
                    {
                        handler.Invoke(type);
                    }
                }
            }
        }
        private void Send<T>(Dictionary<short, List<Delegate>> dic, short type, T msg)
        {
            if (dic.TryGetValue(type, out List<Delegate> delegates))
            {
                int length = delegates.Count;
                for (int i = 0; i < length; i++)
                {
                    EventHandler<T> handler = delegates[i] as EventHandler<T>;
                    if (handler != null)
                    {
                        handler.Invoke(type, msg);
                    }
                }
            }
        }
    }
}