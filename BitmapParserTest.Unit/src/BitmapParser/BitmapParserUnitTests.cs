using System.Drawing;
using BitmapParser;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;


namespace BitmapParserTest.Unit.BitmapParser {
	public class BitmapParserUnitTests : IDisposable {
		Bitmap[] MockArrays { get; }

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
			var pathResponse = () => m_sut.GetPath(index);

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
			var pathResponse = () => m_sut.GetPath(1);

			// Assert
			pathResponse.Should()
			            .Throw<NullReferenceException>()
			            .WithMessage(BitmapRepository.EXCEPTION_NULL_PATHS_ARRAY);
		}

		[Theory]
		[InlineData(0, 0.8f)]
		[InlineData(1, 1.5f)]
		public void ScaleBitmapAndSetNew_ShouldScaleAndReplaceBitmap_AtGivenIndex(int index, float scale) {
			var bmpOrigin = new Bitmap(1, 1);
			var bmp       = new Bitmap(1, 1);
			var criteria  = BitmapResizeCriteria.Default();

			m_repository.GetOriginal(index).Returns(bmpOrigin);

			// Act
			m_sut.ScaleBitmapAndSetNew(index, scale, criteria);
			m_repository.GetOriginal(index).Returns(bmp);

			// Assert
			m_repository.Received().Swap(index, Arg.Any<Bitmap>());
		}

		[Fact]
		public void ScaleBitmapAndSetNew_WhenIndexIsValid_CallsSwapOnRepository() {
			// Arrange
			var validIndex  = 1;
			var scale       = 1.0f;
			var criteria    = BitmapResizeCriteria.Default();
			var dummyBitmap = new Bitmap(100, 100);
			m_repository.GetOriginal(validIndex).Returns(dummyBitmap);

			// Act
			m_sut.ScaleBitmapAndSetNew(validIndex, scale, criteria);

			// Assert
			m_repository.Received(1)
			            .Swap(validIndex, Arg.Is<Bitmap>(
				             bmp => Math.Abs(bmp.Width  - dummyBitmap.Width  * scale) < 0.1f &&
				                    Math.Abs(bmp.Height - dummyBitmap.Height * scale) < 0.1f));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		public void SwapBitmaps_SwapsABitmapAtIndex_WhenProvidedWithNewBitmap(int index) {
			var bmp = new Bitmap(1, 1);

			m_sut.SwapBitmaps(index, bmp);

			m_repository.Received(1).Swap(index, bmp);
		}

		[Fact]
		public void SwapBitmaps_ThrowsNullReferenceException_WhenOriginalArrayIsNull() {
			var index = 1;
			var bmp   = new Bitmap(1, 1);
			m_repository.Swap(1, bmp)
			            .Throws(new NullReferenceException(BitmapRepository.EXCEPTION_NULL_PATHS_ARRAY));

			Action swap = () => _ = m_sut.SwapBitmaps(index, bmp);
			swap.Should().Throw<NullReferenceException>()
			    .WithMessage(BitmapRepository.EXCEPTION_NULL_PATHS_ARRAY);

			m_repository.Received(1).Swap(Arg.Any<int>(), Arg.Any<Bitmap>());
		}

		[Fact]
		public void SwapBitmaps_ThrowsIndexOutOfRangeException_WhenOriginalArrayIsNull() {
			var index = -1;
			var bmp   = new Bitmap(1, 1);
			m_repository.Swap(index, bmp)
			            .Throws(new IndexOutOfRangeException(BitmapRepository.EXCEPTION_INDEX_OUT_OF_RANGE));

			Action swap = () => _ = m_sut.SwapBitmaps(index, bmp);
			swap.Should().Throw<IndexOutOfRangeException>()
			    .WithMessage(BitmapRepository.EXCEPTION_INDEX_OUT_OF_RANGE);

			m_repository.Received(1).Swap(-1, Arg.Any<Bitmap>());
		}

		[Theory]
		[InlineData(1,   1,   1)]
		[InlineData(10,  10,  0.5f)]
		[InlineData(100, 100, 2)]
		[InlineData(1,   1,   -1)]
		public void GetNewScaledBitmap_ShouldClampAndEnsureAbsValueOfScale_WhenInputIsValid(
			int width, int height, float scale) {
			const int max                  = global::BitmapParser.BitmapParser.MAX_BITMAP_DIMENSION;
			var       criteria             = BitmapResizeCriteria.Default();
			var       originBmp            = new Bitmap(width, height);
			var       expectedScaledWidth  = Math.Clamp((int)(originBmp.Width  * scale), 1, max);
			var       expectedScaledHeight = Math.Clamp((int)(originBmp.Height * scale), 1, max);
			;

			m_repository.GetOriginal(0).Returns(originBmp);

			var scaledBitmap = m_sut.GetNewScaledBitmap(0, scale, criteria);
			scaledBitmap.Width.Should().Be(expectedScaledWidth);
			scaledBitmap.Height.Should().Be(expectedScaledHeight);
		}

		[Fact]
		public void Dispose_ShouldCallDisposeOnRepositoryAndImageSaver() {
			m_sut.Dispose();
			m_repository.Received(1).Dispose();
			m_sut.IsDisposed.Should().BeTrue();
		}

		[Fact]
		public void ModifyRgbUnsafe_ShouldThrowBitMapParserDisposedException_WhenBitmapParserIsDispose() {
			m_sut.Dispose();
			m_sut.Should().Throws<BitMapParserDisposedException>();
		}

		[Fact]
		public void ModifyRgbUnsafe_ShouldReturnAModifiedBitmap_WhenAppropriateFunctorIsPassed() {
			const int index           = 0;
			var       mockOriginalBmp = new Bitmap(2, 2);
			var       mockModifiedBmp = new Bitmap(1, 1);

			var spy = Substitute.For<global::BitmapParser.BitmapParser.BitmapRgbDelegate>();

			m_parallelOptions.Get.Returns(ParallelOptionsDefault.Get());
			m_repository.GetOriginal(index).Returns(mockOriginalBmp);
			m_repository.Swap(index, Arg.Any<Bitmap>()).Returns(mockModifiedBmp);

			var modifiedBmp =
				m_sut.ModifyRgbUnsafe(0, spy);
			
			spy.ReceivedWithAnyArgs();
			modifiedBmp.Should().NotBeSameAs(mockOriginalBmp);
			modifiedBmp.Should().BeEquivalentTo(mockModifiedBmp);
		}

		[Fact]
		public void ModifyRgbUnsafe_ShouldModifyAllBitmapIndices_WhenValidIndicesAndFunctorArePassed()
		{
			// Arrange
			var bitmapIndices   = new[] {1, 2, 3};
			var mockBitmap      = new Bitmap(1, 1);
			var mockBitmapArray = new Bitmap[bitmapIndices.Length];
			m_repository.GetOriginal(1).Returns(mockBitmap);
			m_repository.GetOriginal(2).Returns(mockBitmap);
			m_repository.GetOriginal(3).Returns(mockBitmap);
			m_parallelOptions.Get.Returns(ParallelOptionsDefault.Get());
			m_repository.Modified.Returns(mockBitmapArray);
			m_repository.Count.Returns(bitmapIndices.Length + 1);
			
			// Act
			var modifiedBitmaps = m_sut.ModifyRgbUnsafe(Functor, bitmapIndices);

			// Assert
			modifiedBitmaps.Should().NotBeNull();
			modifiedBitmaps.Should().HaveCount(bitmapIndices.Length);
			modifiedBitmaps.Should().BeSameAs(mockBitmapArray);
        
			m_repository.Received(bitmapIndices.Length).Swap(Arg.Any<int>(), Arg.Any<Bitmap>());
		}

		[Theory]
		[InlineData(-1, 5)]
		[InlineData(100, 101)]
		public void ModifyRgbUnsafe_ShouldThrowIndexOutOfRangeException_WhenContainsInvalidIndex(int i1, int i2) {
			m_repository.Count.Returns(4);

			Action response = () => m_sut.ModifyRgbUnsafe(Functor, i1, i2);

			response.Should().Throw<IndexOutOfRangeException>()
			        .WithMessage(global::BitmapParser.BitmapParser.EXCEPTION_OUT_OF_RANGE_INDEX_MODIFY_RGB_UNSAFE);
		}

		[Fact]
		public void ModifyAllRgbUnsafeRef_ShouldReturnModified_WhenFunctorIsValid() {
			var bitmapIndices   = new[] {0, 1, 2};
			var mockBitmap      = new Bitmap(1, 1);
			var mockBitmapArray = new Bitmap[bitmapIndices.Length];
			
			m_repository.Count.Returns(bitmapIndices.Length );
			m_repository.GetOriginal(0).Returns(mockBitmap);
			m_repository.GetOriginal(1).Returns(mockBitmap);
			m_repository.GetOriginal(2).Returns(mockBitmap);
			m_parallelOptions.Get.Returns(ParallelOptionsDefault.Get());
			m_repository.Modified.Returns(mockBitmapArray);
			
			// Act
			var modifiedBitmaps = m_sut.ModifyAllRgbUnsafeRef(Functor);

			// Assert
			modifiedBitmaps.Should().NotBeNull();
			modifiedBitmaps.Should().HaveCount(bitmapIndices.Length);
			modifiedBitmaps.Should().BeSameAs(mockBitmapArray);
        
			m_repository.Received(bitmapIndices.Length).Swap(Arg.Any<int>(), Arg.Any<Bitmap>());
		}
		
		static void Functor(ref int index, ref int red, ref int green, ref int blue) {
			red   += 1;
			green += 1;
			blue  += 1;
		}

		readonly global::BitmapParser.BitmapParser m_sut;
		readonly IBitmapRepository                 m_repository      = Substitute.For<IBitmapRepository>();
		readonly IImageSaver                       m_imageSaver      = Substitute.For<IImageSaver>();
		readonly IParallelOptions                  m_parallelOptions = Substitute.For<IParallelOptions>();
	}
}