using CSVParsing.Data;
using CSVParsing.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Faker;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace CSVParsing.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string _connectionString;

        public FileUploadController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpGet("getpeople")]
        public List<Person> GetAllPeople()
        {
            var repo = new PersonRepo(_connectionString);
            return repo.GetPeople();
        }

        [HttpPost("delete")]
        public void DeleteAllPeople()
        {
            var repo = new PersonRepo(_connectionString);
            repo.DeleteAll();
        }

        [HttpPost("upload")]
        public void Upload(UploadViewModel vm)
        {
            int indexOfComma = vm.Base64Data.IndexOf(',');
            string base64 = vm.Base64Data[(indexOfComma + 1)..];
            byte[] bytes = Convert.FromBase64String(base64);
            List<Person> people = GetFromCsvBytes(bytes);

            var repo = new PersonRepo(_connectionString);
            repo.AddPeople(people);
        }

        [HttpGet("generate")]
        public IActionResult GeneratePeople(int amount)
        {
            var people = (Generate(amount));
            var writer = new StringWriter();
            var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csvWriter.WriteRecords(people);
            byte[] csvBytes = Encoding.UTF8.GetBytes(writer.ToString());
            return File(csvBytes, "text/csv", "people.csv");
        }


        private static List<Person> Generate(int amount)
        {
            return Enumerable.Range(1, amount)
                .Select(_ => new Person
                {
                    FirstName = Name.First(),
                    LastName = Name.Last(),
                    Age = RandomNumber.Next(15, 80),
                    Address = Address.StreetAddress(),
                    Email = Internet.Email()
                }).ToList();
        }

        private static List<Person> GetFromCsvBytes(byte[] csvBytes)
        {
            using var memoryStream = new MemoryStream(csvBytes);
            using var reader = new StreamReader(memoryStream);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csvReader.GetRecords<Person>().ToList();
        }
    }
}
