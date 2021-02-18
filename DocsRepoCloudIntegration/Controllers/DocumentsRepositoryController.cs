using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsRepositoryController : ControllerBase
    {
        private readonly ILogger<DocumentsRepositoryController> _logger;
        private readonly IStorageDriver _storageDriver;
        private readonly IOptionsMonitor<StorageOptions> _options;

        public DocumentsRepositoryController(ILogger<DocumentsRepositoryController> logger, IStorageDriver storageDriver, IOptionsMonitor<StorageOptions> options)
        {
            _logger = logger;
            _storageDriver = storageDriver ?? throw new ArgumentNullException(nameof(storageDriver));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var result = await _storageDriver.ListAsync("ZadERP");

            if (result is null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOfertaFiles(int idOferta)
        {
            string ofertaFolder = string.Format(_options.CurrentValue.PathTemplate, Entities.Ofertas.ToString(), idOferta.ToString());
            try
            {
                await _storageDriver.CreatFolderIfNotExists(ofertaFolder);
                return Ok();
            }
            catch (Exception exIO)
            {
                _logger.LogError(exIO, $"Error Creando Carpetas {ofertaFolder}");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePedidosFiles(int idPedido)
        {
            string pedidoFolder = string.Format(_options.CurrentValue.PathTemplate, Entities.Pedidos.ToString(), idPedido.ToString());
            try
            {
                await _storageDriver.CreatFolderIfNotExists(pedidoFolder);
                return Ok();
            }
            catch (Exception exIO)
            {
                _logger.LogError(exIO, $"Error Creando Carpetas {pedidoFolder}");
                throw;
            }
        }
    }

    public enum Entities
    {
        Ofertas,
        Pedidos,
        PRL,
        Productos
    }
}
