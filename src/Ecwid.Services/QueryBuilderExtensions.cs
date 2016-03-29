﻿using System;
using System.Collections.Generic;
using System.Threading;
using Ecwid.Tools;
using System.Threading.Tasks;
using Ecwid.Models.Legacy;

namespace Ecwid.Services
{
    /// <summary>
    /// LINQ like extensions for QueryBuilder
    /// </summary>
    public static class QueryBuilderExtensions
    {
        /// <summary>
        /// Customs the specified name.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static OrdersQueryBuilder Custom(this OrdersQueryBuilder query, string name, object value) => query.AddOrUpdate(name, value);

        /// <summary>
        /// All orders placed this day.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="date">The date.</param>
        public static OrdersQueryBuilder Date(this OrdersQueryBuilder query, DateTime date) => query.AddOrUpdate("date", date.ToString("yyyy-MM-dd"));

        /// <summary>
        /// All orders placed after this date.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="date">The date.</param>
        public static OrdersQueryBuilder FromDate(this OrdersQueryBuilder query, DateTime date) => query.AddOrUpdate("from_date", date.ToString("yyyy-MM-dd"));

        /// <summary>
        /// All orders placed before this date.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="date">The date.</param>
        public static OrdersQueryBuilder ToDate(this OrdersQueryBuilder query, DateTime date) => query.AddOrUpdate("to_date", date.ToString("yyyy-MM-dd"));

        /// <summary>
        /// All orders changed after this date.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="date">The date.</param>
        public static OrdersQueryBuilder FromUpdateDate(this OrdersQueryBuilder query, DateTime date) => query.AddOrUpdate("from_update_date", date.ToString("yyyy-MM-dd"));

        /// <summary>
        /// All orders changed before this date.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="date">The date.</param>
        public static OrdersQueryBuilder ToUpdateDate(this OrdersQueryBuilder query, DateTime date) => query.AddOrUpdate("to_update_date", date.ToString("yyyy-MM-dd"));

        /// <summary>
        /// Set where property by the specified number.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="number">The ordinary order number</param>
        public static OrdersQueryBuilder Order(this OrdersQueryBuilder query, int number) => query.AddOrUpdate("order", number);

        /// <summary>
        /// Set where property by the specified vendor number.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="vendorNumber">The vendor order number (order number with prefix/suffix)</param>
        public static OrdersQueryBuilder Order(this OrdersQueryBuilder query, string vendorNumber) => query.AddOrUpdate("order", vendorNumber);

        /// <summary>
        /// All orders with numbers bigger than or equal to value. 
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="number">The ordinary order number</param>
        public static OrdersQueryBuilder FromOrder(this OrdersQueryBuilder query, int number) => query.AddOrUpdate("from_order", number);

        /// <summary>
        /// All orders with numbers bigger than or equal to value.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="vendorNumber">The vendor order number (order number with prefix/suffix)</param>
        public static OrdersQueryBuilder FromOrder(this OrdersQueryBuilder query, string vendorNumber) => query.AddOrUpdate("from_order", vendorNumber);

        /// <summary>
        /// Unique numeric customer identifier or null for anonymous orders.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="customerId">The customer identifier.</param>
        public static OrdersQueryBuilder CustomerId(this OrdersQueryBuilder query, int? customerId)
        {
            return customerId == null ? query.AddOrUpdate("customer_id", "null") : query.AddOrUpdate("customer_id", customerId);
        }

        /// <summary>
        /// Customer email or null for orders with empty or absent emails.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="customerEmail">The customer email.</param>
        public static OrdersQueryBuilder CustomerEmail(this OrdersQueryBuilder query, string customerEmail) => query.AddOrUpdate("customer_email", customerEmail ?? "");

        /// <summary>
        /// Set payment and/or fulfillment comma or space separated status names. 
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="paymentStatuses">Payment statuses: PAID==ACCEPTED, DECLINED, CANCELLED, AWAITING_PAYMENT==QUEUED, CHARGEABLE, REFUNDED, INCOMPLETE</param>
        /// <param name="fulfillmentStatuses">Fulfillment statuses: AWAITING_PROCESSING==NEW, PROCESSING, SHIPPED, DELIVERED, WILL_NOT_DELIVER, RETURNED</param>
        public static OrdersQueryBuilder Statuses(this OrdersQueryBuilder query, string paymentStatuses, string fulfillmentStatuses)
        {
            Validators.PaymentStatusesValidate(paymentStatuses);
            Validators.FulfillmentStatusesValidate(fulfillmentStatuses);

            var resultList = paymentStatuses.TrimUpperReplaceSplit();
            resultList.AddRange(fulfillmentStatuses.TrimUpperReplaceSplit());

            return query.AddOrUpdateStatuses(resultList);
        }

        /// <summary>
        /// Add payment comma or space separated status names.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="paymentStatuses">Payment statuses: PAID==ACCEPTED, DECLINED, CANCELLED, AWAITING_PAYMENT==QUEUED, CHARGEABLE, REFUNDED, INCOMPLETE</param>
        public static OrdersQueryBuilder AddPaymentStatuses(this OrdersQueryBuilder query, string paymentStatuses)
        {
            return Validators.PaymentStatusesValidate(paymentStatuses)
                ? query.AddOrUpdateStatuses(paymentStatuses.TrimUpperReplaceSplit())
                : query;
        }

        /// <summary>
        /// Add fulfillment comma or space separated status names.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="fulfillmentStatuses">Fulfillment statuses: AWAITING_PROCESSING==NEW, PROCESSING, SHIPPED, DELIVERED, WILL_NOT_DELIVER, RETURNED</param>
        public static OrdersQueryBuilder AddFulfillmentStatuses(this OrdersQueryBuilder query, string fulfillmentStatuses)
        {
            return Validators.FulfillmentStatusesValidate(fulfillmentStatuses)
                ? query.AddOrUpdateStatuses(fulfillmentStatuses.TrimUpperReplaceSplit())
                : query;
        }

        /// <summary>
        /// Number of orders returned in response. The default and maximal value is 200, any greater value is reset to 200.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="limit">Number of orders returned in response. The default and maximal value is 200, any greater value is reset to 200.</param>
        public static OrdersQueryBuilder Limit(this OrdersQueryBuilder query, int limit) => query.AddOrUpdate("limit", limit);

        /// <summary>
        /// How many orders skip from beginning
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="offset">The offset.</param>
        public static OrdersQueryBuilder Offset(this OrdersQueryBuilder query, int offset) => query.AddOrUpdate("offset", offset);

        /// <summary>
        /// Gets orders the asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        public static async Task<List<LegacyOrder>> GetAsync(this OrdersQueryBuilder query)
        {
            return await query.Client.GetOrdersAsync(query);
        }

        /// <summary>
        /// Gets orders the asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static async Task<List<LegacyOrder>> GetAsync(this OrdersQueryBuilder query, CancellationToken cancellationToken)
        {
            return await query.Client.GetOrdersAsync(query, cancellationToken);
        }
    }
}