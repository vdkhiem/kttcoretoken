using KTT.WebAPI.Models;
using KTT.WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTT.WebAPI.Controllers
{
    [ApiController]
    public class BaseController<T> : Controller
        where T : EntityBase
    {
        protected IEntityMaint<T> entityMaint;
        public BaseController(IEntityMaint<T> entityMaint)
        {
            this.entityMaint = entityMaint;
        }

        [HttpGet]
        public IEnumerable<T> Get()
        {
            return entityMaint.GetAll();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = entityMaint.Get(id);

            if (entity != null)
                return Ok(entity);
            else
                return NotFound();
        }

        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add(T entity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            T createdEntity;
            try
            {
                createdEntity = entityMaint.Add(entity);
                throw new Exception("Khiem unhandle error");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            if (createdEntity?.Id > 0) return CreatedAtAction("Get", new { id = createdEntity.Id }, createdEntity);

            return BadRequest();
        }

        /// <summary>
        /// Update a entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(T entity)
        {
            T updatedEntity = entityMaint.Get(entity.Id);
            if (updatedEntity == null) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                updatedEntity = entityMaint.Update(entity);
                return Ok(updatedEntity);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500);
            }
        }

        /// <summary>
        /// De;ete a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var isDeleted = entityMaint.Delete(id);
            if (isDeleted)
                return Ok();
            else
                return NotFound();
        }
    }
}
