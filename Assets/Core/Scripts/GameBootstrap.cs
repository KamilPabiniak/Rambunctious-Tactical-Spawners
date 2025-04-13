using UnityEngine;

namespace Core
{
    public class GameBootstrap : MonoBehaviour
    {
        public MonoBehaviour agentManagerReference;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (agentManagerReference is not IAgentManager agentManager) return;
            if (!ServiceLocator.HasService<IAgentService>())
            {
                IAgentService agentService = new AgentService(agentManager);
                ServiceLocator.RegisterService(agentService);
            }

            if (!ServiceLocator.HasService<ITickService>())
            {
                ITickService tickService = new TickService();
                ServiceLocator.RegisterService(tickService);
            }
        }
    }
}