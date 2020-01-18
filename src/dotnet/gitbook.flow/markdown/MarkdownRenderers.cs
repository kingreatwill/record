using gitbook.flow.markdown.pipeline;
using Markdig;

namespace gitbook.flow.markdown
{
    public static class MarkdownRenderers
    {
        private static readonly MarkdownPipeline _webpipeline = new MarkdownPipelineBuilder().Use<WebModifier>().UseAdvancedExtensions().Build();

        /// <summary>
        /// Generate markdown to html
        /// </summary>
        /// <param name="md">Markdown input string</param>
        /// <returns>html page</returns>
        public static string Markdown2WebHTML(string md, StyleClasses settings)
        {
            WebModifier.RuntimeConfig = settings;
            return Markdig.Markdown.ToHtml(md, _webpipeline);
        }
    }
}
