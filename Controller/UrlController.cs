using EncurtadorUrl.Services;
using EncurtadorUrl.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EncurtadorUrl.Controller
{
    [ApiController]
    [Route("")]
    public class UrlController : ControllerBase
    {
        private readonly UrlService _service;

        public UrlController(UrlService service)
        {
            _service = service;
        }

        [HttpPost("encurtar")]
        public async Task<IActionResult> Encurtar([FromBody] UrlOriginalDto request)
        {
            var novaUrl = await _service.CriarEncurtamento(request.UrlOriginal);
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return Ok(new { urlCurta = $"{baseUrl}/{novaUrl.Codigo}" });
        }

        [HttpPost("encurtar/linkPersonalizado")]
        public async Task<IActionResult> EncurtarComLinkPersonalizado([FromBody] LinkPersonalizadoDto request)
        {
            try
            {
                var novaUrl = await _service.CriarEncurtamentoComLinkPersonalizado(request.UrlOriginal, request.LinkPersonalizado);
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                return Ok(new { urlCurta = $"{baseUrl}/{novaUrl.Codigo}" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> Redirecionar(string codigo)
        {
            var registro = await _service.BuscarPorCodigo(codigo);
            if (registro == null) return NotFound();
        
            return Redirect(registro.UrlOriginal);
        }
    }
}