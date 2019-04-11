namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            //            var ns = new XmlSerializerNamespaces();
            //            ns.Add("", "");

            var prisoners = context.Prisoners.Where(p => ids.Contains(p.Id))
                .Select(p => new ExportPrisonerDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(po => new ExportOfficerDto
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(o => o.OfficerName)
                        .ToArray(),

                    TotalOfficerSalary = p.PrisonerOfficers.Sum(po => po.Officer.Salary)
                }).OrderBy(p => p.Name).
                ThenBy(p => p.Id)
                .ToArray();

            var json = JsonConvert.SerializeObject(prisoners, Formatting.Indented);

            return json;

        }

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {


            ;
            string[] names = prisonersNames
                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                 .ToArray();

            var prisoners = context.Prisoners
                .Where(p => names.Contains(p.FullName))
               .Select(p => new ExportPrisonerInboxDto
               {
                   Id = p.Id,
                   Name = p.FullName,
                   IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                   EncryptedMessages = p.Mails.Select(m => new ExportEncryptedMessageDto
                   {
                       Description = ReverseString(m.Description)
                   }).ToArray()
               })
               .OrderBy(p => p.Name)
               .ThenBy(p => p.Id)
               .ToArray();
            string result = ExportXml(prisoners, "Prisoners");

            ;
            return result;
        }

        private static string ExportXml(object[] entities, string rootAttributeName)
        {

            var T = entities.GetType();

            XmlSerializer xmlSerializer = new XmlSerializer(T,
                            new XmlRootAttribute(rootAttributeName));

            var sb = new StringBuilder();

            var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), entities, ns);

            return sb.ToString();
        }
    }
}