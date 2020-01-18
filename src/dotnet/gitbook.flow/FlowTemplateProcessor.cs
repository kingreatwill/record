using gitbook.core;
using gitbook.core.middleware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace gitbook.flow
{

    public class FlowTemplateProcessor : IFlow
    {
        public string Name => "TemplateProcessor";

        private readonly PipelineDelagate _next;

        public FlowTemplateProcessor(PipelineDelagate next)
        {
            _next = next;
        }

        public async Task Invoke(MiddlewareContext context)
        {
            var output = Path.ChangeExtension(context.ItemPath.ToString(), ".html");
            new FsPath(output).WriteFile("");
            await _next(context);
        }
    }
}
