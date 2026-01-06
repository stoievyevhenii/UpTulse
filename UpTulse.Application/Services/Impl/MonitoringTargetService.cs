using Facet.Extensions;
using Facet.Mapping;

using FluentValidation;

using UpTulse.Application.Managers;
using UpTulse.Application.MapperConfigs;
using UpTulse.Application.Models;
using UpTulse.Application.ModelsValidators;
using UpTulse.Core.Entities;
using UpTulse.Core.Exceptions;
using UpTulse.DataAccess.Repositories;

namespace UpTulse.Application.Services.Impl
{
    public class MonitoringTargetService : IMonitoringTargetService
    {
        private readonly IMonitoringGroupService _monitoringGroupService;
        private readonly IMonitoringTargetsManager _monitoringManagerService;
        private readonly MonitoringTargetMapperWithDi _monitoringTargetMapperWithDi;
        private readonly IMonitoringTargetRepository _monitoringTargetRepository;
        private readonly IValidator<MonitoringTargetRequest> _monitoringTargetRequestValidator;

        public MonitoringTargetService(
            MonitoringTargetMapperWithDi monitoringTargetMapperWithDi,
            IMonitoringTargetRepository monitoringTargetRepository,
            IValidator<MonitoringTargetRequest> monitoringTargetRequestValidator,
            IMonitoringTargetsManager monitoringManagerService,
            IMonitoringGroupService monitoringGroupService)
        {
            _monitoringTargetRepository = monitoringTargetRepository;
            _monitoringTargetMapperWithDi = monitoringTargetMapperWithDi;
            _monitoringManagerService = monitoringManagerService;
            _monitoringGroupService = monitoringGroupService;
            _monitoringTargetRequestValidator = monitoringTargetRequestValidator;
        }

        public async Task<MonitoringTargetResponse> CreateAsync(MonitoringTargetRequest request)
        {
            var validationResult = await _monitoringTargetRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);

                throw new ModelValidationNotPassedException(string.Join(", ", errors));
            }

            var recordAlreadyExist = await _monitoringTargetRepository.AnyAsync(r => r.Name.Trim().ToUpper() == request.Name.Trim().ToUpper());

            if (recordAlreadyExist)
            {
                throw new DbRecordAlreadyExistsException($"Monitoring target with name {request.Name} already exists.");
            }

            var newMonitoringTarget = new MonitoringTarget();

            newMonitoringTarget.ApplyFacet(request);

            var response = await _monitoringTargetRepository.AddAsync(newMonitoringTarget);

            await _monitoringManagerService.AddOrUpdateExistTargetAsync(request);

            return await ReturnAfterMap(response);
        }

        public async Task<MonitoringTargetResponse> DeleteAsync(Guid id)
        {
            var response = await _monitoringTargetRepository.DeleteAsync(r => r.Id == id);

            if (response is not null)
            {
                await _monitoringManagerService.RemoveTargetAsync(response.Name);
            }

            return await ReturnAfterMap(response ?? throw new DbRecordNotFoundException($"Record with id {id} not found"));
        }

        public async Task<IEnumerable<MonitoringTargetResponse>> GetAllAsync()
        {
            var records = await _monitoringTargetRepository.GetAllAsync();

            var mappedList = await records.ToFacetsAsync(_monitoringTargetMapperWithDi);

            return mappedList;
        }

        public async Task<MonitoringTargetResponse?> GetByIdAsync(Guid id)
        {
            if (!await _monitoringTargetRepository.AnyAsync(x => x.Id == id))
            {
                return null;
            }

            var record = await _monitoringTargetRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            return await ReturnAfterMap(record);
        }

        public async Task<MonitoringTargetResponse> UpdateAsync(Guid id, MonitoringTargetUpdateRequest request)
        {
            var oldRecord = await _monitoringTargetRepository.GetFirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DbRecordNotFoundException($"Monitoring target record with ID {id} not found");

            oldRecord.Name = string.IsNullOrWhiteSpace(request.Name) ? oldRecord.Name : request.Name;
            oldRecord.Address = string.IsNullOrWhiteSpace(request.Address) ? oldRecord.Address : request.Address;
            oldRecord.Description = string.IsNullOrWhiteSpace(request.Description) ? oldRecord.Description : request.Description;
            oldRecord.Protocol = request.Protocol != null ? request.Protocol.Value : oldRecord.Protocol;

            if (
                request.GroupId != Guid.Empty &&
                request.GroupId != null &&
                request.GroupId is Guid guid &&
                await _monitoringGroupService.AnyAsync(guid))
            {
                oldRecord.GroupId = request.GroupId != oldRecord.GroupId ? request.GroupId.Value : oldRecord.GroupId;
            }

            var updatedRecord = await _monitoringTargetRepository.UpdateAsync(oldRecord);

            var updatedTargetToRequestFacet = new MonitoringTargetRequest();
            updatedTargetToRequestFacet.ApplyFacet(updatedRecord);

            await _monitoringManagerService.RemoveTargetAsync(oldRecord.Name);
            await _monitoringManagerService.AddOrUpdateExistTargetAsync(updatedTargetToRequestFacet);

            return await ReturnAfterMap(updatedRecord);
        }

        private async Task<MonitoringTargetResponse> ReturnAfterMap(MonitoringTarget monitoringTarget)
        {
            return await monitoringTarget.ToFacetAsync(_monitoringTargetMapperWithDi);
        }
    }
}