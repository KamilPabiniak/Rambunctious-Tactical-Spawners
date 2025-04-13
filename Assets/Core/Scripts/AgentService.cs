using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Core
{
    public class AgentService : IAgentService
    {
        public AgentService(IAgentManager manager) => _agentManager = manager;
        private readonly List<IAgent> _agents = new();
        private readonly IAgentManager _agentManager;

        public event EventHandler<AgentArrivedEventArgs> AgentArrived;
        
        public void SpawnAgent()
        {
            IAgent agent = _agentManager.SpawnAgent();
            if (agent != null)
            {
                RegisterAgent(agent);
            }
        }

        public void RemoveRandomAgent()
        {
            if (_agents.Count == 0) return;
            int index = UnityEngine.Random.Range(0, _agents.Count);
            IAgent agent = _agents[index];
            UnregisterAgent(agent);
        }

        public void ClearAgents()
        {
            foreach (var agent in new List<IAgent>(_agents))
            {
                UnregisterAgent(agent);
            }
        }
        
        private void RegisterAgent(IAgent agent)
        {
            if (_agents.Contains(agent)) return;
            _agents.Add(agent);
            agent.AgentArrived += OnAgentArrived;
        }

        private void UnregisterAgent(IAgent agent)
        {
            if (!_agents.Contains(agent)) return;
            _agents.Remove(agent);
            agent.AgentArrived -= OnAgentArrived;
            Object.Destroy(agent.AgentGameObject);
        }

        //For UI u know
        public int GetAgentCount() => _agents.Count;
        //Agent know what it is
        private void OnAgentArrived(object sender, AgentArrivedEventArgs e) => AgentArrived?.Invoke(sender, e);
    }
}