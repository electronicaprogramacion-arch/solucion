using System;
using System.Collections.Generic;
using Xunit;
using CalibrationSaaS.Infraestructure.Blazor.Shared;

namespace CalibrationSaaS.Tests
{
    /// <summary>
    /// Unit tests for the updated CreateGroupedDictionary methods
    /// that support both flat and nested dictionary structures.
    /// </summary>
    public class CreateGroupedDictionaryTests
    {
        // Test data class
        public class TestItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
        }

        // Test component
        public class TestComponent : GenericTestComponent<object, object, object>
        {
            public TestComponent()
            {
                // Initialize AppState with mock for testing
                AppState = new CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState();
            }

            public object TestCreateGroupedDictionary<T>(
                string keyPropertyName,
                string valuePropertyName,
                string groupingPropertyName,
                List<T> items)
            {
                return CreateGroupedDictionary(keyPropertyName, valuePropertyName, groupingPropertyName, items);
            }
        }

        [Fact]
        public void CreateGroupedDictionary_WithNullGrouping_ReturnsFlatDictionary()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A" },
                new TestItem { Id = "2", Name = "Item2", Category = "B" },
                new TestItem { Id = "3", Name = "Item3", Category = "A" }
            };

            // Act
            var result = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: null,
                items: items
            );

            // Assert
            Assert.IsType<Dictionary<string, string>>(result);
            var flatDict = (Dictionary<string, string>)result;
            Assert.Equal(3, flatDict.Count);
            Assert.Equal("Item1", flatDict["1"]);
            Assert.Equal("Item2", flatDict["2"]);
            Assert.Equal("Item3", flatDict["3"]);
        }

        [Fact]
        public void CreateGroupedDictionary_WithGrouping_ReturnsNestedDictionary()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A" },
                new TestItem { Id = "2", Name = "Item2", Category = "B" },
                new TestItem { Id = "3", Name = "Item3", Category = "A" }
            };

            // Act
            var result = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: "Category",
                items: items
            );

            // Assert
            Assert.IsType<Dictionary<string, Dictionary<string, string>>>(result);
            var nestedDict = (Dictionary<string, Dictionary<string, string>>)result;
            Assert.Equal(2, nestedDict.Count);
            Assert.True(nestedDict.ContainsKey("A"));
            Assert.True(nestedDict.ContainsKey("B"));
            Assert.Equal(2, nestedDict["A"].Count);
            Assert.Equal(1, nestedDict["B"].Count);
            Assert.Equal("Item1", nestedDict["A"]["1"]);
            Assert.Equal("Item3", nestedDict["A"]["3"]);
            Assert.Equal("Item2", nestedDict["B"]["2"]);
        }

        [Fact]
        public void CreateGroupedDictionary_WithEmptyGrouping_ReturnsFlatDictionary()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A" }
            };

            // Act
            var result = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: "",  // Empty string should be treated like null
                items: items
            );

            // Assert
            Assert.IsType<Dictionary<string, string>>(result);
            var flatDict = (Dictionary<string, string>)result;
            Assert.Single(flatDict);
            Assert.Equal("Item1", flatDict["1"]);
        }

        [Fact]
        public void CreateGroupedDictionary_WithNullItems_ThrowsArgumentNullException()
        {
            // Arrange
            var component = new TestComponent();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                component.TestCreateGroupedDictionary<TestItem>(
                    keyPropertyName: "Id",
                    valuePropertyName: "Name",
                    groupingPropertyName: null,
                    items: null
                )
            );
        }

        [Fact]
        public void CreateGroupedDictionary_WithInvalidKeyProperty_ThrowsArgumentException()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A" }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                component.TestCreateGroupedDictionary(
                    keyPropertyName: "InvalidProperty",
                    valuePropertyName: "Name",
                    groupingPropertyName: null,
                    items: items
                )
            );

            Assert.Contains("InvalidProperty", exception.Message);
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public void CreateGroupedDictionary_WithInvalidValueProperty_ThrowsArgumentException()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A" }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                component.TestCreateGroupedDictionary(
                    keyPropertyName: "Id",
                    valuePropertyName: "InvalidProperty",
                    groupingPropertyName: null,
                    items: items
                )
            );

            Assert.Contains("InvalidProperty", exception.Message);
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public void CreateGroupedDictionary_WithInvalidGroupingProperty_ThrowsArgumentException()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A" }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                component.TestCreateGroupedDictionary(
                    keyPropertyName: "Id",
                    valuePropertyName: "Name",
                    groupingPropertyName: "InvalidProperty",
                    items: items
                )
            );

            Assert.Contains("InvalidProperty", exception.Message);
            Assert.Contains("not found", exception.Message);
        }

        [Fact]
        public void CreateGroupedDictionary_WithEmptyList_ReturnsEmptyFlatDictionary()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>();

            // Act
            var result = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: null,
                items: items
            );

            // Assert
            Assert.IsType<Dictionary<string, string>>(result);
            var flatDict = (Dictionary<string, string>)result;
            Assert.Empty(flatDict);
        }

        [Fact]
        public void CreateGroupedDictionary_WithEmptyList_ReturnsEmptyNestedDictionary()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>();

            // Act
            var result = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: "Category",
                items: items
            );

            // Assert
            Assert.IsType<Dictionary<string, Dictionary<string, string>>>(result);
            var nestedDict = (Dictionary<string, Dictionary<string, string>>)result;
            Assert.Empty(nestedDict);
        }

        [Fact]
        public void CreateGroupedDictionary_WithDuplicateKeys_LastValueWins()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "First", Category = "A" },
                new TestItem { Id = "1", Name = "Second", Category = "A" },
                new TestItem { Id = "1", Name = "Third", Category = "A" }
            };

            // Act
            var result = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: null,
                items: items
            );

            // Assert
            var flatDict = (Dictionary<string, string>)result;
            Assert.Single(flatDict);
            Assert.Equal("Third", flatDict["1"]); // Last value should win
        }

        [Fact]
        public void CreateGroupedDictionary_WithNullItemsInList_SkipsNullItems()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A" },
                null,
                new TestItem { Id = "2", Name = "Item2", Category = "B" },
                null
            };

            // Act
            var result = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: null,
                items: items
            );

            // Assert
            var flatDict = (Dictionary<string, string>)result;
            Assert.Equal(2, flatDict.Count);
            Assert.Equal("Item1", flatDict["1"]);
            Assert.Equal("Item2", flatDict["2"]);
        }

        [Fact]
        public void CreateGroupedDictionary_WithNullPropertyValues_UsesEmptyString()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = null, Category = "A" },
                new TestItem { Id = null, Name = "Item2", Category = "B" }
            };

            // Act
            var result = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: null,
                items: items
            );

            // Assert
            var flatDict = (Dictionary<string, string>)result;
            Assert.Equal(2, flatDict.Count);
            Assert.Equal("", flatDict["1"]);
            Assert.Equal("Item2", flatDict[""]);
        }

        [Fact]
        public void CreateGroupedDictionary_PatternMatching_WorksCorrectly()
        {
            // Arrange
            var component = new TestComponent();
            var items = new List<TestItem>
            {
                new TestItem { Id = "1", Name = "Item1", Category = "A" }
            };

            // Act - Test with null grouping
            var flatResult = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: null,
                items: items
            );

            // Assert - Pattern matching for flat dictionary
            Assert.True(flatResult is Dictionary<string, string>);
            Assert.False(flatResult is Dictionary<string, Dictionary<string, string>>);

            // Act - Test with grouping
            var nestedResult = component.TestCreateGroupedDictionary(
                keyPropertyName: "Id",
                valuePropertyName: "Name",
                groupingPropertyName: "Category",
                items: items
            );

            // Assert - Pattern matching for nested dictionary
            Assert.False(nestedResult is Dictionary<string, string>);
            Assert.True(nestedResult is Dictionary<string, Dictionary<string, string>>);
        }
    }
}

