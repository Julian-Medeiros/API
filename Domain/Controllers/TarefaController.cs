using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Context;
using API.Domain.Enums;
using API.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Domain.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaContext _context;

        public TarefaController(TarefaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Tarefa tarefa)
        {
            _context.Add(tarefa);
            _context.SaveChanges();
            return Ok(tarefa);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            return tarefaBanco == null ? NotFound("Tarefa inexistente.") : Ok(tarefaBanco);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tarefaBanco = new List<Tarefa>(_context.Tarefas.ToList());

            return tarefaBanco.Count == 0 ? NotFound("A lista de Tarefas está vazia") : Ok(tarefaBanco);
        }

        [HttpGet("GetByTitulo")]
        public IActionResult GetByTitulo(string titulo)
        {
            var tarefaBanco = _context.Tarefas.Where(tarefa => tarefa.Titulo.Contains(titulo)).ToList();

            return tarefaBanco.Count == 0 ? NotFound("Tarefas inexistentes.") : Ok(tarefaBanco);
        }

        [HttpGet("GetByData")]
        public IActionResult GetByData(DateTime data)
        {
            var tarefaBanco = _context.Tarefas.Where(tarefa => tarefa.Data.Date == data.Date).ToList();

            if (tarefaBanco.Count == 0)
                return NotFound($"Tarefa com a data: {data:dd/MM/yyyy}, inexistente.");

            return Ok(tarefaBanco);
        }

        [HttpGet("GetByStatus")]
        public IActionResult GetByStatus(TarefaEnum status)
        {
            var tarefaBanco = _context.Tarefas.Where(tarefa => tarefa.Status == status).ToList();

            return tarefaBanco.Count == 0 ? NotFound($"Tarefas com o status: {status}, inexistentes.") : Ok(tarefaBanco);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound("Contato inexistente.");

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);
            var tarefaTitulo = tarefaBanco.Titulo;

            if (tarefaBanco == null)
                return NotFound("Contato inexistente.");

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();
            return Ok($"Tarefa: {tarefaTitulo}, deletada com sucesso");
        }
    }
}