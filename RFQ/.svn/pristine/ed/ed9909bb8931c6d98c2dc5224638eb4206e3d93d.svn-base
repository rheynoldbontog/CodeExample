using System.Collections.Generic;
using SSG.Plugin.Misc.MailChimp.Data;

namespace SSG.Plugin.Misc.MailChimp.Services
{
    public interface ISubscriptionEventQueueingService
    {
        /// <summary>
        /// Deletes the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        void Delete(MailChimpEventQueueRecord record);

        /// <summary>
        /// Inserts the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        void Insert(MailChimpEventQueueRecord record);

        /// <summary>
        /// Queues all subscriptions.
        /// </summary>
        void QueueAll();

        /// <summary>
        /// Reads the list.
        /// </summary>
        /// <returns>Result</returns>
        IList<MailChimpEventQueueRecord> GetAll();
    }
}