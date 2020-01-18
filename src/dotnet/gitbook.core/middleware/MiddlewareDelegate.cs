using System.Threading.Tasks;

namespace gitbook.core.middleware
{
    /// <summary>
    /// Middleware pipeline.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public delegate Task PipelineDelagate(MiddlewareContext context);
}