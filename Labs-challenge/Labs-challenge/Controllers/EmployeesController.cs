using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Labs_challenge.Models;
using System.Threading.Tasks;

namespace Labs_challenge.Controllers
{
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
      
        private Labs_challengeContext db = new Labs_challengeContext();


        // GET: api/Employees/5
        /// <summary>
        /// Get info of just one employee.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        /// <summary>
        /// Get employees with pagination.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        // GET: api/Employees/?page_size=10&page=1
        [ResponseType(typeof(List<Employee>))]
        [HttpGet]
        public IHttpActionResult GetEmployeesWithPagination([FromUri] int page_Size = 1, [FromUri] int page = 0)
        {

            if (page == 0)
            {
                return Ok(db.Employees.ToList());
            }
            
            var skipAmount = page_Size * (page - 1);

            var pagingEmployees = db.Employees.
                AsQueryable()
                .OrderBy(c => c.Id)
                .Skip(skipAmount)
                .Take(page_Size)
                .ToList();

            if (pagingEmployees.Count == 0)
            {
                return NotFound();
            }

            return Ok(pagingEmployees);
        }

        /// <summary>
        /// Update an employee info.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (!EmployeeExists(id))
            {
                return NotFound();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

                return InternalServerError();

            }

            return Ok();
        }

        /// <summary>
        /// Add an employee.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Delete an employee.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.Id == id) > 0;
        }
    }
}