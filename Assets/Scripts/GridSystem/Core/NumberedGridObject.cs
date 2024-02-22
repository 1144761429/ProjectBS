namespace GridSystem.Core
{
    /// <summary>
    /// <see cref="GridObject"/> with an amount.
    /// </summary>
    public struct NumberedGridObject
    {
        /// <summary>
        /// The <see cref="GridObject"/>.
        /// </summary>
        public readonly GridObject GridObject;

        /// <summary>
        /// The number of <see cref="GridObject"/>.
        /// </summary>
        public int Amount { get; private set; }

        public NumberedGridObject(GridObject gridObject, int amount)
        {
            GridObject = gridObject;
            Amount = amount;
        }
    }
}