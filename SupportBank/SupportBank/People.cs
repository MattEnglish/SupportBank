using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    public class People
    {
        public static List<Person> people = new List<Person>();
        public bool DoesPersonExist(string theName)
        {
            
            foreach (Person p in People.people)//lambda expression
            {
                if (p.name == theName)
                {
                    return true;
                }
            }
            return false;
        }

        public Person GetPersonWithName(string theName)
        {
            foreach (Person p in people)//lambda
            {
                if (p.name == theName)
                {
                    return p;
                }
            }
            throw new Exception();
        }

    }
}
