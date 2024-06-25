using CsvHelper.Configuration;
using CsvHelper;
using CSVParsing.Data;
using Faker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using CSVParsing.Web.ViewModels;

namespace CSVParsing.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSVParsingController : ControllerBase
    {

        private readonly string _connectionString;

        public CSVParsingController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost("upload")]
        public void Upload(UploadViewModel vm)
        {
            var base64 = vm.Base64.Substring(vm.Base64.IndexOf(',') + 1);
            byte[] bytes = Convert.FromBase64String(base64);
            using var memoryStream = new MemoryStream(bytes);
            using var reader = new StreamReader(memoryStream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var people = csv.GetRecords<Person>().ToList();
            var repo = new PeopleRepository(_connectionString);
            repo.Add(people);
        }

        [HttpGet("generate")]
        public IActionResult Generate(int amount)
        {
            var people = GeneratePeople(amount);
            using var writer = new StringWriter();
            using var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csvWriter.WriteRecords(people);
            var csvBytes = Encoding.UTF8.GetBytes(writer.ToString());
            return File(csvBytes, "text/csv", "people.csv"); 
        }

        [HttpGet("getpeople")]
        public List<Person> GetPeople()
        {
            var repo = new PeopleRepository(_connectionString);
            return repo.GetPeople();
        }

        [HttpPost("deleteall")]
        public void DeleteAll()
        {
            var repo = new PeopleRepository(_connectionString);
            repo.DeleteAll();
        }

        private List<Person> GeneratePeople(int amount)
        {
            return Enumerable.Range(1, amount).Select(_ => new Person
            {
                FirstName = Name.First(),
                LastName = Name.Last(),
                Age = RandomNumber.Next(20, 80),
                Address = Address.StreetAddress(), 
                Email = Internet.Email()
            }).ToList(); ;
        }
    }
}
