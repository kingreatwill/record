using gitbook.core.middleware;
using System;
using System.Threading.Tasks;

namespace gitbook.core
{
    public interface IPlugin
    {
        string Name { get; }

        Task Invoke(MiddlewareContext context);
    }
}
