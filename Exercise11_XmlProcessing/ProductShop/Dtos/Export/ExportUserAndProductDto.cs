﻿namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class ExportUserAndProductDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public SoldProductDto SoldProductsDto { get; set; }

        //[XmlType("SoldProducts")]
        

        // <Users>
        // <count>54</count>
        // <users>
        // <User>
        //    <firstName>Cathee</firstName>
        //    <lastName>Rallings</lastName>
        //    <age>33</age>
        //    <SoldProducts>
        //      <count>9</count>
        //      <products>
        //        <Product>
        //          <name>Fair Foundation SPF 15</name>
        //          <price>1394.24</price>
        //        </Product>
    }

    public class SoldProductDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ProductDto[] ProductDtos { get; set; }
    }
}
