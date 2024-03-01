using System.Collections.Generic;
using System.Linq;
using System;

namespace DataService.Core.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataNamesAttribute : Attribute
    {
        protected List<string> _valueNames { get; set; }

        public List<string> ValueNames
        {
            get
            {
                return _valueNames;
            }
            set
            {
                _valueNames = value;
            }
        }

        public DataNamesAttribute()
        {
            _valueNames = new List<string>();
        }

        public DataNamesAttribute(params string[] valueNames)
        {
            _valueNames = valueNames.ToList();
        }
    }


    // USAGE: 
    //public class Person
    //{
    //    [DataNames("first_name", "firstName")]
    //    public string FirstName { get; set; }

    //    [DataNames("last_name", "lastName")]
    //    public string LastName { get; set; }

    //    [DataNames("dob", "dateOfBirth")]
    //    public DateTime DateOfBirth { get; set; }

    //    [DataNames("job_title", "jobTitle")]
    //    public string JobTitle { get; set; }

    //    [DataNames("taken_name", "nickName")]
    //    public string TakenName { get; set; }

    //    [DataNames("is_american", "isAmerican")]
    //    public bool IsAmerican { get; set; }
    //}
}
