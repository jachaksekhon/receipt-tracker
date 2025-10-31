using Microsoft.AspNetCore.Mvc;
using ReceiptTracker.Application.DTOs.Receipts;
using ReceiptTracker.Application.Services.Receipts;
using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceiptController : Controller
{
    private readonly IReceiptService _receiptService;

    public ReceiptController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<ReceiptReadDto>> UploadReceipt([FromForm] ReceiptUploadDto uploadDto)
    {
        try
        {
            // will update to take current user id
            int id = 1;
            ReceiptReadDto receiptDto = await _receiptService.UploadReceiptAsync(uploadDto, id);
            return Ok(receiptDto);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occured" });
        }

    }
}
