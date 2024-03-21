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
			const int index     = 0;
			var       newBitmap = new Bitmap(1, 1);
			m_sut.SwapBitmaps(index, newBitmap);
			// Assert
			m_repository.Received(1).Swap(index, newBitmap);
		}

		[Fact]
		public void GetModified_ShouldReturnBitmap_WhenValidIndex() {
			// Arrange
			const int index          = 0;
			var       expectedBitmap = new Bitmap(1, 1);
			m_repository.GetModified(index).Returns(expectedBitmap);
			// Act
			var result = m_sut.GetModified(index);
			// Assert
			result.Should().BeSameAs(expectedBitmap);
		}

		[Fact]
		public void ModifyRgbUnsafe_ShouldReturnModifiedBitmapUnchanged_WhenValidIndexAndFunctor() {
			// Arrange
			const int index   = 0;
			var       initBmp = new Bitmap(1, 1);
			m_parallelOptions.Get.Returns(ParallelOptionsDefault.Get());
			m_repository.GetOriginal(index).Returns(initBmp);

			// Act
			var result = m_sut.ModifyRgbUnsafe(index,
				(ref int i, ref int red, ref int green, ref int blue)
					=> Functor(ref i, ref red, ref green, ref blue));

			// Assert
			result.Should().NotBeNull();
			return;

			void Functor(ref int i, ref int red, ref int green, ref int blue) {
			}
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