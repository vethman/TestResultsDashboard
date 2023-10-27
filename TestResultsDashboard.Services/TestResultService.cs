using TestResultsDashboard.Common;
using TestResultsDashboard.Common.Enums;
using TestResultsDashboard.Repositories;
using TestResultsDashboard.Repositories.Entities;
using TestResultsDashboard.Services.Models;
using TestResultsDashboard.Services.Models.Dto;

namespace TestResultsDashboard.Services;

public interface ITestResultService
{
    Task AddTestResultAsync(TestResultCreateRequest createRequest);
    Task<IEnumerable<TestResultDto>> GetAllTestResultsForTestProject(string testProjectId);
}

public class TestResultService : ITestResultService
{
    private readonly IRepository<TestResultEntity> _testResultEntityRepository;

    public TestResultService(IRepository<TestResultEntity> testResultEntityRepository)
    {
        _testResultEntityRepository = testResultEntityRepository;
    }

    public async Task AddTestResultAsync(TestResultCreateRequest createRequest)
    {
        var entity = new TestResultEntity
        {
            TestProjectId = createRequest.TestProjectId,
            Passed = createRequest.Passed,
            Failed = createRequest.Failed,
            Skipped = createRequest.Skipped,
            CreatedBy = createRequest.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            Start = createRequest.Start,
            End = createRequest.End
        };

        await _testResultEntityRepository.InsertAsync(entity);
    }

    public async Task<IEnumerable<TestResultDto>> GetAllTestResultsForTestProject(string testProjectId)
    {
        var entities = await _testResultEntityRepository
            .FindAsync(x => x.TestProjectId == testProjectId);

        return entities.Select(CreateTestResultDto);
    }

    private static TestResultDto CreateTestResultDto(TestResultEntity entity)
    {
        var dto = new TestResultDto
        {
            PassedTests = entity.Passed,
            FailedTests = entity.Failed,
            SkippedTests = entity.Skipped,
            TotalTests = entity.Passed + entity.Failed + entity.Skipped,
            CreatedBy = entity.CreatedBy,
            CreatedAt = entity.CreatedAt,
            Duration = entity.End - entity.Start
        };

        dto.PassedPercentage = PercentageCalculator.Calculate(dto.PassedTests, dto.TotalTests);
        dto.FailedPercentage = PercentageCalculator.Calculate(dto.FailedTests, dto.TotalTests);
        dto.SkippedPercentage = PercentageCalculator.Calculate(dto.SkippedTests, dto.TotalTests);
        dto.Status = GetStatus(dto.PassedTests, dto.FailedTests, dto.SkippedTests);

        return dto;
    }
    
    private static Status GetStatus(int passedTests, int failedTests, int skippedTests)
    {
        if (failedTests > 0)
            return Status.Failed;
        if (passedTests > 0 && failedTests == 0 && skippedTests == 0)
            return Status.Passed;
        if (skippedTests > 0)
            return Status.Skipped;
        if (passedTests == 0 && failedTests == 0 && skippedTests == 0)
            return Status.NoRun;
        return Status.Unknown;
    }
}