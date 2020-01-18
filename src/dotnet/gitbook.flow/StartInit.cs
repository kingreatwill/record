using gitbook.core;
using gitbook.core.middleware;
using System;
using System.Threading.Tasks;

namespace gitbook.flow
{
    public class StartInit : IStart
    {
        public string Name => "Init";

        private readonly PipelineDelagate _next;

        public StartInit(PipelineDelagate next)
        {
            _next = next;
        }

        public async Task Invoke(MiddlewareContext context)
        {
            // 添加忽略文件;
            GitignoreParser.Default.AddGitignoreFile(context.SourcePath.Combine(".gitignore").ToString());
            GitignoreParser.Default.AddGitignoreRules(".git");
            await _next(context);
        }
       
    }
}
