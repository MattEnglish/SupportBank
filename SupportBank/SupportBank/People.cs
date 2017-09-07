using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    public static class People
    {
        public static List<Person> people = new List<Person>();

        public static bool doesPersonExist(string theName)
        {
            foreach (Person p in People.people)
            {
                if (p.name == theName)
                {
                    return true;
                }
            }
            return false;
        }

        public static Person GetPersonWithName(string theName)
        {
            foreach (Person p in people)
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
