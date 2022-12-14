using AssetManagement.Application.Filters;
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
using System.Security.Claims;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FilterCheckIsChangeRole]
    public class ReportController : ControllerBase
    {
        private readonly AssetManagementDbContext _dbContext;

        public ReportController(AssetManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ViewListPageResult<ViewReportResponse>>> GetReport(
            [FromQuery] string? sort = "category",
            [FromQuery] string? order = "ASC")
        {
            string userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            AppUser currentUser = await _dbContext.AppUsers.FirstOrDefaultAsync(x => x.UserName == userName);
            List<Category> categories = await _dbContext.Categories.ToListAsync();

            IQueryable<ViewReportResponse> viewReportResponses = _dbContext.Assets
                .Where(x => !x.IsDeleted && x.Location == currentUser.Location)
                .GroupBy(x => x.CategoryId)
                .Select(grAsset => new ViewReportResponse
                {
                    ID = grAsset.FirstOrDefault().Category.Name,
                    Category = grAsset.FirstOrDefault().Category.Name,
                    Total = grAsset.Count(),
                    Assigned = grAsset.Count(x => x.State == Domain.Enums.Asset.State.Assigned),
                    Available = grAsset.Count(x => x.State == Domain.Enums.Asset.State.Available),
                    NotAvailable = grAsset.Count(x => x.State == Domain.Enums.Asset.State.NotAvailable),
                    WaitingForRecycling = grAsset.Count(x => x.State == Domain.Enums.Asset.State.WaitingForRecycling),
                    Recycled = grAsset.Count(x => x.State == Domain.Enums.Asset.State.Recycled)
                });

            List<ViewReportResponse> result = await viewReportResponses.ToListAsync();

            foreach(Category category in categories)
            {
                if (!viewReportResponses.Any(x => x.Category == category.Name))
                {
                    result.Add(new ViewReportResponse
                    {
                        ID = category.Name,
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

            return Ok(new ViewListPageResult<ViewReportResponse>
                {
                    Data = result,
                    Total = result.Count
                });
        }
    }
}
