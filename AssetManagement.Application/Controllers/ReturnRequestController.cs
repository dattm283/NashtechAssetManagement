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
using System.Security.Claims;

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

        [HttpPost("{id}")]
        public async Task<IActionResult> CreateReturnRequest(int id)
        {
            string userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            AppUser currentUser = _dbContext.AppUsers.FirstOrDefault(x => x.UserName == userName);
            if (currentUser == null)
            {
                return BadRequest(new ErrorResponseResult<string>("Invalid User"));
            }
            Assignment assignment = _dbContext.Assignments.Where(x => x.Id == id).FirstOrDefault();
            if (assignment == null)
            {
                return BadRequest(new ErrorResponseResult<string>("Invalid Assignment"));
            }
            ReturnRequest returnRequest = new ReturnRequest
            {
                AssignmentId = assignment.Id,
                AssignedBy = currentUser.Id,
                AcceptedBy = null,
                ReturnedDate = null,
                AssignedDate = assignment.AssignedDate,
                State = Domain.Enums.ReturnRequest.State.WaitingForReturning
            };

            assignment.State = Domain.Enums.Assignment.State.WaitingForReturning;
            await _dbContext.ReturnRequests.AddAsync(returnRequest);
            await _dbContext.SaveChangesAsync();

            return Ok(new SuccessResponseResult<string>("Create ReturningRequest successfully"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CancelReturnRequest(int id)
        {
            ReturnRequest? returnRequest = await _dbContext.ReturnRequests
                .Include(a => a.Assignment)
                .Where(a => a.Id == id && !a.IsDeleted &&
                    a.State == Domain.Enums.ReturnRequest.State.WaitingForReturning)
                .FirstOrDefaultAsync();
            if (returnRequest != null)
            {
                try
                {
                    returnRequest.IsDeleted = true;
                    returnRequest.Assignment.State = Domain.Enums.Assignment.State.Accepted;
                    await _dbContext.SaveChangesAsync();
                    return Ok(_mapper.Map<CancelReturnRequestResponse>(returnRequest));
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
            //var tempList = _dbContext.ReturnRequests.ToList();
            var list = _dbContext.ReturnRequests
                .Include(x => x.AssignedByUser)
                .Include(x => x.AcceptedByUser)
                .Include(x => x.Assignment)
                    .ThenInclude(a => a.Asset)
                .Where(x => !x.IsDeleted && 
                    (x.ReturnedDate == null || x.ReturnedDate.Value.Date <= DateTime.Now.Date)
                )
                .Select(x => new ViewListReturnRequestResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Assignment.Asset.AssetCode,
                    AssetName = x.Assignment.Asset.Name,
                    RequestedBy = x.AssignedByUser.UserName,
                    AcceptedBy = x.AcceptedByUser.UserName,
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
                list = list.Where(x => x.ReturnedDate.Value.Date == DateTime.Parse(returnedDateFilter).Date);
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

        [HttpPut("complete/{id}")]
        [Authorize]
        public async Task<IActionResult> Complete(int id)
        {
            var returnRequest = await _dbContext.ReturnRequests.FindAsync(id);

            if (returnRequest == null)
            {
                return NotFound(new ErrorResponseResult<string>("Return request does not exists!"));
            }

            var assignment = await _dbContext.Assignments.FindAsync(returnRequest.AssignmentId);

            if (assignment == null)
            {
                return NotFound(new ErrorResponseResult<string>("Assigment does not exists!"));
            }

            var asset = await _dbContext.Assets.FindAsync(assignment.AssetId);

            if (asset == null)
            {
                return NotFound(new ErrorResponseResult<string>("Asset does not exists!"));
            }

            var currentUserLogin = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName.Equals(User.Identity.Name));
            
            returnRequest.State = Domain.Enums.ReturnRequest.State.Completed;
            returnRequest.ReturnedDate = DateTime.Now;
            assignment.ReturnedDate = DateTime.Now;
            returnRequest.AcceptedBy = currentUserLogin.Id;

            if (assignment.State == Domain.Enums.Assignment.State.WaitingForReturning)
            {
                assignment.State = Domain.Enums.Assignment.State.Returned;
            }
            else
            {
                return BadRequest(new ErrorResponseResult<string>("Assignment's state of this return request is invalid"));
            }

            if (asset.State == Domain.Enums.Asset.State.Assigned)
            {
                asset.State = Domain.Enums.Asset.State.Available;
            }
            else
            {
                return BadRequest(new ErrorResponseResult<string>("Asset's state of this assignment is invalid"));
            }

            var result = await _dbContext.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(new SuccessResponseResult<string>("Return request successfully!"));
            }

            return BadRequest("Return asset unsuccessfully!");
        }
    }
}
