using System;
using System.Threading.Tasks;

namespace gitbook.core.middleware
{
    /// <summary>
    /// MiddlewareContext
    /// </summary>
    public class MiddlewareContext
    {

        public MiddlewareContext(string sourcePath, string outputPath = "", string pluginPath = "") 
        {
            SourcePath = new FsPath(sourcePath);
            if (string.IsNullOrWhiteSpace(outputPath)) 
            {
                outputPath = "_output";
            }
            OutputPath = SourcePath.Combine(outputPath);
            if (string.IsNullOrWhiteSpace(pluginPath))
            {
                pluginPath = "plugins";
            }
            PluginPath = SourcePath.Combine(pluginPath);           
        }

        public FsPath SourcePath { get;  }

        public FsPath OutputPath { get; }

        public string ItemPath { get; set; }

        // 输出;
        public string ItemResponse { get; set; }

        public FsPath PluginPath { get; }

        /// <summary>
        /// Final handler.
        /// </summary>
        public Func<Task> HandlerExecutor { get; set; }
    }
}