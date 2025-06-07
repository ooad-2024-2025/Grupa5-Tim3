using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public static class QrCodeHelper
{
    public static byte[] GenerateQrCode(string content)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20); 
    }
}
