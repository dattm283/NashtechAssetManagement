using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.Report.Response;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AssetManagementDbContext _dbContext;

        public ReportController(AssetManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<SuccessResponseResult<ViewListPageResult<ViewReportResponse>>>> GetReport(
            [FromQuery] string? sort = "category",
            [FromQuery] string? order = "ASC")
        {
            List<Category> categories = await _dbContext.Categories.ToListAsync();
            IQueryable<ViewReportResponse> viewReportResponses = (from asset in _dbContext.Assets
                                                                  group asset by asset.CategoryId into grAsset
                                                                  select new ViewReportResponse
                                                                  {
                                                                      Category = grAsset.FirstOrDefault().Category.Name,
                                                                      Total = grAsset.Count(),
                                                                      Assigned = grAsset.Count(x => x.State == Domain.Enums.Asset.State.Assigned),
                                                                      Available = grAsset.Count(x => x.State == Domain.Enums.Asset.State.Available),
                                                                      NotAvailable = grAsset.Count(x => x.State == Domain.Enums.Asset.State.NotAvailable),
                                                                      WaitingForRecycling = grAsset.Count(x => x.State == Domain.Enums.Asset.State.WaitingForRecycling),
                                                                      Recycled = grAsset.Count(x => x.State == Domain.Enums.Asset.State.Recycled)
                                                                  }).AsNoTracking();

            List<ViewReportResponse> result = await viewReportResponses.ToListAsync();

            foreach(Category category in categories)
            {
                if (!viewReportResponses.Any(x => x.Category == category.Name))
                {
                    result.Add(new ViewReportResponse
                    {
                        Category = category.Name,
                        Total = 0,
                        Assigned = 0,
                        Available = 0,
                        NotAvailable = 0,
                        WaitingForRecycling = 0,
                        Recycled = 0
                    });
                }
            }

            switch (sort)
            {
                case "category":
                    {
                        result = result.OrderBy(x => x.Category).ToList();
                        break;
                    }
                case "total":
                    {
                        result = result.OrderBy(x => x.Total).ToList();
                        break;
                    }
                case "assigned":
                    {
                        result = result.OrderBy(x => x.Assigned).ToList();
                        break;
                    }
                case "available":
                    {
                        result = result.OrderBy(x => x.Available).ToList();
                        break;
                    }
                case "notAvailable":
                    {
                        result = result.OrderBy(x => x.NotAvailable).ToList();
                        break;
                    }
                case "waitingForRecycling":
                    {
                        result = result.OrderBy(x => x.WaitingForRecycling).ToList();
                        break;
                    }
                case "recycled":
                    {
                        result = result.OrderBy(x => x.Recycled).ToList();
                        break;
                    }
                default:
                    {
                        result = result.OrderBy(x => x.Category).ToList();
                        break;
                    }
            }

            if (order == "DESC")
            {
                result.Reverse();
            }

            return Ok(new SuccessResponseResult<ViewListPageResult<ViewReportResponse>>
            {
                IsSuccessed = true,
                Result = new ViewListPageResult<ViewReportResponse>
                {
                    Data = result,
                    Total = result.Count
                }
            });
        }
    }
}
