using Moq;
using Xunit;
using webapi.Repository;
using webapi.service.impl;
using webapi.Model;
using webapi.Enum;
using webapi.Exception;

namespace webapi.Test
{
    public class ItemServiceTest
    {
        private readonly Mock<IInventoryRepo> _inventoryRepoMock;
        private readonly ItemService _itemService;

        public ItemServiceTest()
        {
            _inventoryRepoMock = new Mock<IInventoryRepo>();
            _itemService = new ItemService(_inventoryRepoMock.Object);
        }

        [Fact]
        public async Task GetItems_ReturnsListOfItems()
        {
            // Arrange
            string userId = "testUser";
            var inventories = new List<Inventory>
            {
                new Inventory(userId, "invader001", 2),
                new Inventory(userId, "food001", 3)
            };

            Item item1 = ItemsLibrary.INVADER_001;
            item1.Quantity = 2;
            Item item2 = ItemsLibrary.FOOD_001;
            item2.Quantity = 3;
            List<Item> items = new List<Item> { item1, item2 };
    
            _inventoryRepoMock.Setup(repo => repo.GetInventory(userId)).Returns(inventories);

            // Act
            List<Item> result = _itemService.GetItems(userId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(items, result);
        }

        [Fact]
        public async Task GetItems_NoItems()
        {
            string userId = "testUser";
            var inventories = new List<Inventory>();
    
            _inventoryRepoMock.Setup(repo => repo.GetInventory(userId)).Returns(inventories);

            List<Item> result = _itemService.GetItems(userId);

            Assert.Equal(result, new List<Item>());
        }

        [Fact]
        public async Task AddItem()
        {
            _itemService.AddItem("user", "invader001");
            _inventoryRepoMock.Verify(repo => repo.AddInventory("user", "invader001"), Times.Once);
        }

        [Fact]
        public async Task AddItem_InvalidItem()
        {
            var ex = await Assert.ThrowsAsync<GameException>(async () => _itemService.AddItem("user", "test"));
            Assert.Equal(ErrorCode.INVALID_ITEM, ex.Code);
        }

        [Fact]
        public async Task GenerateItem()
        {
            Item item = _itemService.GenerateItem();
            Assert.NotNull(item);
            Assert.NotNull(ItemsLibrary.GetItemById(item.ItemId));
        }
    }
}