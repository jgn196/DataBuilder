namespace Capgemini.DataBuilder
{
    /// <summary>
    /// An enumeration of byte orders (or endianness) that are supported by DataBuilder.
    /// </summary>
    public enum ByteOrder
    {
        /// <summary>
        /// The least significant bytes are ordered first.
        /// </summary>
        LittleEndian, 
        /// <summary>
        /// The most significant bytes are ordered first.
        /// </summary>
        BigEndian
    };
}
