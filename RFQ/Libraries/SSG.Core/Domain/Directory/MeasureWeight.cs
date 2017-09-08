namespace SSG.Core.Domain.Directory
{
    /// <summary>
    /// Represents a measure weight
    /// </summary>
    public partial class MeasureWeight : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the system keyword
        /// </summary>
        public virtual string SystemKeyword { get; set; }

        /// <summary>
        /// Gets or sets the ratio
        /// </summary>
        public virtual decimal Ratio { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}
