namespace ToDoList.Automation
{
    /// <summary>
    /// Represent at which level of the application an action or question acts    
    /// </summary>
    public enum TestLevel
    {
        /// <summary>
        /// The action or question acts on the user interface, via Selenium
        /// </summary>
        UI = 0,
        /// <summary>
        /// The action or question acts on the API, via HTTP requests
        /// </summary>
        Api = 1
    }
}
