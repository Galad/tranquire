using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquire.Reporting
{
    /// <summary>
    /// Represent a file attachment for an action or a question
    /// </summary>
    public sealed class ActionFileAttachment
    {
        /// <summary>
        /// Gets the file path
        /// </summary>
        public string FilePath { get; }
        /// <summary>
        /// Gets a description of the attachment
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ActionFileAttachment"/>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="description"></param>
        public ActionFileAttachment(string filePath, string description)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
