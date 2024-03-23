using System.Drawing;
using System.Runtime.InteropServices;
using BitmapParser;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Xunit;


namespace BitmapParserTest.Unit.BitmapParser {
	public class BitmapParserUnitTests : IDisposable {
		Bitmap[] MockArrays { get; set; }

		public BitmapParserUnitTests() {
			MockArrays = Enumerable.Repeat(new Bitmap(1, 1), 100).ToArray();
			m_sut      = new global::BitmapParser.BitmapParser(m_repository, m_imageSaver, m_parallelOptions);
		}

		public void Dispose() {
			m_sut.Dispose();
		}

		[Fact]
		public void Dispose_ShouldSetIsDisposedToTrue() {
			m_sut.Dispose();
			m_sut.IsDisposed.Should().BeTrue();
		}

		[Theory]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(100)]
		[InlineData(0)]
		public void Count_ShouldReturnValidLength_WhenOriginalArrayIsNotNull(int expected) {
			m_repository.Count.Returns(expected);

			var count = m_sut.Count;

			count.Should().Be(expected);
		}

		[Fact]
		public void Count_ShouldThrowNullReferenceException_WhenOriginalArrayIsNull() {
			m_repository.Count.Throws(new NullReferenceException(
				BitmapRepository.EXCEPTION_NULL_ORIGINAL_ARRAY));

			Action action = () => _ = m_sut.Count;

			action.Should()
			      .Throw<NullReferenceException>()
			      .WithMessage(BitmapRepository.EXCEPTION_NULL_ORIGINAL_ARRAY);
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
			m_repository.Received(1).GetModified(index);
			result.Should().BeSameAs(expectedBitmap);
		}

		[Fact]
		public void GetAllModifiedBitmaps_ShouldReturnThrowBitMapParserDisposedException_WhenBitmapParserIsDisposed() {
			m_sut.Dispose();

			Action getAllModified = () => m_sut.GetAllModifiedBitmaps();

			getAllModified.Should().Throw<BitMapParserDisposedException>()
			              .WithMessage(BitMapParserDisposedException.EXCEPTION_BITMAP_PARSER_DISPOSED);
		}

		[Fact]
		public void GetAllOriginalBitmaps_ShouldReturnThrowBitMapParserDisposedException_WhenBitmapParserIsDisposed() {
			m_sut.Dispose();

			Action getAllOriginal = () => m_sut.GetAllOriginalBitmaps();

			getAllOriginal.Should().Throw<BitMapParserDisposedException>()
			              .WithMessage(BitMapParserDisposedException.EXCEPTION_BITMAP_PARSER_DISPOSED);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(50)]
		public void GetOriginal_ShouldReturnValid_WhenIndexParameterIsValid(int index) {
			m_repository.GetOriginal(index).Returns(MockArrays[index]);

			var original = m_sut.GetOriginal(index);

			m_repository.Received(1).GetOriginal(Arg.Is(index));
			original.Should().BeEquivalentTo(MockArrays[index]);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(50)]
		public void GetModified_ShouldReturnValid_WhenIndexParameterIsValid(int index) {
			m_repository.GetModified(index).Returns(MockArrays[index]);

			var original = m_sut.GetModified(index);

			m_repository.Received(1).GetModified(Arg.Is(index));
			original.Should().BeEquivalentTo(MockArrays[index]);
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
			m_repository.Received(1).GetOriginal(Arg.Is(index));

			m_sut.IsDisposed.Should().BeFalse();
			result.Should().NotBeNull();
			result.Should().NotBeSameAs(initBmp);
			return;

			void Functor(ref int i, ref int red, ref int green, ref int blue) {
				var intIndex = i;
				red++;
				green++;
				blue++;
				var test = index;
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

		[Fact]
		public void GetPaths_ShouldReturnPopulatedStringArray_WhenInValidState() {
			// Arrange
			m_repository.Paths.Returns(Enumerable.Repeat("Location", 10).ToArray());

			// Act
			var paths = m_sut.GetPaths();

			// Assert
			paths.Should().NotBeNull();
			paths.Should().NotBeEmpty();
			paths.Length.Should().Be(m_repository.Paths.Length);
		}


		[Fact]
		public void GetPaths_ShouldHaveSameLengthAsModifiedAndOriginal_WhenInValidState() {
			// Arrange
			m_repository.Paths.Returns(Enumerable.Repeat("Location",          10).ToArray());
			m_repository.Original.Returns(Enumerable.Repeat(new Bitmap(1, 1), 10).ToArray());
			m_repository.Modified.Returns(Enumerable.Repeat(new Bitmap(1, 1), 10).ToArray());

			// Act
			var paths = m_sut.GetPaths();

			// Assert
			paths.Length.Should().Be(m_repository.Original.Length);
			paths.Length.Should().Be(m_repository.Modified.Length);
		}

		[Fact]
		public void GetPaths_ShouldThrowException_WhenPathsIsNullOrEmpty() {
			// Arrange
			var e = new NullReferenceException(BitmapRepository.EXCEPTION_NULL_PATHS_ARRAY);
			m_repository.Paths.Throws(e);

			// Act
			Action pathAction = () => m_sut.GetPaths();

			// Assert
			pathAction.Should()
			          .Throw<NullReferenceException>()
			          .WithMessage(BitmapRepository.EXCEPTION_NULL_PATHS_ARRAY);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(10)]
		[InlineData(100)]
		public void GetPath_ShouldReturnNonEmptyString_WhenValidIndexIsPassed(int index) {
			// Arrange
			m_repository.GetPath(index).ReturnsForAnyArgs("location");
			// Act
			var path = m_sut.GetPath(index);

			// Assert
			path.Should().NotBeNullOrEmpty();
		}
		
		[Theory]
		[InlineData(-1)]
		[InlineData(10)]
		[InlineData(100)]
		public void GetPath_ShouldThrowIndexOutOfRangeException_WhenIndexOutSideOfBounds(int index) {
			// Arrange
			m_repository.GetPath(index)
			            .Throws(new IndexOutOfRangeException(BitmapRepository.EXCEPTION_INDEX_OUT_OF_RANGE));
			
			// Act
			var pathResponse =() => m_sut.GetPath(index);

			// Assert
			pathResponse.Should()
			            .Throw<IndexOutOfRangeException>()
			            .WithMessage(BitmapRepository.EXCEPTION_INDEX_OUT_OF_RANGE);
		}
		
	[Fact]
		public void GetPath_ShouldNullReferenceException_WhenPathsIsNull() {
			// Arrange
			m_repository.GetPath(Arg.Any<int>())
			            .Throws(new NullReferenceException(BitmapRepository.EXCEPTION_NULL_PATHS_ARRAY));
			
			// Act
			var pathResponse =() => m_sut.GetPath(1);

			// Assert
			pathResponse.Should()
			            .Throw<NullReferenceException>()
			            .WithMessage(BitmapRepository.EXCEPTION_NULL_PATHS_ARRAY);
		}

		[Fact]
		public void Dispose_ShouldCallDisposeOnRepositoryAndImageSaver() {
			m_sut.Dispose();
			m_repository.Received(1).Dispose();
		}

		readonly global::BitmapParser.BitmapParser m_sut;
		readonly IBitmapRepository                 m_repository      = Substitute.For<IBitmapRepository>();
		readonly IImageSaver                       m_imageSaver      = Substitute.For<IImageSaver>();
		readonly IParallelOptions                  m_parallelOptions = Substitute.For<IParallelOptions>();
	}
}