namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            Department[] allDepartments = JsonConvert.DeserializeObject<Department[]>(jsonString);

            var validDepartments = new List<Department>();

            var sb = new StringBuilder();

            foreach (var department in allDepartments)
            {
                var isValid = IsValid(department) && department.Cells.All(IsValid);

                if (isValid)
                {
                    validDepartments.Add(department);
                    sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                }
                else
                {
                    sb.AppendLine("Invalid Data");
                }


            }
            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            ImportPrisonerMailDto[] allPrisonersDto = JsonConvert.DeserializeObject<ImportPrisonerMailDto[]>(jsonString);
            ;

            var validPrisoners = new List<Prisoner>();
            var sb = new StringBuilder();

            foreach (var prisonerDto in allPrisonersDto)
            {
                if (IsValid(prisonerDto) &&
                    String.IsNullOrEmpty(prisonerDto.FullName) == false &&
                    prisonerDto.Mails.All(IsValid))
                {
                    DateTime? releaseDateDto = prisonerDto.ReleaseDate == null ?
                        (DateTime?)null : DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    Prisoner prisoner = new Prisoner
                    {
                        FullName = prisonerDto.FullName,
                        Nickname = prisonerDto.Nickname,
                        Age = prisonerDto.Age,
                        IncarcerationDate = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ReleaseDate = releaseDateDto,
                        CellId = prisonerDto.CellId,
                        Mails = prisonerDto.Mails.Select(m => new Mail
                        {
                            Description = m.Description,
                            Sender = m.Sender,
                            Address = m.Address

                        }).ToArray()
                    };

                    validPrisoners.Add(prisoner);
                    sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");

                }
                else
                {
                    sb.AppendLine("Invalid Data");
                }
            }
            ;

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            //  sb.AppendLine($"{validPrisoners.Count} m:{validPrisoners.Sum(v => v.Mails.Count)}");


            return sb.ToString().TrimEnd();

        }
        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportOfficerPrisonerDto[]),
                new XmlRootAttribute("Officers"));


            var allOfficers = (ImportOfficerPrisonerDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            var officers = new List<Officer>();
            ;
            var sb = new StringBuilder();

            foreach (var officerDto in allOfficers)
            {
                bool isPositionValid = Enum.TryParse<Position>(officerDto.Position, out Position officerPosition);
                bool isWeaponValid = Enum.TryParse<Weapon>(officerDto.Weapon, out Weapon officerWeapon);


                if (IsValid(officerDto) &&
                    officerDto.OfficerPrisoners.All(IsValid) &&
                    isPositionValid && isWeaponValid
                    )
                {
                    ;
                    Officer officer = new Officer
                    {
                        FullName = officerDto.FullName,
                        Salary = officerDto.Salary,
                        Position = officerPosition,
                        Weapon = officerWeapon,
                        DepartmentId=officerDto.DepartmentId,
                        OfficerPrisoners = officerDto.OfficerPrisoners.Select(p => new OfficerPrisoner
                        {
                            PrisonerId = p.Id
                        }).ToArray()

                    };


                    officers.Add(officer);

                    sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners)");
                }
                else
                {
                    sb.AppendLine("Invalid Data");
                }
                ;
            }

            context.Officers.AddRange(officers);

            context.SaveChanges();
            return sb.ToString();
        }


        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}