using gitbook.core;
using gitbook.core.middleware;
using gitbook.flow;
using System;
using System.Threading.Tasks;

namespace gitbook
{
    class Program
    {
        static async Task Main(string[] args)
        {
            PipelineBuilder pipelineBuilder = new PipelineBuilder();
            pipelineBuilder.Use<StartInit>();
            pipelineBuilder.Use(async (ctx, next) =>
            {
                await next(ctx);
            });

            PipelineBuilder item_pipelineBuilder = new PipelineBuilder();
            item_pipelineBuilder.Use<FlowCommonMarkToHtml>();
            item_pipelineBuilder.Use<FlowTemplateProcessor>();
            

            pipelineBuilder.Use<StartProcess>(item_pipelineBuilder.Build());

            Pipeline pipeline = pipelineBuilder.Build();

            var context = new MiddlewareContext("E:/openjw/open")
            {
                HandlerExecutor = async () =>
                {
                    var response = await Task.FromResult(1);
                }
            };

            await pipeline.Process(context);

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
