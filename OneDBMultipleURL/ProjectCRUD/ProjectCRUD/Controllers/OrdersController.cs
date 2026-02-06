using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCRUD.Data;
using ProjectCRUD.Models;
using System.Net;

namespace ProjectCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public OrdersController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet("report-link")]
        public IActionResult GetReportLink()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return Unauthorized("No token found");

            string url = $"https://localhost:7126/api/Reports/orders?token={WebUtility.UrlEncode(token)}";
            return Ok(new { Url = url });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Orders.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Order updatedOrder)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
                return NotFound();

            order.CustomerName = updatedOrder.CustomerName;
            order.OrderDate = updatedOrder.OrderDate;
            order.TotalAmount = updatedOrder.TotalAmount;

            _context.SaveChanges();
            return Ok(order);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
