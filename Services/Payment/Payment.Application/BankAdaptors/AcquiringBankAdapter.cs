using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Payment.Application.Commands;
using Payment.Application.Contracts;
using Payment.Domain.Configuration;
using Payment.Domain.DTO.Requests;
using Payment.Domain.DTO.Response;
using Payment.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.BankAdaptors
{
    public class AcquiringBankAdapter : IAcquiringBankAdapter
    {
        private readonly IConfigurationOptions _configurationOptions;      
        private readonly ILogger<AcquiringBankAdapter> _logger;

        public AcquiringBankAdapter(ILogger<AcquiringBankAdapter> logger, IConfigurationOptions configurationOptions)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configurationOptions = configurationOptions ?? throw new ArgumentNullException(nameof(configurationOptions));
        }

        public async Task<PaymentResponse> SendRequestAsync(PaymentCommand request)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(request);
                string queryString = "";

                HttpClient client = new HttpClient { BaseAddress = new Uri(_configurationOptions.Url) };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(queryString, content);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var result = await response.Content.ReadAsStringAsync();

                    client.Dispose();

                    return JsonConvert.DeserializeObject<PaymentResponse>(result);
                }
                else
                {
                    //error handling
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return new PaymentResponse() { PaymentStatus = BankPaymentStatus.Rejected};
        }
    }
}
