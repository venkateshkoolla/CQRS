using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class CollegeExtension
    {
        public static IEnumerable<College> WithCampuses(this IEnumerable<College> colleges, IEnumerable<Campus> campuses)
        {
            var list = new List<College>();
            foreach (var college in colleges)
            {
                if (campuses.Any(s => s.CollegeId == college.Id))
                {
                    list.Add(college);
                }
            }

            if (list.Count == 0) throw new Exception("Colleges with campuses could not be found");

            return list;
        }

        public static IList<College> WithAppCycle(this IList<College> colleges, IList<CollegeApplicationCycle> collegeApplicationCycles, Guid appCycleId)
        {
            var list = new List<College>();
            foreach (var college in colleges)
            {
                if (collegeApplicationCycles.Any(s => s.CollegeId == college.Id && s.MasterId == appCycleId))
                {
                    list.Add(college);
                }
            }

            if (list.Count == 0) throw new Exception("Colleges with campuses could not be found");

            return list;
        }
    }
}
