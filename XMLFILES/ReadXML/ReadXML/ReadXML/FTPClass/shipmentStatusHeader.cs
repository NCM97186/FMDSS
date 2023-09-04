using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
    [XmlRoot(ElementName = "shipmentStatusHeader")]
    public class ShipmentStatusHeader
    {
        [XmlElement(ElementName = "SenderID")]
        public string SenderID { get; set; }
        [XmlElement(ElementName = "ReceiverID")]
        public string ReceiverID { get; set; }
        [XmlElement(ElementName = "TransmissionDate")]
        public string TransmissionDate { get; set; }
        [XmlElement(ElementName = "TransmissionTime")]
        public string TransmissionTime { get; set; }
    }

    [XmlRoot(ElementName = "shipmentPackageInfo")]
    public class ShipmentPackageInfo
    {
        [XmlElement(ElementName = "articleNumber")]
        public string ArticleNumber { get; set; }
        [XmlElement(ElementName = "referenceNumber")]
        public string ReferenceNumber { get; set; }
    }

    [XmlRoot(ElementName = "itemEvent")]
    public class ItemEvent
    {
        [XmlElement(ElementName = "activityCode")]
        public string ActivityCode { get; set; }
        [XmlElement(ElementName = "activityDateTime")]
        public string ActivityDateTime { get; set; }
        [XmlElement(ElementName = "officeName")]
        public string OfficeName { get; set; }
        [XmlElement(ElementName = "reasonCode")]
        public string ReasonCode { get; set; }
        [XmlElement(ElementName = "activityCity")]
        public string ActivityCity { get; set; }
        [XmlElement(ElementName = "activityState")]
        public string ActivityState { get; set; }
    }

    [XmlRoot(ElementName = "itemEvents")]
    public class ItemEvents
    {
        [XmlElement(ElementName = "shipmentPackageInfo")]
        public ShipmentPackageInfo ShipmentPackageInfo { get; set; }
        [XmlElement(ElementName = "itemEvent")]
        public ItemEvent ItemEvent { get; set; }
    }

    [XmlRoot(ElementName = "shipmentStatusDetail")]
    public class ShipmentStatusDetail
    {
        [XmlElement(ElementName = "itemEvents")]
        public List<ItemEvents> ItemEvents { get; set; }
    }

    [XmlRoot(ElementName = "ShipmentInformation")]
    public class ShipmentInformation
    {
        [XmlElement(ElementName = "shipmentStatusHeader")]
        public ShipmentStatusHeader ShipmentStatusHeader { get; set; }
        [XmlElement(ElementName = "shipmentStatusDetail")]
        public ShipmentStatusDetail ShipmentStatusDetail { get; set; }
    }

}
