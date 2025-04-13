using System;

namespace Core
{
    public interface IAgentService
    {
        void SpawnAgent();
        void RemoveRandomAgent();
        void ClearAgents();
        int GetAgentCount();
        event EventHandler<AgentArrivedEventArgs> AgentArrived;
    }
}