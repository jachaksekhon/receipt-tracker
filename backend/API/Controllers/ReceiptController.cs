using Microsoft.AspNetCore.Mvc;
using ReceiptTracker.Application.Constants;
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
            // **** TODO: PERSIST USER ID FROM CURRENT INSTANCE ****
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
            return StatusCode(500, new { message = ErrorMessages.UnexpectedError });
        }

    }

    [HttpPost("{receiptId}/process")]
    public async Task<ActionResult<ReceiptReadDto>> ProcessReceipt(int receiptId)
    {
        try
        {
            int userId = 1; 

            var processedReceipt = await _receiptService.ProcessReceiptAsync(receiptId, userId);
            return Ok(processedReceipt);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred while processing the receipt." });
        }
    }

    [HttpGet("{receiptId}/confirm")]
    public async Task<ActionResult<ReceiptPreviewDto>> GetConfirmPreview(int receiptId)
    {
        // **** TODO: PERSIST USER ID FROM CURRENT INSTANCE ****
        var userId = 1;
        var receipt = await _receiptService.GetReceiptPreviewAsync(receiptId, userId);
        if (receipt == null)
            return NotFound();

        return Ok(receipt);
    }

    [HttpPost("{receiptId}/confirm")]
    public async Task<ActionResult<ReceiptReadDto>> ConfirmReceipt(int receiptId, [FromBody] ReceiptConfirmDto dto)
    {
        // **** TODO: PERSIST USER ID FROM CURRENT INSTANCE ****
        var userId = 1;

        try
        {
            var result = await _receiptService.ConfirmReceiptAsync(receiptId, userId, dto);
            return Ok(result);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ErrorMessages.ErrorConfirmingReceipt });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IReadOnlyList<ReceiptReadDto>>> GetAllReceiptsForUser(int userId)
    {
        try
        {
            var receipts = await _receiptService.GetAllReceiptsForUserAsync(userId);
            return Ok(receipts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

}
