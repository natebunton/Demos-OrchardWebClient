using Demos.Orchard.WebClient.Helpers;
using Demos.Orchard.WebClient.Models;
using Demos.Orchard.WebClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Demos.Orchard.WebClient.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index(string department)
        {
            var viewModel = GetViewModel(department);
            return View(viewModel);
        }

        private EmployeesViewModel GetViewModel(string department)
        {
            var employeesViewModel = new EmployeesViewModel
            {
                Employees = new List<Employee>()
            };

            var cmsODataUrl = ConfigurationManager.AppSettings["CmsODataUrl"];
            var uri = new Uri(cmsODataUrl);

            var oData = new Data.DefaultContainer(uri);

            var employees = (!string.IsNullOrEmpty(department))
                            ? oData.Employees.Where(x => x.Department.Value == department).ToList()
                            : oData.Employees.ToList();

            foreach (var employee in employees)
            {
                var temp = new Employee
                {
                    Name = employee.TitlePart.Title,
                    JobTitle = employee.JobTitle.Value,
                    Department = employee.Department.Value,
                    Bio = employee.Bio.Value

                };

                if (!string.IsNullOrEmpty(employee.Avatar.FirstMediaUrl))
                {
                    var media = CmsHelper.GetMedia(employee.Avatar.FirstMediaUrl);

                    if (media != null)
                    {
                        temp.Avatar = media;
                    }
                }

                employeesViewModel.Employees.Add(temp);
            }

            return employeesViewModel;
        }
    }
}