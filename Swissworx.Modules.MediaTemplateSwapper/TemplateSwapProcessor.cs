// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateSwapProcessor.cs" company="">
//   
// </copyright>
// <summary>
//   The template swap processor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Swissworx.Modules.MediaTemplateSwapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.Upload;
    using Sitecore.SecurityModel;

    /// <summary>The template swap processor.</summary>
    public class TemplateSwapProcessor
    {
        #region Fields

        /// <summary> The database. </summary>
        private readonly Database database;

        #endregion

        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="TemplateSwapProcessor"/> class.</summary>
        public TemplateSwapProcessor()
        {
            this.database = Database.GetDatabase("master");
            this.SwapConfigurations = new List<SwapperConfiguration>();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the name.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the media root path for items considered in scope for this processor instance.</summary>
        public string MediaRootPath { get; set; }

        /// <summary>Gets or sets the templates. </summary>
        public List<SwapperConfiguration> SwapConfigurations { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>If the file uploaded matches one of the paths specified in the configuration will try to change the template.</summary>
        /// <param name="args">The args. </param>
        public void Process(UploadArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            using (new SecurityDisabler())
            {
                Item uploadedItem = this.database.GetItem(args.Folder);
                if (uploadedItem?.Paths != null)
                {
                    string uploadPath = uploadedItem.Paths.ContentPath;
                    if (!uploadPath.StartsWith(this.MediaRootPath, StringComparison.OrdinalIgnoreCase))
                    {
                        // Item has not been uploaded to the path configured in scope for this processor
                        return;
                    }

                    foreach (Item item in args.UploadedItems)
                    {
                        // Check whether the processor has a swap config that matches the source template id
                        SwapperConfiguration swapConfiguration = this.SwapConfigurations.FirstOrDefault(config => config.SourceTemplateId == item.Template.ID);
                        if (swapConfiguration != null)
                        {
                            TemplateItem targetTemplate = this.database.Templates[swapConfiguration.TargetTemplateId];
                            try
                            {
                                item.ChangeTemplate(targetTemplate);
                            }
                            catch (Exception e)
                            {
                                Log.Error(
                                    $"Failed changing source template {swapConfiguration.SourceTemplateId} to target template {swapConfiguration.TargetTemplateId} for item '{item.Name}'.",
                                    e,
                                    this);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}