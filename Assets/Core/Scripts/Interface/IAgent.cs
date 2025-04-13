using System;
using UnityEngine;

namespace Core
{
    public interface IAgent
    {
        public event EventHandler<AgentArrivedEventArgs> AgentArrived;
        GameObject AgentGameObject { get; }
    }
}