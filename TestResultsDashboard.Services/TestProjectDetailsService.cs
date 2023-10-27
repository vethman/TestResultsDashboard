using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestResultsDashboard.Common;
using TestResultsDashboard.Common.Enums;
using TestResultsDashboard.Repositories;
using TestResultsDashboard.Repositories.Entities;
using TestResultsDashboard.Services.Models;
using TestResultsDashboard.Services.Models.Dto;

namespace TestResultsDashboard.Services;

public interface ITestProjectDetailsService
{
    Task<string> AddTestProjectDetailsAsync(TestProjectDetailsCreateRequest createRequest);
    Task UpdateTestProjectDetailsAsync(TestProjectDetailsUpdateRequest updateRequest);
    Task UpdateTestProjectDetailsNameAsync(string id, string name, string updatedBy);
    Task<IEnumerable<TestProjectDetailsDto>> GetAllTestProjectDetailsAsync();
    Task<TestProjectDetailsDto?> GetTestProjectDetailsAsync(string name, int version);
    Task<bool> TestProjectDetailsExistsAsync(string name, int version);
    Task<bool> TestProjectDetailsIdExistsAsync(string id);
    Task<TestProjectDetailsTotalsDto> GetTotalsAsync();
}

public class TestProjectDetailsService : ITestProjectDetailsService
{
    private readonly IRepository<TestProjectDetailsEntity> _testProjectDetailsEntityRepository;

    public TestProjectDetailsService(IRepository<TestProjectDetailsEntity> testProjectDetailsEntityRepository)
    {
        _testProjectDetailsEntityRepository = testProjectDetailsEntityRepository;
    }

    public async Task<string> AddTestProjectDetailsAsync(TestProjectDetailsCreateRequest createRequest)
    {
        var utcNow = DateTime.UtcNow;
        
        var testProjectEntity = new TestProjectDetailsEntity
        {
            Name = createRequest.Name,
            Version = createRequest.Version,
            Summary = createRequest.Summary,
            TestCategory = createRequest.TestCategory,
            CreatedBy = createRequest.CreatedBy,
            CreatedAt = utcNow,
            UpdatedBy = createRequest.CreatedBy,
            LastUpdatedAt = utcNow
        };

        return await _testProjectDetailsEntityRepository.InsertAsync(testProjectEntity);
    }

    public async Task UpdateTestProjectDetailsAsync(TestProjectDetailsUpdateRequest updateRequest)
    {
        var entity = await _testProjectDetailsEntityRepository
            .FindOneAsync(x => x.Name == updateRequest.Name && x.Version == updateRequest.Version);

        var updateEntity = new TestProjectDetailsEntity
        {
            ID = entity!.ID,
            Name = updateRequest.Name,
            Version = updateRequest.Version,
            Summary = updateRequest.Summary,
            TestCategory = updateRequest.TestCategory,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedBy = updateRequest.UpdatedBy,
            LastUpdatedAt = DateTime.UtcNow
        };

        await _testProjectDetailsEntityRepository.UpdateAsync(updateEntity);
    }
    
    public async Task UpdateTestProjectDetailsNameAsync(string id, string name, string updatedBy)
    {
        var entity = await _testProjectDetailsEntityRepository.FindOneByIdAsync(id);

        var updateEntity = new TestProjectDetailsEntity
        {
            ID = entity!.ID,
            Name = name,
            Version = entity.Version,
            Summary = entity.Summary,
            TestCategory = entity.TestCategory,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            UpdatedBy = updatedBy,
            LastUpdatedAt = DateTime.UtcNow
        };

        await _testProjectDetailsEntityRepository.UpdateAsync(updateEntity);
    }

    public async Task<IEnumerable<TestProjectDetailsDto>> GetAllTestProjectDetailsAsync()
    {
        var entities = await _testProjectDetailsEntityRepository.FindAllAsync();

        return entities.Select(x => new TestProjectDetailsDto
        {
            Id = x.ID!,
            Name = x.Name,
            Version = x.Version,
            Summary = x.Summary,
            TestCategory = x.TestCategory,
            UpdatedBy = x.UpdatedBy,
            LastUpdatedAt = x.LastUpdatedAt
        });
    }
    
    public async Task<TestProjectDetailsDto?> GetTestProjectDetailsAsync(string name, int version)
    {
        var testProjectDetailsCollection = await GetAllTestProjectDetailsAsync();

        return testProjectDetailsCollection.SingleOrDefault(x => x.Name == name && x.Version == version);
    }
    
    public async Task<bool> TestProjectDetailsExistsAsync(string name, int version)
    {
        var testProjectDetails = await _testProjectDetailsEntityRepository
            .FindOneAsync(x => x.Name == name && x.Version == version);

        return testProjectDetails != null;
    }
    
    public async Task<bool> TestProjectDetailsIdExistsAsync(string id)
    {
        var testProjectDetails = await _testProjectDetailsEntityRepository.FindOneByIdAsync(id);

        return testProjectDetails != null;
    }

    public async Task<TestProjectDetailsTotalsDto> GetTotalsAsync()
    {
        var testProjectDetailsCollection = (await GetAllTestProjectDetailsAsync()).ToList();
        
        var totals = new TestProjectDetailsTotalsDto
        {
            Unit = testProjectDetailsCollection.Count(x => x.TestCategory == TestCategory.Unit),
            Integration = testProjectDetailsCollection.Count(x => x.TestCategory == TestCategory.Integration),
            UserInterface = testProjectDetailsCollection.Count(x => x.TestCategory == TestCategory.UserInterface)
        };
        
        totals.TotalTestProjects = totals.Unit + totals.Integration + totals.UserInterface;
        totals.UnitPercentage = PercentageCalculator.Calculate(totals.Unit, totals.TotalTestProjects);
        totals.IntegrationPercentage = PercentageCalculator.Calculate(totals.Integration, totals.TotalTestProjects);
        totals.UserInterfacePercentage = PercentageCalculator.Calculate(totals.UserInterface, totals.TotalTestProjects);

        return totals;
    }
}