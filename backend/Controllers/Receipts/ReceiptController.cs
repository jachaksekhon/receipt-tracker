using Microsoft.AspNetCore.Mvc;
using ReceiptTracker.DTOs.Receipts;
using ReceiptTracker.Services.Receipts;

namespace ReceiptTracker.Controllers.Receipts;

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
