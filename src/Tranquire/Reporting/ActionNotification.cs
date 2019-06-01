namespace Tranquire.Reporting
{
    /// <summary>
    /// Represent informations about the action execution
    /// </summary>
    public struct ActionNotification
    {
        /// <summary>
        /// The action that trigerred the notification
        /// </summary>
        public INamed Action { get; }
        /// <summary>
        /// The action call stack depth
        /// </summary>
        public int Depth { get; }
        /// <summary>
        /// The action content
        /// </summary>
        public IActionNotificationContent Content { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ActionNotification"/>
        /// </summary>
        /// <param name="action"></param>
        /// <param name="depth"></param>
        /// <param name="content"></param>
        public ActionNotification(INamed action, int depth, IActionNotificationContent content)
        {
            Action = action ?? throw new System.ArgumentNullException(nameof(action));
            Depth = depth;
            Content = content ?? throw new System.ArgumentNullException(nameof(content));
        }
    }
}
