﻿using System;
using System.IO;
using System.Linq;

namespace VDT.Core.XmlConverter.Markdown {
    /// <summary>
    /// Base converter for rendering elements as Markdown
    /// </summary>
    public abstract class BaseElementConverter : IElementConverter {
        private const string listItemName = "li";

        private readonly string[] validForElementNames;

        /// <summary>
        /// Constructs an instance of a base Markdown element converter
        /// </summary>
        /// <param name="validForElementNames">Element names for which this converter is valid; names are case-insensitive</param>
        protected BaseElementConverter(params string[] validForElementNames) {
            this.validForElementNames = validForElementNames;
        }
        
        /// <inheritdoc/>
        public virtual bool IsValidFor(ElementData elementData) => validForElementNames.Any(e => string.Equals(e, elementData.Name, StringComparison.OrdinalIgnoreCase));

        /// <inheritdoc/>
        public abstract void RenderStart(ElementData elementData, TextWriter writer);

        /// <inheritdoc/>
        public virtual bool ShouldRenderContent(ElementData elementData) => true;

        /// <inheritdoc/>
        public abstract void RenderEnd(ElementData elementData, TextWriter writer);

        internal int GetAncestorListItemCount(ElementData elementData) => elementData.Ancestors.Count(d => string.Equals(d.Name, listItemName, StringComparison.OrdinalIgnoreCase));
    }
}
