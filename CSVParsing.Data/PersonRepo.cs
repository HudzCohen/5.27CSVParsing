using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVParsing.Data
{
    public class PersonRepo
    {
        private readonly string _connectionString;

        public PersonRepo(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<Person> GetPeople()
        {
            using var ctx = new PeopleDataContext(_connectionString);
            return ctx.People.ToList();
        }

        public void DeleteAll()
        {
            using var ctx = new PeopleDataContext(_connectionString);
            ctx.Database.ExecuteSqlRaw("TRUNCATE TABLE People");
        }

        public List<Person> AddPeople(List<Person> people)
        {
            using var ctx = new PeopleDataContext(_connectionString);
            ctx.AddRange(people);
            ctx.SaveChanges();
            return ctx.People.ToList();
        }
    }
}
