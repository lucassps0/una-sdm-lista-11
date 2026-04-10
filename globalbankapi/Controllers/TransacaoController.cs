using System;
using Microsoft.AspNetCore.Mvc;
using GlobalBankApi.Data;
using GlobalBankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransacaoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> RegistrarTransacao([FromBody] Transacao transacao)
        {
            var conta = await _context.Contas.FindAsync(transacao.ContaId);
            if (conta == null)
            {
                return NotFound("Conta não encontrada.");
            }
            if (transacao.Tipo == "Saque" && transacao.Valor > conta.Saldo)
            {
                return Conflict("Saldo Insuficiente.");
            }

            if (transacao.Tipo == "Saque")
            {
                conta.Saldo -= transacao.Valor;
            }
            else if (transacao.Tipo == "Depósito")
            {
                conta.Saldo += transacao.Valor;
            }
            else
            {
                return BadRequest("Tipo de transação inválido.");
            }
            if (transacao.Valor > 10000)
            {
                Console.WriteLine($"🚩 ALERTA DE SEGURANÇA: Transação de alto valor detectada para a conta {conta.NumeroConta}!");
            }
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return Ok("Transação registrada com sucesso.");

        }
        [HttpGet("extrato/{contaId}")]
        public async Task<IActionResult> ObterExtrato(int contaId)
        {
            var conta = await _context.Contas.FindAsync(contaId);

            if (conta == null)
            {
                return NotFound("Conta não encontrada.");
            }

            var transacoes = await _context.Transacoes
                .Where(t => t.ContaId == contaId)
                .ToListAsync();

            return Ok(transacoes);
        }
    }
}