using Xunit;
using Moq;
using BLLAccountingDemo;
using EFAccounting;
using AutoMapper;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace AccountingDemo.Tests
{
    public class KidManagerTests
    {
        private Context CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            return new Context(options);
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<DTO.Kid, EFAccounting.Entities.Kid>();
                cfg.CreateMap<EFAccounting.Entities.Kid, DTO.Kid>();
            });
            
            return config.CreateMapper();
        }

        [Fact]
        public void CreateKid_ShouldAddKidToContext()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var mapper = CreateMapper();
            var kidManager = new KidManager(context, mapper);
            var testKid = new DTO.Kid { Id = 1, Name = "John", LastName = "Doe" };
            
            // Act
            kidManager.CreateKid(testKid);
            
            // Assert
            Assert.Single(context.Kids);
            var savedKid = context.Kids.First();
            Assert.Equal("John", savedKid.Name);
            Assert.Equal("Doe", savedKid.LastName);
        }

        [Fact]
        public async Task GetKids_ShouldReturnEmptyList_WhenNoKidsExist()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var mapper = CreateMapper();
            var kidManager = new KidManager(context, mapper);
            
            // Act
            var result = await kidManager.GetKids();
            
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetKids_ShouldReturnAllKids_WhenKidsExist()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var mapper = CreateMapper();
            var kidManager = new KidManager(context, mapper);
            
            // Add some test data
            var kid1 = new EFAccounting.Entities.Kid { Id = 1, Name = "John", LastName = "Doe" };
            var kid2 = new EFAccounting.Entities.Kid { Id = 2, Name = "Jane", LastName = "Smith" };
            context.Kids.AddRange(kid1, kid2);
            await context.SaveChangesAsync();
            
            // Act
            var result = await kidManager.GetKids();
            
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, k => k.Name == "John");
            Assert.Contains(result, k => k.Name == "Jane");
        }
    }
} 