using System;
using SSG.Core.Plugins;
using SSG.Services.Events;

namespace SSG.Plugin.SMS.Verizon
{
    public class OrderPlacedEventConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly VerizonSettings _verizonSettings;
        private readonly IPluginFinder _pluginFinder;
        private readonly IOrderService _orderService;

        public OrderPlacedEventConsumer(VerizonSettings verizonSettings,
            IPluginFinder pluginFinder, IOrderService orderService)
        {
            this._verizonSettings = verizonSettings;
            this._pluginFinder = pluginFinder;
            this._orderService = orderService;
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            //is enabled?
            if (!_verizonSettings.Enabled)
                return;

            //is plugin installed?
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("Mobile.SMS.Verizon");
            if (pluginDescriptor == null)
                return;

            var plugin = pluginDescriptor.Instance() as VerizonSmsProvider;
            if (plugin == null)
                return;

            var order = eventMessage.Order;
            //send SMS
            if (plugin.SendSms(String.Format("New order(#{0}) has been placed.", order.Id)))
            {
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = "\"Order placed\" SMS alert (to site owner) has been sent",
                    DisplayToUser = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(order);
            }
        }
    }
}