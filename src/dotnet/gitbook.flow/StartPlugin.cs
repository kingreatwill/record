using gitbook.core;
using gitbook.core.middleware;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gitbook.flow
{
    public class StartPlugin : IStart
    {
        public string Name => "Plugin";

        private readonly PipelineDelagate _next;

        public StartPlugin(PipelineDelagate next)
        {
            _next = next;
        }

        public async Task Invoke(MiddlewareContext context)
        {
            await _next(context);
        }
      

    }
}
