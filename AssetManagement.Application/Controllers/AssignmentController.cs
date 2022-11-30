// using AssetManagement.Contracts.Assignment.Request;
using AssetManagement.Contracts.Assignment.Response;
using AssetManagement.Contracts.Common;
using AssetManagement.Data.EF;
using AssetManagement.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AssetManagement.Domain.Enums.Assignment;
using AssetManagement.Contracts.Assignment.Request;

namespace AssetManagement.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AssetManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public AssignmentsController(UserManager<AppUser> userManager,
            AssetManagementDbContext dbContext,
            IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("assignement/{assetCodeId}")]
        //[Authorize]
        public IActionResult GetAssignmentsByAssetCodeId(int assetCodeId)
        {
            var result = _dbContext.Assignments.Where(x => x.AssetId == assetCodeId).ToList();
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
                .Include(x => x.AssignedByToAppUser)
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
        public async Task<IActionResult> UpdateAssignment(int id, UpdateAssignmentRequest request)
        {
            Assignment updatingAssignment = await _dbContext.Assignments
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();

            try
            {
                if (updatingAssignment != null)
                {
                    updatingAssignment.AssignedTo = request.AssignedTo;
                    updatingAssignment.AssetId = request.AssetId;
                    updatingAssignment.AssignedDate = request.AssignedDate;
                    updatingAssignment.Note = request.Note;
                    await _dbContext.SaveChangesAsync();
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

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<ViewList_ListResponse<ViewListAssignmentResponse>>> Get(
            [FromQuery] int start,
            [FromQuery] int end,
            [FromQuery] string? searchString = "",
            [FromQuery] string? assignedDateFilter = "",
            [FromQuery] string? stateFilter = "",
            [FromQuery] string? sort = "name",
            [FromQuery] string? order = "ASC",
            [FromQuery] string? createdId = "")
        {
            var list = _dbContext.Assignments
                .Include(x => x.Asset)
                .Include(x => x.AssignedToAppUser)
                .Include(x => x.AssignedByToAppUser)
                .Select(x => new ViewListAssignmentResponse
                {
                    Id = x.Id,
                    AssetCode = x.Asset.AssetCode,
                    AssetName = x.Asset.Name,
                    AssignedTo = x.AssignedToAppUser.UserName,
                    AssignedBy = x.AssignedByToAppUser.UserName,
                    AssignedDate = x.AssignedDate,
                    State = x.State
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.AssetName.ToUpper().Contains(searchString.ToUpper()) || x.AssetCode.ToUpper().Contains(searchString.ToUpper()));
            }
            // if(!string.IsNullOrEmpty(assignedDateFilter))
            // {
            //     var arrayChar = assignedDateFilter.Split("&");
            //     var arrNumberChar = new List<int>();
            //     for (int i = 0; i < arrayChar.Length; i++)
            //     {
            //         var temp = 0;
            //         if (int.TryParse(arrayChar[i], out temp))
            //         {
            //             arrNumberChar.Add(temp);
            //         }
            //     }
            //     list = list.Where(x=> arrNumberChar.Contains(x.AssignedDate.GetValueOrDefault()));
            // }
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
                case "state":
                    {
                        list = list.OrderBy(x => x.State);
                        break;
                    }
                case "id":
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
                var newList = list.Where(item => item.Id == int.Parse(createdId));
                list = list.Where(item => item.Id != int.Parse(createdId));
                list = newList.Concat(list);
            }

            var sortedResult = StaticFunctions<ViewListAssignmentResponse>.Paging(list, start, end);

            // var mappedResult = _mapper.Map<List<ViewListAssignmentResponse>>(list);

            return Ok(new ViewList_ListResponse<ViewListAssignmentResponse> { Data = sortedResult, Total = list.Count() });
            // return Ok(sortedResult);
        }
    }
}
