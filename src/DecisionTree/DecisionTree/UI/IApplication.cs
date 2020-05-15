namespace DecisionTree.UI
{
    /// <summary>
    /// Represents the IApplication interface.
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Starts the UI and user interaction.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the application.
        /// </summary>
        void Exit();
    }
}