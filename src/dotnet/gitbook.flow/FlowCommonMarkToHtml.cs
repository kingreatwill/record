using gitbook.core;
using gitbook.core.middleware;
using gitbook.flow.markdown;
using System;
using System.Threading.Tasks;

namespace gitbook.flow
{
    public class FlowCommonMarkToHtml : IFlow
    {
        public string Name => "CommonMarkToHtml";

        private readonly PipelineDelagate _next;

        public FlowCommonMarkToHtml(PipelineDelagate next)
        {
            _next = next;
        }

        public async Task Invoke(MiddlewareContext context)
        {
            context.ItemResponse = MarkdownRenderers.Markdown2WebHTML(context.ItemResponse, null);
            await _next(context);
        }
    }
}
