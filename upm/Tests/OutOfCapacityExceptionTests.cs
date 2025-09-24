using System;
using Moroshka.Xcp;
using NUnit.Framework;
using Is = NUnit.Framework.Is;

namespace Moroshka.Collections.Tests
{

[TestFixture]
internal sealed class OutOfCapacityExceptionTests
{
	[Test]
	public void Constructor_WithMessageAndInnerException_SetsPropertiesCorrectly()
	{
		// Arrange
		const string message = "Test error message";
		var innerException = new InvalidOperationException("Inner exception");

		// Act
		var exception = new OutOfCapacityException(message, innerException);

		// Assert
		Assert.That(exception.Message, Is.EqualTo(message));
		Assert.That(exception.InnerException, Is.SameAs(innerException));
		Assert.That(exception.Code, Is.EqualTo("OUT_OF_CAPACITY"));
	}

	[Test]
	public void Constructor_WithMessageOnly_SetsPropertiesCorrectly()
	{
		// Arrange
		const string message = "Test error message";

		// Act
		var exception = new OutOfCapacityException(message);

		// Assert
		Assert.That(exception.Message, Is.EqualTo(message));
		Assert.That(exception.InnerException, Is.Null);
		Assert.That(exception.Code, Is.EqualTo("OUT_OF_CAPACITY"));
	}

	[Test]
	public void Constructor_WithInnerExceptionOnly_UsesDefaultMessage()
	{
		// Arrange
		var innerException = new InvalidOperationException("Inner exception");

		// Act
		var exception = new OutOfCapacityException(innerException);

		// Assert
		Assert.That(exception.Message, Is.EqualTo("Required capacity is too large"));
		Assert.That(exception.InnerException, Is.SameAs(innerException));
		Assert.That(exception.Code, Is.EqualTo("OUT_OF_CAPACITY"));
	}

	[Test]
	public void Constructor_WithoutParameters_UsesDefaultMessage()
	{
		// Act
		var exception = new OutOfCapacityException();

		// Assert
		Assert.That(exception.Message, Is.EqualTo("Required capacity is too large"));
		Assert.That(exception.InnerException, Is.Null);
		Assert.That(exception.Code, Is.EqualTo("OUT_OF_CAPACITY"));
	}

	[Test]
	public void CurrentCapacity_CanBeSetAndRetrieved()
	{
		// Arrange
		const string capacity = "100";

		// Act
		var exception = new OutOfCapacityException
		{
			CurrentCapacity = capacity
		};

		// Assert
		Assert.That(exception.CurrentCapacity, Is.EqualTo(capacity));
	}

	[Test]
	public void RequiredSize_CanBeSetAndRetrieved()
	{
		// Arrange
		const string size = "150";

		// Act
		var exception = new OutOfCapacityException
		{
			RequiredSize = size
		};

		// Assert
		Assert.That(exception.RequiredSize, Is.EqualTo(size));
	}

	[Test]
	public void CurrentCapacity_WithNullValue_ReturnsNull()
	{
		// Act
		var exception = new OutOfCapacityException
		{
			CurrentCapacity = null
		};

		// Assert
		Assert.That(exception.CurrentCapacity, Is.Null);
	}

	[Test]
	public void RequiredSize_WithNullValue_ReturnsNull()
	{
		// Act
		var exception = new OutOfCapacityException
		{
			RequiredSize = null
		};

		// Assert
		Assert.That(exception.RequiredSize, Is.Null);
	}

	[Test]
	public void CurrentCapacity_WithEmptyString_ReturnsEmptyString()
	{
		// Arrange
		const string capacity = "";

		// Act
		var exception = new OutOfCapacityException
		{
			CurrentCapacity = capacity
		};

		// Assert
		Assert.That(exception.CurrentCapacity, Is.EqualTo(capacity));
	}

	[Test]
	public void RequiredSize_WithEmptyString_ReturnsEmptyString()
	{
		// Arrange
		const string size = "";

		// Act
		var exception = new OutOfCapacityException
		{
			RequiredSize = size
		};

		// Assert
		Assert.That(exception.RequiredSize, Is.EqualTo(size));
	}

	[Test]
	public void InheritsFromDetailedException()
	{
		// Act
		var exception = new OutOfCapacityException();

		// Assert
		Assert.That(exception, Is.InstanceOf<DetailedException>());
	}

	[Test]
	public void Data_ContainsCurrentCapacityAndRequiredSize()
	{
		// Arrange
		const string capacity = "200";
		const string size = "300";

		// Act
		var exception = new OutOfCapacityException
		{
			CurrentCapacity = capacity,
			RequiredSize = size
		};

		// Assert
		Assert.That(exception.Data[nameof(OutOfCapacityException.CurrentCapacity)], Is.EqualTo(capacity));
		Assert.That(exception.Data[nameof(OutOfCapacityException.RequiredSize)], Is.EqualTo(size));
	}

	[Test]
	public void Constructor_WithEmptyMessage_SetsEmptyMessage()
	{
		// Arrange
		const string message = "";

		// Act
		var exception = new OutOfCapacityException(message);

		// Assert
		Assert.That(exception.Message, Is.EqualTo(message));
		Assert.That(exception.Code, Is.EqualTo("OUT_OF_CAPACITY"));
	}
}

}
