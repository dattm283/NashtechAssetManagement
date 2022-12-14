// using AssetManagement.Contracts.Assignment.Request;
using AssetManagement.Contracts.Assignment.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Domain.Enums.Assignment;
using AssetManagement.Contracts.Assignment.Request;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System;
using AssetManagement.Application.Filters;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FilterCheckIsChangeRole]
    public class AssignmentsController : ControllerBase
    {
        private readonly AssetManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public AssignmentsController(
            AssetManagementDbContext dbContext,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("assets/{assetCodeId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAssignmentsByAssetCodeId(int assetCodeId)
        {
            var result = _dbContext.Assignments.Where(x => x.AssetId == assetCodeId && !x.IsDeleted).ToList();
            var assignmentResponse = _mapper.Map<List<AssignmentResponse>>(result);

            foreach (var item in assignmentResponse)
            {
                item.AssignedTo = _dbContext.Users.Find(new Guid(item.AssignedTo)).UserName;
                item.AssignedBy = _dbContext.Users.Find(new Guid(item.AssignedBy)).UserName;
            }

            return Ok(assignmentResponse);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentDetail(int id)
        {
            var assignment = await _dbContext.Assignments
                .Include(x => x.Asset)
                .Include(x => x.AssignedToAppUser)
                .Include(x => x.AssignedByAppUser)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (assignment != null)
            {
                return Ok(_mapper.Map<AssignmentDetailResponse>(assignment));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAssignment(int id, UpdateAssignmentRequest request)
        {
            Assignment updatingAssignment = await _dbContext.Assignments
                .Include(a => a.Asset)
                .Include(a => a.AssignedToAppUser)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            try
            {
                if (updatingAssignment != null && updatingAssignment.State == State.WaitingForAcceptance)
                {
                    if (AssignmentChanged(updatingAssignment, request))
                    {
                        AppUser assignedToUser = await _dbContext.AppUsers
                            .Where(u => u.StaffCode.Equals(request.AssignToAppUserStaffCode))
                            .FirstOrDefaultAsync();

                        Asset asset = await _dbContext.Assets
                            .Where(a => a.AssetCode.Equals(request.AssetCode))
                            .FirstOrDefaultAsync();

                        if (assignedToUser != null && asset != null)
                        {
                            updatingAssignment.AssignedTo = assignedToUser.Id;
                            updatingAssignment.AssetId = asset.Id;
                            updatingAssignment.AssignedDate = request.AssignedDate;
                            updatingAssignment.Note = request.Note.Trim();
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("User or asset is invalid");
                        }
                    }
                }
                else
                {
                    throw new Exception($"Cannot find an assignment with id: {id}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseResult<string>(ex.Message));
            }

            return Ok(_mapper.Map<UpdateAssignmentResponse>(updatingAssignment));
        }

        [HttpPut("{id}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptAssignment(int id) {
            Assignment updatingAssignment = await _dbContext.Assignments
                .Include(a => a.Asset)
                .Where(a => a.Id == id && a.IsDeleted == false && a.State == State.WaitingForAcceptance)
                .FirstOrDefaultAsync();

            try
            {
                if (updatingAssignment != null )
                {
                    Asset asset = await _dbContext.Assets
                        .Where(a => a.Id == updatingAssignment.AssetId)
                        .FirstOrDefaultAsync();

                    if (asset != null)
                    {
                        switch (asset.State)
                        { 
                            case AssetManagement.Domain.Enums.Asset.State.Available: 
                            {
                                updatingAssignment.State = State.Accepted;
                                asset.State = AssetManagement.Domain.Enums.Asset.State.Assigned;
                                await _dbContext.SaveChangesAsync();
                                break;
                            } 
                            case AssetManagement.Domain.Enums.Asset.State.NotAvailable:
                            {
                                throw new Exception("This asset is not available");
                                break;
                            }
                            case AssetManagement.Domain.Enums.Asset.State.WaitingForRecycling:
                            {
                                throw new Exception("This asset is waiting for recycling");
                                break;
                            }
                            case AssetManagement.Domain.Enums.Asset.State.Recycled:
                            {
                                throw new Exception("This asset is recycled");
                                break;
                            }
                            case AssetManagement.Domain.Enums.Asset.State.Assigned: 
                            {
                                throw new Exception("This asset is already assigned to another assignment");
                                break;
                            }
                            default:
                            {
                                throw new Exception("Asset of this assignment is invalid");
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Asset of this assignment is invalid");
                    }
                }
                else
                {
                    throw new Exception($"Cannot find a Waiting For Acceptance assignment with id: {id}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseResult<string>(ex.Message));
            }

            return Ok(_mapper.Map<AcceptAssignmentResponse>(updatingAssignment));
        }

        [HttpPut("{id}/decline")]
        [Authorize]
        public async Task<IActionResult> DeclineAssignment(int id) {
            Assignment updatingAssignment = await _dbContext.Assignments
                .Include(a => a.Asset)
                .Where(a => a.Id == id && a.IsDeleted == false && a.State == State.WaitingForAcceptance)
                .FirstOrDefaultAsync();

            try
            {
                if (updatingAssignment != null )
                {
                    Asset asset = await _dbContext.Assets
                        .Where(a => a.Id == updatingAssignment.AssetId)
                        .FirstOrDefaultAsync();

                    if (asset != null)
                    {
                        switch (asset.State)
                        { 
                            case AssetManagement.Domain.Enums.Asset.State.Available: 
                            {
                                updatingAssignment.State = State.Declined;
                                await _dbContext.SaveChangesAsync();
                                break;
                            } 
                            case AssetManagement.Domain.Enums.Asset.State.NotAvailable:
                            {
                                throw new Exception("This asset is not available");
                                break;
                            }
                            case AssetManagement.Domain.Enums.Asset.State.WaitingForRecycling:
                            {
                                throw new Exception("This asset is waiting for recycling");
                                break;
                            }
                            case AssetManagement.Domain.Enums.Asset.State.Recycled:
                            {
                                throw new Exception("This asset is recycled");
                                break;
                            }
                            case AssetManagement.Domain.Enums.Asset.State.Assigned: 
                            {
                                throw new Exception("This asset is already assigned to another assignment");
                                break;
                            }
                            default:
                            {
                                throw new Exception("Asset of this assignment is invalid");
                                break;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Asset of this assignment is invalid");
                    }
                }
                else
                {
                    throw new Exception($"Cannot find a Waiting For Acceptance assignment with id: {id}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseResult<string>(ex.Message));
            }

            return Ok(_mapper.Map<DeclineAssignmentResponse>(updatingAssignment));
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ViewListPageResult<ViewListAssignmentResponse>>> Get(
            [FromQuery] int start,
            [FromQuery] int end,
            [FromQuery] string? searchString = "",
            [FromQuery] string? assignedDateFilter = "",
            [FromQuery] string? stateFilter = "",
            [FromQuery] string? sort = "noNumber",
            [FromQuery] string? order = "ASC",
            [FromQuery] string? createdId = "")
        {
            var list = _dbContext.Assignments
                .Where(x => !x.IsDeleted && x.State != State.Returned)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    NoNumber = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.AssetCode.ToUpper().Contains(searchString.ToUpper()) || x.AssetName.ToUpper().Contains(searchString.ToUpper()) || x.AssignedTo.ToUpper().Contains(searchString.ToUpper()) || x.AssignedBy.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!string.IsNullOrEmpty(assignedDateFilter))
            {
                list = list.Where(x => x.AssignedDate.Date == DateTime.Parse(assignedDateFilter).Date);
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
                case "assignedTo":
                    {
                        list = list.OrderBy(x => x.AssignedTo);
                        break;
                    }
                case "assignedBy":
                    {
                        list = list.OrderBy(x => x.AssignedBy);
                        break;
                    }
                case "assignedDate":
                    {
                        list = list.OrderBy(x => x.AssignedDate);
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
                ViewListAssignmentResponse recentlyCreatedItem = list.Where(item => item.Id == int.Parse(createdId)).AsNoTracking().FirstOrDefault();
                if (recentlyCreatedItem != null)
                {
                    list = list.Where(item => item.Id != int.Parse(createdId));

                    var sortedResultWithCreatedIdParam = StaticFunctions<ViewListAssignmentResponse>.Paging(list, start, end - 1);

                    sortedResultWithCreatedIdParam.Insert(0, recentlyCreatedItem);

                    return Ok(new ViewListPageResult<ViewListAssignmentResponse> { Data = sortedResultWithCreatedIdParam, Total = list.Count() + 1 });
                }
            }

            var sortedResult = StaticFunctions<ViewListAssignmentResponse>.Paging(list, start, end);

            return Ok(new ViewListPageResult<ViewListAssignmentResponse> { Data = sortedResult, Total = list.Count() });
        }

        private bool AssignmentChanged(Assignment updatingAssignment, UpdateAssignmentRequest updateRequest)
        {
            if (updatingAssignment.AssignedToAppUser.StaffCode == updateRequest.AssignToAppUserStaffCode
                && updatingAssignment.Asset.AssetCode == updateRequest.AssetCode
                && updatingAssignment.AssignedDate.Equals(updateRequest.AssignedDate)
                && updatingAssignment.Note.Equals(updateRequest.Note))
            {
                return false;
            }
            return true;
        }

        

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Assignment? assignment = await _dbContext.Assignments.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            if (assignment != null)
            {
                if (assignment.State == State.WaitingForAcceptance)
                {
                    try
                    {
                        assignment.IsDeleted = true;
                        await _dbContext.SaveChangesAsync();
                        return Ok(_mapper.Map<AssignmentResponse>(assignment));
                    }
                    catch (Exception e) { return BadRequest(e.Message); }
                }

                else return BadRequest("Assignment is Accepted and cannot be deleted");
            }
            return NotFound("Assignment does not exist");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateAssignmentRequest requestData)
        {
            var currentUsername = User.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            var currentUser = await _userManager.FindByNameAsync(currentUsername);
            var assignment = _mapper.Map<Assignment>(requestData);
            var staffId = _dbContext.AppUsers.First(x => x.StaffCode == requestData.AssignToAppUserStaffCode).Id;
            var assetId = _dbContext.Assets.First(x => x.AssetCode == requestData.AssetCode).Id;

            var isUnique = _dbContext.Assignments
                .FirstOrDefault(x =>
                x.AssetId == assetId &&
                x.AssignedTo == staffId &&
                x.IsDeleted == false && 
                !(x.State == State.Declined || x.State == State.Returned)) == null;
            if (!isUnique)
            {
                return BadRequest(new ErrorResponseResult<string>("Create Assignment unsuccessfully. Existed an assignment with selected User and Asset"));
            }
            try
            {
                assignment.State = State.WaitingForAcceptance;
                assignment.AssignedBy = currentUser.Id;
                assignment.AssignedTo = staffId;
                assignment.AssetId = assetId;
                _dbContext.Assignments.Add(assignment);
                var asset = _dbContext.Assets.First(x => x.AssetCode == requestData.AssetCode);
                _dbContext.Assets.Update(asset);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponseResult<string>(e.Message));
            }
            _dbContext.SaveChanges();
            var mappedResult = _mapper.Map<CreateAssignmentResponse>(assignment);
            return Ok(mappedResult);
        }

        [HttpGet("/api/home")]
        [Authorize]
        public async Task<ActionResult<ViewListPageResult<MyAssignmentResponse>>> GetHome(
            [FromQuery] int start,
            [FromQuery] int end,
            [FromQuery] string? sort = "id",
            [FromQuery] string? order = "ASC")
        {
            var userName = User.Identity.Name;
            var list = _dbContext.Assignments
                .Include(x => x.AssignedToAppUser)
                .Where(x => !x.IsDeleted && x.AssignedToAppUser.UserName.Equals(userName) &&
                    x.AssignedDate.Date <= DateTime.Today.Date &&
                    x.State != State.Declined && x.State != State.Returned)
                .Select(x => new MyAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    CategoryName = x.Asset.Category.Name,
                    AssignedDate = x.AssignedDate,
                    State = x.State,
                });
            switch (sort)
            {
                case "id":
                    {
                        list = list.OrderBy(x => x.Id);
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
                case "categoryName":
                    {
                        list = list.OrderBy(x => x.CategoryName);
                        break;
                    }
                case "assignedDate":
                    {
                        list = list.OrderBy(x => x.AssignedDate);
                        break;
                    }
                case "state":
                    {
                        list = list.OrderBy(x => x.State);
                        break;
                    }
                default:
                    {
                        list = list.OrderBy(x => x.AssetCode);
                        break;
                    }
            }

            if (order == "DESC")
            {
                list = list.Reverse();
            }

            var sortedResult = StaticFunctions<MyAssignmentResponse>.Paging(list, start, end);

            return Ok(new ViewListPageResult<MyAssignmentResponse> { Data = sortedResult, Total = list.Count() });
        }
    }
}
