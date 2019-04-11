
namespace SoftJail.DataProcessor.ExportDto
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("Prisoner")]
    public class ExportPrisonerInboxDto
    {


        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("IncarcerationDate")]
        public String IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public ExportEncryptedMessageDto[] EncryptedMessages { get; set; }

    }

    [XmlType("Message")]
    public class ExportEncryptedMessageDto
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
