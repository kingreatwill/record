using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;

namespace gitbook.flow.markdown.pipeline
{
    internal class WebModifier : IMarkdownExtension
    {
        public static StyleClasses RuntimeConfig { get; set; }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
            pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // Method intentionally left empty.
        }

        private static bool IsOffHostLink(LinkInline link)
        {
            return true;
            //return !link.Url.StartsWith(RuntimeConfig?.Configuration.HostName);
        }

        private static void PipelineOnDocumentProcessed(MarkdownDocument document)
        {
            //if (RuntimeConfig == null)
            //    throw new InvalidOperationException("Settings not configured");

            //PipelineHelpers.ApplyStyles(new StyleClasses(), document);

            //foreach (var node in document.Descendants())
            //{
            //    if (node is LinkInline link)
            //    {
            //        if (link.IsImage && RuntimeConfig.InlineImgCache?.Count > 0)
            //        {
            //            var inlinekey = PipelineHelpers.ToImgCacheKey(link.Url, RuntimeConfig.OutputDirectory);
            //            if (RuntimeConfig.InlineImgCache.ContainsKey(inlinekey))
            //            {
            //                link.Url = RuntimeConfig.InlineImgCache[inlinekey];
            //            }
            //        }
            //        else if (IsOffHostLink(link) && RuntimeConfig.Configuration.LinksOutSideOfHostOpenNewTab)
            //        {
            //            link.GetAttributes().AddProperty("target", "_blank");
            //        }
            //    }
            //}
        }
    }


    public class StyleClasses
    {
      
        public string Heading1
        {
            get;
            set;
        }
      
        public string Heading2
        {
            get;
            set;
        }
 
        public string Heading3
        {
            get;
            set;
        }
       
        public string Image
        {
            get;
            set;
        }
     
        public string Table
        {
            get;
            set;
        }
               
        public string Blockquote
        {
            get;
            set;
        }
      
        public string Figure
        {
            get;
            set;
        }
       
        public string FigureCaption
        {
            get;
            set;
        }
               
        public string Link
        {
            get;
            set;
        }
       
        public string OrderedList
        {
            get;
            set;
        }
       
        public string UnorederedList
        {
            get;
            set;
        }
       
        public string ListItem
        {
            get;
            set;
        }

        public StyleClasses()
        {
            Heading1 = string.Empty;
            Heading2 = string.Empty;
            Heading3 = string.Empty;
            Image = string.Empty;// "img-fluid mx-auto rounded";
            Table = string.Empty; //"table";
            Blockquote = string.Empty;// "blockquote";
            Figure = string.Empty;//"figure";
            FigureCaption = string.Empty;//"figure-caption";
            Link = string.Empty;
            OrderedList = string.Empty;
            UnorederedList = string.Empty;
            ListItem = string.Empty;
        }
    }
}
