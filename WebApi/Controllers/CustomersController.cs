using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.DataAccess.Context;
using Otus.Teaching.Concurrency.Import.Handler.Entities;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerContext _context;

        public CustomersController(CustomerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение пользователя по id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);            
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Customer>> Post([FromBody] Customer customer)
        {
            if (customer.Id == 0)
            {
                customer.Id = _context.Customers.OrderByDescending(e => e.Id).FirstOrDefault().Id + 1;
            }

            if (await this.GetById(customer.Id) == Ok())
            {
                return Conflict();
            }

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
