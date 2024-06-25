using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParsing.Data
{
    public class PeopleRepository
    {
        private readonly string _connectionString;

        public PeopleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(List<Person> people)
        {
            var context = new CSVParsingDataContext(_connectionString);
            context.People.AddRange(people);
            context.SaveChanges();
        }

        public List<Person> GetPeople()
        {
            var context = new CSVParsingDataContext(_connectionString);
            return context.People.ToList();
        }

        public void DeleteAll()
        {
            var context = new CSVParsingDataContext(_connectionString);
            context.Database.ExecuteSqlRaw("DELETE FROM People");
            context.SaveChanges();
        }
    }
}
