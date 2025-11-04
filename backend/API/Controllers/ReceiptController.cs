using Microsoft.AspNetCore.Mvc;
using ReceiptTracker.Application.Constants;
using ReceiptTracker.Application.DTOs.Receipts;
using ReceiptTracker.Application.Services.Receipts;
using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.API.Controllers;

public class ReceiptController : BaseApiController
{
    private readonly IReceiptService _receiptService;

    public ReceiptController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }

    [HttpGet("{receiptId}")]
    public async Task<ActionResult<ReceiptViewDto>> GetReceiptById(int receiptId)
    {
        try
        {
            int userId = GetCurrentUserId();

            var receipt = await _receiptService.FindByIdAsync(receiptId, userId);
            return Ok(receipt);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }

    [HttpPost("upload")]
    public async Task<ActionResult<ReceiptReadDto>> UploadReceipt([FromForm] ReceiptUploadDto uploadDto)
    {
        try
        {
            int userId = GetCurrentUserId();

            ReceiptReadDto receiptDto = await _receiptService.UploadReceiptAsync(uploadDto, userId);
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
            int userId = GetCurrentUserId();

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
            return StatusCode(500, ErrorMessages.FailedToProcessReceipt);
        }
    }

    [HttpGet("{receiptId}/confirm")]
    public async Task<ActionResult<ReceiptPreviewDto>> GetConfirmPreview(int receiptId)
    {
        int userId = GetCurrentUserId();

        var receipt = await _receiptService.GetReceiptPreviewAsync(receiptId, userId);
        if (receipt == null)
            return NotFound();

        return Ok(receipt);
    }

    [HttpPost("{receiptId}/confirm")]
    public async Task<ActionResult<ReceiptReadDto>> ConfirmReceipt(int receiptId, [FromBody] ReceiptConfirmDto dto)
    {
        int userId = GetCurrentUserId();

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

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReceiptDashboardDto>>> GetAllReceiptsForUser()
    {
        try
        {
            int userId = GetCurrentUserId();

            var receipts = await _receiptService.GetAllReceiptsForUserAsync(userId);
            return Ok(receipts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("{receiptId}")]
    public async Task<IActionResult> DeleteReceipt(int receiptId)
    {
        try
        {
            int userId = GetCurrentUserId();

            var success = await _receiptService.DeleteAsync(receiptId, userId);
            if (!success)
                return NotFound(ErrorMessages.ReceiptNotFound(receiptId));

            return Ok(Strings.ReceiptDeleteSuccess);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

}
