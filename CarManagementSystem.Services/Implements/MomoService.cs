using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Azure.Core;
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Dtos.Momo;
using CarManagementSystem.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CarManagementSystem.Services.Implements
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionDTO> _options;
        private static readonly HttpClient _client = new();

        public MomoService(IOptions<MomoOptionDTO> options)
        {
            _options = options;
        }

        public async Task<MomoResponseDTO> CreatePaymentAsync(Order order)
        {
            string requestIds = DateTime.UtcNow.Ticks.ToString();
            decimal amounts = 100000;
            string orderIds = Guid.NewGuid().ToString();

            var rawData =
                $"accessKey={_options.Value.AccessKey}" +
                $"&amount={amounts}" +
                $"&extraData=" +
                $"&ipnUrl={_options.Value.IpnUrl}" +
                $"&orderId={orderIds}" +
                $"&orderInfo=pay with MoMo" +
                $"&partnerCode={_options.Value.PartnerCode}" +
                $"&redirectUrl={_options.Value.RedirectUrl}" +
                $"&requestId={requestIds}" +
                $"&requestType={_options.Value.RequestType}";

            var signatures = GetSignature(rawData, _options.Value.SecretKey);

            var requestData = new
            {
                orderInfo = "pay with MoMo",
                partnerCode = _options.Value.PartnerCode,
                ipnUrl = _options.Value.IpnUrl,
                redirectUrl = _options.Value.RedirectUrl,
                requestType = _options.Value.RequestType,
                orderId = orderIds,
                amount = amounts,
                accessKey = _options.Value.AccessKey,
                requestId = requestIds,
                extraData = "",
                signature = signatures,
            };

            StringContent httpContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var quickPayResponse = await _client.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", httpContent);

            var contents = await quickPayResponse.Content.ReadAsStringAsync();

            var momoResponse = JsonSerializer.Deserialize<MomoResponseDTO>(contents);

            return momoResponse;
        }

        private static String GetSignature(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public MomoExecuteDTO PaymentExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
