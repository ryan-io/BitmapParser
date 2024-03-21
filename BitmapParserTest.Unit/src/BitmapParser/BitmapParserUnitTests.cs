using System.Drawing;
using BitmapParser;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;


namespace BitmapParserTest.Unit.BitmapParser {
	public class BitmapParserUnitTests : IDisposable {
		public BitmapParserUnitTests() {
			m_sut = new global::BitmapParser.BitmapParser(m_repository, m_imageSaver, m_parallelOptions);
		}

		public void Dispose() {
			m_sut.Dispose();
		}

		[Fact]
		public void Dispose_ShouldSetIsDisposedToTrue() {
			m_sut.Dispose();
			m_sut.IsDisposed.Should().BeTrue();
		}

		[Fact]
		public void GetOriginal_ShouldReturnBitmap_WhenValidIndex() {
			// Arrange
			int    index          = 0;
			Bitmap expectedBitmap = new Bitmap(1, 1);
			m_repository.GetOriginal(index).Returns(expectedBitmap);
			// Act
			Bitmap result = m_sut.GetOriginal(index);
			// Assert
			result.Should().BeSameAs(expectedBitmap);
		}

		[Fact]
		public void SetNewBitmap_ShouldCallSwap_WhenValidIndexAndBitmap() {
			// Arrange
			int    index     = 0;
			Bitmap newBitmap = new Bitmap(1, 1);
			m_sut.SwapBitmaps(index, newBitmap);
			// Assert
			m_repository.Received(1).Swap(index, newBitmap);
		}

		[Fact]
		public void GetModified_ShouldReturnBitmap_WhenValidIndex() {
			// Arrange
			int    index          = 0;
			Bitmap expectedBitmap = new Bitmap(1, 1);
			m_repository.GetModified(index).Returns(expectedBitmap);
			// Act
			Bitmap result = m_sut.GetModified(index);
			// Assert
			result.Should().BeSameAs(expectedBitmap);
		}

		[Fact]
		public void ModifyRgbUnsafe_ShouldReturnModifiedBitmap_WhenValidIndexAndFunctor() {
			// Arrange
			int    index          = 0;
			Bitmap expectedBitmap = new Bitmap(1, 1);
			m_repository.Original.Returns(new Bitmap[] { new(2, 2), new(2, 2) });
			m_repository.Modified.Returns(new[] { expectedBitmap, new(2, 2) });
			global::BitmapParser.BitmapParser.BitmapRgbDelegate functor
				= (ref int pxlIndex, ref int red, ref int green, ref int blue) => { };

			m_repository.GetModified(index).Returns(expectedBitmap);
			// Act
			Bitmap result = m_sut.ModifyRgbUnsafe(index, functor);
			// Assert
			result.Should().BeSameAs(expectedBitmap);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(50)]
		public void GetAllBitmaps_ShouldReturnCountGreaterOrEqual0_WhenInValidState(int num) {
			m_repository.Original.Returns(Enumerable.Repeat(new Bitmap(1, 1), num).ToArray());
			m_repository.Count.Returns(num);
			m_sut.Count.Should().BeGreaterThanOrEqualTo(0);
			m_sut.Count.Should().Be(num);
		}

		[Fact]
		public void GetAllBitmapsOriginal_ShouldThrowNullReferenceException_WhenBitMapArrayIsNull() {
			m_sut.GetAllOriginalBitmaps()
			     .Throws(new NullReferenceException(BitmapRepository.EXCEPTION_NULL_ORIGINAL_ARRAY));

			Action action = () => m_sut.GetAllOriginalBitmaps();

			action.Should()
			      .Throw<NullReferenceException>()
			      .WithMessage(BitmapRepository.EXCEPTION_NULL_ORIGINAL_ARRAY);
		}

		[Fact]
		public void GetAllBitmapsModified_ShouldThrowNullReferenceException_WhenBitMapArrayIsNull() {
			m_sut.GetAllModifiedBitmaps()
			     .Throws(new NullReferenceException(BitmapRepository.EXCEPTION_NULL_MODIFIED_ARRAY));

			Action action = () => m_sut.GetAllModifiedBitmaps();

			action.Should()
			      .Throw<NullReferenceException>()
			      .WithMessage(BitmapRepository.EXCEPTION_NULL_MODIFIED_ARRAY);
		}

		readonly global::BitmapParser.BitmapParser m_sut;
		readonly IBitmapRepository                 m_repository      = Substitute.For<IBitmapRepository>();
		readonly IImageSaver                       m_imageSaver      = Substitute.For<IImageSaver>();
		readonly IParallelOptions                  m_parallelOptions = Substitute.For<IParallelOptions>();
	}
}