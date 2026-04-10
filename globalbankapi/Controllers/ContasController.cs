using GlobalBankApi.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlobalBankApi.Data;
using System.Security.Cryptography;


namespace GlobalBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContasController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> CriarConta([FromBody] ContaBancaria conta)
        {
            if (conta.Saldo < 0)
            {
                return BadRequest("O saldo inicial não pode ser negativo.");
            }
            _context.Contas.Add(conta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterConta), new { id = conta.Id }, conta);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterConta(int id)
        {
            var conta = await _context.Contas.FindAsync(id);
            if (conta == null)
            {
                return NotFound();
            }
            return Ok(conta);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var patrimonioTotal = await _context.Contas
                .SumAsync(c => c.Saldo);

            var totalTransacoes = await _context.Transacoes
                .CountAsync();

            var resultado = new
            {
                PatrimonioTotal = patrimonioTotal,
                TotalTransacoes = totalTransacoes
            };

            return Ok(resultado);
        }

    }
}
