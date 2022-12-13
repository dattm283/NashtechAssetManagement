using AssetManagement.Application.Filters;
using AssetManagement.Contracts.Assignment.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Contracts.ReturnRequest.Response;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FilterCheckIsChangeRole]
    public class ReturnRequestController : ControllerBase
    {
        private readonly AssetManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReturnRequestController(
            AssetManagementDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReturnRequest(int id)
        {
            Assignment? assignment = await _dbContext.Assignments
                .Where(a => a.Id == id && !a.IsDeleted &&
                    a.State == Domain.Enums.Assignment.State.WaitingForReturning)
                .FirstOrDefaultAsync();
            if (assignment != null)
            {
                try
                {
                    assignment.State = Domain.Enums.Assignment.State.Accepted;
                    await _dbContext.SaveChangesAsync();
                    return Ok(_mapper.Map<AssignmentResponse>(assignment));
                }
                catch (Exception e) { return BadRequest(e.Message); }
            }
            return NotFound("Return request does not exist");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ViewListPageResult<ViewListReturnRequestResponse>>> Get(
            [FromQuery] int start,
            [FromQuery] int end,
            [FromQuery] string? searchString = "",
            [FromQuery] string? returnedDateFilter = "",
            [FromQuery] string? stateFilter = "",
            [FromQuery] string? sort = "id",
            [FromQuery] string? order = "ASC",
            [FromQuery] string? createdId = "")
        {
            var list = _dbContext.Assignments
                .Include(x => x.Asset)
                .Include(x => x.AssignedToAppUser)
                .Include(x => x.AssignedByAppUser)
                .Where(x => !x.IsDeleted &&
                    (x.State == Domain.Enums.Assignment.State.WaitingForReturning
                    || x.State == Domain.Enums.Assignment.State.Completed))
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    RequestedBy = x.AssignedToAppUser.UserName,
                    AcceptedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    ReturnedDate = x.ReturnedDate,
                    State = x.State,
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.AssetCode.ToUpper().Contains(searchString.ToUpper()) || x.AssetName.ToUpper().Contains(searchString.ToUpper()) || x.RequestedBy.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!string.IsNullOrEmpty(returnedDateFilter))
            {
                list = list.Where(x => x.ReturnedDate.Date == DateTime.Parse(returnedDateFilter).Date);
            }
            if (!string.IsNullOrEmpty(stateFilter))
            {
                var arrayChar = stateFilter.Split("&");
                var arrNumberChar = new List<int>();
                for (int i = 0; i < arrayChar.Length; i++)
                {
                    var temp = 0;
                    if (int.TryParse(arrayChar[i], out temp))
                    {
                        arrNumberChar.Add(int.Parse(arrayChar[i]));
                    }
                }
                list = list.Where(x => arrNumberChar.Contains((int)x.State));
            }
            switch (sort)
            {
                case "id":
                    {
                        list = list.OrderBy(x => x.Id);
                        break;
                    }
                case "noNumber":
                    {
                        list = list.OrderBy(x => x.NoNumber);
                        break;
                    }
                case "assetCode":
                    {
                        list = list.OrderBy(x => x.AssetCode);
                        break;
                    }
                case "assetName":
                    {
                        list = list.OrderBy(x => x.AssetName);
                        break;
                    }
                case "requestedBy":
                    {
                        list = list.OrderBy(x => x.RequestedBy);
                        break;
                    }
                case "acceptedBy":
                    {
                        list = list.OrderBy(x => x.AcceptedBy);
                        break;
                    }
                case "assignedDate":
                    {
                        list = list.OrderBy(x => x.AssignedDate);
                        break;
                    }
                case "returnedDate":
                    {
                        list = list.OrderBy(x => x.ReturnedDate);
                        break;
                    }
                case "state":
                    {
                        list = list.OrderBy(x => x.State);
                        break;
                    }
                default:
                    {
                        list = list.OrderBy(x => x.Id);
                        break;
                    }
            }

            if (order == "DESC")
            {
                list = list.Reverse();
            }

            if (!string.IsNullOrEmpty(createdId))
            {
                ViewListReturnRequestResponse recentlyCreatedItem = list.Where(item => item.Id == int.Parse(createdId)).AsNoTracking().FirstOrDefault();
                list = list.Where(item => item.Id != int.Parse(createdId));

                var sortedResultWithCreatedIdParam = StaticFunctions<ViewListReturnRequestResponse>.Paging(list, start, end - 1);

                sortedResultWithCreatedIdParam.Insert(0, recentlyCreatedItem);

                return Ok(new ViewListPageResult<ViewListReturnRequestResponse> { Data = sortedResultWithCreatedIdParam, Total = list.Count() + 1 });
            }

            var sortedResult = StaticFunctions<ViewListReturnRequestResponse>.Paging(list, start, end);

            return Ok(new ViewListPageResult<ViewListReturnRequestResponse> { Data = sortedResult, Total = list.Count() });
        }
    }
}
