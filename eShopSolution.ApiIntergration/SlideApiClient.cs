using eShopSolution.ViewModels.Utilities.Slides;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eShopSolution.ApiIntergration
{
    public class SlideApiClient : BaseApiClient, ISlideApiClient
    {

        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly IConfiguration _configuration;

        public SlideApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
            //_httpClientFactory = httpClientFactory;
            //_httpContextAccessor = httpContextAccessor;
            //_configuration = configuration;
        }

        public async Task<List<SlideViewModel>> GetAll()
        {
            
            return await GetListAsync<SlideViewModel>("/api/Slides");
        }
    }
}