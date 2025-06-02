namespace MessageBus
{
    public interface IMessageBus
    {
        /// <summary>
        /// Write a message to the message bus
        /// </summary>
        /// <param name="messageQueue">Queue to send a message on</param>
        /// <param name="payload">Message to send</param>
        /// <returns>True if write was succesfull. False if write failed.</returns>
        bool Write(string messageQueue, string payload);
        bool TryReadMessage(string messageQueue, out string message);
    }
}
