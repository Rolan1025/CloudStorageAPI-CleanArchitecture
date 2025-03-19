using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentAssertions;
using CloudStorageAPICleanArchitecture.Application.Common.Mappings;
using CloudStorageAPICleanArchitecture.Application.Common.Models;
using MockQueryable;
using MockQueryable.Moq;
using NUnit.Framework;

namespace CloudStorageAPICleanArchitecture.Application.UnitTests.Common.Mappings;

[TestFixture]
public class MappingExtensionsTests
{
    private IConfigurationProvider _configuration = null!;

    [SetUp]
    public void SetUp()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<TestEntity, TestDto>());

        _configuration = config;
    }

    [Test]
    public void PaginatedList_ShouldReturnCorrectPagination()
    {
        // Arrange
        var data = Enumerable.Range(1, 20).Select(x => new TestDto { Id = x }).AsQueryable();

        // Act
        var result = PaginatedList<TestDto>.Create(data, pageNumber: 2, pageSize: 5);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(5);
        result.Items.First().Id.Should().Be(6);
        result.PageNumber.Should().Be(2);
        result.TotalCount.Should().Be(20);
        result.TotalPages.Should().Be(4);
    }

    [Test]
    public async Task ProjectToListAsync_ShouldProjectDataCorrectly()
    {
        // Arrange
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Prueba" },
            new() { Id = 2, Name = "Test" }
        };

        // Usa MockQueryable para simular IAsyncEnumerable
        var queryable = entities.AsQueryable().BuildMock();

        // Act
        var result = await queryable.ProjectToListAsync<TestDto>(_configuration);

        // Assert
        result.Should().HaveCount(2);
        result.Select(x => x.Name).Should().Contain(new[] { "Prueba", "Test" });
    }

    #region Clases internas para pruebas
    private class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    #endregion
}
