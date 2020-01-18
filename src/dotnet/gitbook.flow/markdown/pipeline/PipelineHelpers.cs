using gitbook.core;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;

namespace gitbook.flow.markdown.pipeline
{
    internal static class PipelineHelpers
    {
        public static void SetupSyntaxRender(IMarkdownRenderer renderer)
        {
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            if (!(renderer is TextRendererBase<HtmlRenderer> htmlRenderer)) return;

            var originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null)
            {
                htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);

                htmlRenderer.ObjectRenderers.AddIfNotAlready(new SyntaxRenderer(originalCodeBlockRenderer));
            }
        }

        public static string ToImgCacheKey(string url, FsPath outputDir)
        {
            FsPath requested = new FsPath(url);
            return requested.GetAbsolutePathRelativeTo(outputDir).ToString();
        }

        private static void AddStyleClass(MarkdownObject node, string style)
        {
            if (string.IsNullOrEmpty(style)) return;
            node.GetAttributes().AddClass(style);
        }

        public static void ApplyStyles(StyleClasses style, MarkdownDocument document)
        {           
            foreach (var node in document.Descendants())
            {
                if (node is HeadingBlock heading)
                {
                    switch (heading.Level)
                    {
                        case 1:
                            AddStyleClass(node, style.Heading1);
                            break;
                        case 2:
                            AddStyleClass(node, style.Heading2);
                            break;
                        case 3:
                            AddStyleClass(node, style.Heading3);
                            break;
                    }
                }
                else if (node is Block)
                {
                    if (node is Markdig.Extensions.Tables.Table)
                        AddStyleClass(node, style.Table);
                    else if (node is QuoteBlock)
                        AddStyleClass(node, style.Blockquote);
                    else if (node is Markdig.Extensions.Figures.Figure)
                        AddStyleClass(node, style.Figure);
                    else if (node is Markdig.Extensions.Figures.FigureCaption)
                        AddStyleClass(node, style.FigureCaption);
                }
                else if (node is LinkInline link)
                {
                    if (link.IsImage)
                    {
                        AddStyleClass(link, style.Image);
                    }
                    else
                    {
                        AddStyleClass(node, style.Link);
                    }
                }
                else if (node is ListBlock listBlock)
                {
                    if (listBlock.IsOrdered)
                        AddStyleClass(node, style.OrderedList);
                    else
                        AddStyleClass(node, style.UnorederedList);
                }
                else if (node is ListItemBlock)
                {
                    AddStyleClass(node, style.ListItem);
                }
            }
        }
    }
}
