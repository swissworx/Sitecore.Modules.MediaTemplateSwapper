// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwapperConfiguration.cs" company="">
//   
// </copyright>
// <summary>
//   The media template swapper configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Swissworx.Modules.MediaTemplateSwapper
{
    using Sitecore.Data;

    /// <summary>The media template swapper configuration.</summary>
    public class SwapperConfiguration
    {
        #region Public Properties

        /// <summary>Gets or sets the source template id.</summary>
        public ID SourceTemplateId { get; set; }

        /// <summary>Gets or sets the target template id.</summary>
        public ID TargetTemplateId { get; set; }

        #endregion
    }
}