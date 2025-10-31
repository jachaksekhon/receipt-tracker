using Microsoft.AspNetCore.Mvc;
using ReceiptTracker.Application.Services.Receipts;
using ReceiptTracker.DTOs.Receipts;

namespace ReceiptTracker.API.Controllers.Receipts;

[ApiController]
[Route("api/[controller]")]
public class ReceiptController : Controller
{
    private readonly IReceiptService _receiptService;

    public ReceiptController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }

    //[HttpPost("upload")]
    //public async Task<IActionResult> UploadReceipt([FromForm] ReceiptUploadDto uploadDto)
    //{
        
    //}
}
