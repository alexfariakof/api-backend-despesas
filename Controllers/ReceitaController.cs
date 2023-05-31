﻿using despesas_backend_api_net_core.Business.Generic;
using despesas_backend_api_net_core.Domain.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace despesas_backend_api_net_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitaController : Controller
    {
        private IBusiness<ReceitaVM> _receitaBusiness;

        public ReceitaController(IBusiness<ReceitaVM> receitaBusiness)
        {
            _receitaBusiness = receitaBusiness;
        }

        [HttpGet]
        //[Authorize("Bearer")]
        public IActionResult Get()
        {
            return Ok(_receitaBusiness.FindAll());
        }

        [HttpGet("GetById/{id}")]
        //[Authorize("Bearer")]
        public IActionResult GetById([FromRoute]int id)
        {
            try
            {
                var _receita = _receitaBusiness.FindById(id);

                if (_receita == null)
                    return Ok(new { message = "Nenhuma receita foi encontrada." });

                return new ObjectResult(new { message = true, receita = _receita });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível realizar a consulta da receita." });
            }
        }

        [HttpPost]
        //[Authorize("Bearer")]
        public IActionResult Post([FromBody] ReceitaVM receita)
        {
            if (receita == null)
                return BadRequest();

            try
            {
                return new ObjectResult(new { message = true, receita = _receitaBusiness.Create(receita) });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível realizar o cadastro" });
            }            
        }

        [HttpPut]
        //[Authorize("Bearer")]
        public IActionResult Put([FromBody] ReceitaVM receita)
        {
            if (receita == null)
                return BadRequest();

            var updateReceita = _receitaBusiness.Update(receita);

            if (updateReceita == null)
                return BadRequest(new { message = "Não foi possível atualizar o cadastro da receita." });

            return new ObjectResult(new { message = true, receita = updateReceita });
        }

        [HttpDelete("{id}")]
        //[Authorize("Bearer")]
        public IActionResult Delete(int id)
        {
            _receitaBusiness.Delete(id);
            return NoContent();
        }
    }
}
