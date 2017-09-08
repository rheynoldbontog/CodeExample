using System;
using SSG.Core.Domain.Messages;
using SSG.Core.Plugins;
using SSG.Plugin.Misc.MailChimp.Data;
using SSG.Services.Events;

namespace SSG.Plugin.Misc.MailChimp.Services
{
    public class SubscriptionEventConsumer : IConsumer<EmailSubscribedEvent>, IConsumer<EmailUnsubscribedEvent>
    {
        private readonly ISubscriptionEventQueueingService _service;
        private readonly IPluginFinder _pluginFinder;

        public SubscriptionEventConsumer(ISubscriptionEventQueueingService service,
            IPluginFinder pluginFinder)
        {
            this._service = service;
            this._pluginFinder = pluginFinder;
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public  void HandleEvent(EmailSubscribedEvent eventMessage)
        {
            //is plugin installed?
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("Misc.MailChimp");
            if (pluginDescriptor == null)
                return;
            
            _service.Insert(new MailChimpEventQueueRecord { Email = eventMessage.Email, IsSubscribe = true, CreatedOnUtc = DateTime.UtcNow});
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(EmailUnsubscribedEvent eventMessage)
        {
            //is plugin installed?
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("Misc.MailChimp");
            if (pluginDescriptor == null)
                return;

            _service.Insert(new MailChimpEventQueueRecord { Email = eventMessage.Email, IsSubscribe = false, CreatedOnUtc = DateTime.UtcNow });
        }
    }
}