﻿

namespace CarDealer.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("customer")]
    public class ExportCustomerTotalSalesDto
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }

        // <customer full-name="Taina Achenbach" bought-cars="1" spent-money="5588.17" />
        // <customer full-name="Johnette Derryberry" bought-cars="1" spent-money="2694.84" />
        // <customer full-name="Jimmy Grossi" bought-cars="1" spent-money="2366.38" />
    }
}
