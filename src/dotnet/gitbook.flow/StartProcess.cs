using gitbook.core;
using gitbook.core.middleware;
using System;
using System.Threading.Tasks;

namespace gitbook.flow
{
    public class StartProcess : IStart
    {
        public string Name => "Process Item";

        private readonly PipelineDelagate _next;
        private Pipeline _item_pipeline { get; set; }

        public StartProcess(PipelineDelagate next, Pipeline pipeline)
        {
            _next = next;
            _item_pipeline = pipeline;
        }

        public async Task Invoke(MiddlewareContext context)
        {
            foreach (var file in context.SourcePath.GetAllFiles()) 
            {
                // 相对路径;
                var rela = context.SourcePath.GetRelativePath(file.ToString());
                // 复制文件;
                file.Copy(context.OutputPath.Combine(rela),true);
                // 将md文件塞进管道进行处理;
                if (string.Equals(file.Extension, ".md", StringComparison.OrdinalIgnoreCase))
                {
                    await _item_pipeline.Process(new MiddlewareContext(context.SourcePath.ToString(), context.OutputPath.ToString(), context.PluginPath.ToString())
                    {
                        ItemPath = file.ToString(),
                        ItemResponse = file.ReadFile(),
                    });
                }
            }
            await _next(context);
        }

    }
}
