using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer;
using System.Data.Entity;


namespace WebAPI2_Test.Controllers
{
    public class EmployeesController : ApiController
    {

        // GET api/values
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBContext context = new EmployeeDBContext())
            {
                List<Employee> employees = context.Employees.ToList();
                return employees;
            }

        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBContext context = new EmployeeDBContext())
            {
                Employee employee = context.Employees.First(x => x.ID == id);
                if (employee == null) {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with " + id + " not found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, employee);
            }

        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBContext context = new EmployeeDBContext())
                {
                    context.Employees.Add(employee);
                    context.SaveChanges();
                    HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }

            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // PUT api/values/5
        public HttpResponseMessage Put(int id, [FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBContext context = new EmployeeDBContext())
                {
                    Employee emp = context.Employees.Where(x => x.ID == id).FirstOrDefault();
                    if (emp == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with " + id + " not found");
                    }
                    emp.FirstName = employee.FirstName;
                    emp.LastName = employee.LastName;
                    emp.Gender = employee.Gender;
                    emp.Salary = employee.Salary;

                    context.SaveChanges();

                    HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.OK, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + id.ToString());
                    return message;
                }
            }
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(int id)
        {
            try {
                using (EmployeeDBContext context = new EmployeeDBContext())
                {
                    Employee employee = context.Employees.FirstOrDefault(x => x.ID == id);
                    if(employee == null)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with " + id + " not found");
                    }
                    context.Employees.Remove(employee);
                    context.SaveChanges();
                    HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.OK, "Employee with id=" + id + " is deleted");
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
            

        }
    }
}
