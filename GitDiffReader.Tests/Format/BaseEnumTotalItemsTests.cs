using System;
using Xunit;

namespace GitDiffReader.Tests.Format
{
    public abstract class BaseEnumTotalNumberOfPartsTests<TEnum>
    {
        private const String TotalNumberOfPartsName = "TotalNumberOfParts";
        private readonly Type EnumType = typeof(TEnum);

        [Fact]
        public void Enum_Must_Have_TotalNumberOfParts_Item()
        {
            // Act
            var result = Enum.TryParse(EnumType, TotalNumberOfPartsName, out Object o);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TotalNumberOfParts_Should_Be_Equal_To_Number_Of_Values_In_Enum()
        {
            // Arrange
            var allEnumValues = Enum.GetValues(EnumType);

            // Act
            var name = Enum.GetName(EnumType, (TEnum)(Object)(allEnumValues.Length - 1));

            // Assert
            Assert.Equal(TotalNumberOfPartsName, name);
        }
    }
}
