namespace SoftJail.DataProcessor.ImportDto
{

    using System.Xml.Serialization;


    [XmlType("Prisoner")]
    public class PrisonerDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        // <Prisoner id = "15" />
        //      </ Prisoners >
    }
}