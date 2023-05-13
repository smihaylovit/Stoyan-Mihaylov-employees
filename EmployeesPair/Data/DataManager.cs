using EmployeesPair.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmployeesPair.Data
{
    public static class DataManager
    {
        private static char csvDelimiter = ',';

        public static List<EmployeeProject> GetEmployeeProjects(string csvFilePath)
        {
            var employeeProjects = new List<EmployeeProject>();

            using (var sr = new StreamReader(csvFilePath))
            {
                var headers = sr.ReadLine().Split(csvDelimiter);

                while (!sr.EndOfStream)
                {
                    var values = sr.ReadLine().Split(csvDelimiter);
                    var emp = new EmployeeProject
                    {
                        EmployeeId = int.Parse(values[0]),
                        ProjectId = int.Parse(values[1]),
                        DateFrom = DateTime.Parse(values[2]),
                        DateTo = string.IsNullOrWhiteSpace(values[3]) ? DateTime.Today : DateTime.Parse(values[3])
                    };

                    employeeProjects.Add(emp);
                }
            };

            return employeeProjects;
        }

        public static List<EmployeePair> GetEmployeePairs(string csvFilePath)
        {
            var employeeProjects = GetEmployeeProjects(csvFilePath);
            var employeePairs =
                from ep1 in employeeProjects
                join ep2 in employeeProjects on ep1.ProjectId equals ep2.ProjectId
                where ep1.EmployeeId < ep2.EmployeeId
                select new EmployeePair
                {
                    FirstEmployeeId = ep1.EmployeeId,
                    SecondEmployeeId = ep2.EmployeeId,
                    ProjectId = ep1.ProjectId,
                    DaysWorkedTogether = ((ep1.DateTo < ep2.DateTo ? ep1.DateTo : ep2.DateTo) - (ep1.DateFrom > ep2.DateFrom ? ep1.DateFrom : ep2.DateFrom)).Days
                };

            employeePairs =
                from ep in employeePairs
                orderby ep.FirstEmployeeId, ep.SecondEmployeeId, ep.DaysWorkedTogether descending
                select ep;

            return employeePairs.ToList();
        }
    }
}