using Microsoft.SemanticKernel;
using AiCoreApi.Models.DbModels;
using AiCoreApi.Common;

namespace AiCoreApi.SemanticKernel.Agents
{
    public class HistoryAgent : BaseAgent, IHistoryAgent
    {
        private const string DebugMessageSenderName = "HistoryAgent";

        private static class AgentContentParameters
        {
            public const string Count = "count";
        }
        private readonly RequestAccessor _requestAccessor;
        private readonly ResponseAccessor _responseAccessor;

        public HistoryAgent(
            RequestAccessor requestAccessor,
            ResponseAccessor responseAccessor,
            ExtendedConfig extendedConfig,
            ILogger<HistoryAgent> logger) : base(requestAccessor, extendedConfig, logger)
        {
            _requestAccessor = requestAccessor;
            _responseAccessor = responseAccessor;
        }

        public override async Task<string> DoCall(
            AgentModel agent, 
            Dictionary<string, string> parameters)
        {
            var count = agent.Content[AgentContentParameters.Count].Value;
            _responseAccessor.AddDebugMessage(DebugMessageSenderName, "Get Message History", count);
            var result = _requestAccessor.MessageDialog!.GetHistory(Convert.ToInt32(count));
            _responseAccessor.AddDebugMessage(DebugMessageSenderName, "Get Message History Result", result);
            return result;
        }
    }

    public interface IHistoryAgent
    {
        Task AddAgent(AgentModel agent, Kernel kernel, List<string> pluginsInstructions);
    }
}
