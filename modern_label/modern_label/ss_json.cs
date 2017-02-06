﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modern_label
{
    class ss_json
    {

        public class Rootobject
        {
            public Order[] orders { get; set; }
            public int total { get; set; }
            public int page { get; set; }
            public int pages { get; set; }
        }

        public class Order
        {
            public int orderId { get; set; }
            public string orderNumber { get; set; }
            public string orderKey { get; set; }
            public string orderDate { get; set; }
            public string createDate { get; set; }
            public DateTime modifyDate { get; set; }
            public DateTime? paymentDate { get; set; }
            public object shipByDate { get; set; }
            public string orderStatus { get; set; }
            public int? customerId { get; set; }
            public string customerUsername { get; set; }
            public string customerEmail { get; set; }
            public Billto billTo { get; set; }
            public Shipto shipTo { get; set; }
            public Item[] items { get; set; }
            public float orderTotal { get; set; }
            public float amountPaid { get; set; }
            public float taxAmount { get; set; }
            public float shippingAmount { get; set; }
            public string customerNotes { get; set; }
            public object internalNotes { get; set; }
            public bool gift { get; set; }
            public object giftMessage { get; set; }
            public string paymentMethod { get; set; }
            public string requestedShippingService { get; set; }
            public string carrierCode { get; set; }
            public string serviceCode { get; set; }
            public string packageCode { get; set; }
            public string confirmation { get; set; }
            public string shipDate { get; set; }
            public object holdUntilDate { get; set; }
            public Weight weight { get; set; }
            public Dimensions dimensions { get; set; }
            public Insuranceoptions insuranceOptions { get; set; }
            public Internationaloptions internationalOptions { get; set; }
            public Advancedoptions advancedOptions { get; set; }
            public object tagIds { get; set; }
            public object userId { get; set; }
            public bool externallyFulfilled { get; set; }
            public object externallyFulfilledBy { get; set; }
        }

        public class Billto
        {
            public string name { get; set; }
            public object company { get; set; }
            public object street1 { get; set; }
            public object street2 { get; set; }
            public object street3 { get; set; }
            public object city { get; set; }
            public object state { get; set; }
            public object postalCode { get; set; }
            public object country { get; set; }
            public object phone { get; set; }
            public object residential { get; set; }
            public object addressVerified { get; set; }
        }

        public class Shipto
        {
            public string name { get; set; }
            public object company { get; set; }
            public string street1 { get; set; }
            public string street2 { get; set; }
            public object street3 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string postalCode { get; set; }
            public string country { get; set; }
            public string phone { get; set; }
            public bool residential { get; set; }
            public string addressVerified { get; set; }
        }

        public class Weight
        {
            public float value { get; set; }
            public string units { get; set; }
        }

        public class Dimensions
        {
            public string units { get; set; }
            public float length { get; set; }
            public float width { get; set; }
            public float height { get; set; }
        }

        public class Insuranceoptions
        {
            public string provider { get; set; }
            public bool insureShipment { get; set; }
            public float insuredValue { get; set; }
        }

        public class Internationaloptions
        {
            public string contents { get; set; }
            public object customsItems { get; set; }
            public string nonDelivery { get; set; }
        }

        public class Advancedoptions
        {
            public int warehouseId { get; set; }
            public bool nonMachinable { get; set; }
            public bool saturdayDelivery { get; set; }
            public bool containsAlcohol { get; set; }
            public bool mergedOrSplit { get; set; }
            public int?[] mergedIds { get; set; }
            public object parentId { get; set; }
            public int storeId { get; set; }
            public object customField1 { get; set; }
            public object customField2 { get; set; }
            public object customField3 { get; set; }
            public object source { get; set; }
            public object billToParty { get; set; }
            public object billToAccount { get; set; }
            public object billToPostalCode { get; set; }
            public object billToCountryCode { get; set; }
            public object billToMyOtherAccount { get; set; }
        }

        public class Item
        {
            public int orderItemId { get; set; }
            public string lineItemKey { get; set; }
            public string sku { get; set; }
            public string name { get; set; }
            public string imageUrl { get; set; }
            public Weight1 weight { get; set; }
            public int quantity { get; set; }
            public float unitPrice { get; set; }
            public float taxAmount { get; set; }
            public object shippingAmount { get; set; }
            public object warehouseLocation { get; set; }
            public object[] options { get; set; }
            public object productId { get; set; }
            public object fulfillmentSku { get; set; }
            public bool adjustment { get; set; }
            public object upc { get; set; }
            public DateTime createDate { get; set; }
            public DateTime modifyDate { get; set; }
        }

        public class Weight1
        {
            public float value { get; set; }
            public string units { get; set; }
        }

    }
}
