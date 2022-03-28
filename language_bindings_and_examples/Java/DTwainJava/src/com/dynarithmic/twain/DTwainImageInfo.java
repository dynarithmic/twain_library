package com.dynarithmic.twain;

public class DTwainImageInfo 
{
	public DTwainImageInfo(double xResolution, double yResolution,
			int imageWidth, int imageLength, int samplesPerPixel,
			int[] bitsPerSample, int bitsPerPixel, int planar, int pixelType,
			int compression) 
	{
		super();
		XResolution = xResolution;
		YResolution = yResolution;
		ImageWidth = imageWidth;
		ImageLength = imageLength;
		SamplesPerPixel = samplesPerPixel;
		BitsPerSample = bitsPerSample;
		BitsPerPixel = bitsPerPixel;
		Planar = planar;
		PixelType = pixelType;
		Compression = compression;
	}

	public DTwainImageInfo() 
	{
		super();
		XResolution = -1;
		YResolution = -1;
		ImageWidth = 0;
		ImageLength = 0;
		SamplesPerPixel = 0;
		BitsPerPixel = 0;
		Planar = 0;
		PixelType = -1;
		Compression = 0;
	}

	private double XResolution;
	private double YResolution;
	private int    ImageWidth;
	private int ImageLength;
	private int SamplesPerPixel;
	private int [] BitsPerSample = new int[8];
	private int BitsPerPixel;
	private int Planar;
	private int PixelType;
	private int Compression;
	
	public boolean isValid() 
	{ 
		if ( BitsPerPixel==0 || PixelType == -1 || ImageWidth == 0 || ImageLength == 0 )
			return false;
		return true;
	}
	
	public int getPlanar() {
		return Planar;
	}
	
	public void setPlanar(int planar) {
		Planar = planar;
	}
	
	public int getBitsPerPixel() {
		return BitsPerPixel;
	}
	public int[] getBitsPerSample() {
		return BitsPerSample;
	}
	public int getCompression() {
		return Compression;
	}
	public int getImageLength() {
		return ImageLength;
	}
	public int getImageWidth() {
		return ImageWidth;
	}
	public int getPixelType() {
		return PixelType;
	}
	public int getSamplesPerPixel() {
		return SamplesPerPixel;
	}
	public double getXResolution() {
		return XResolution;
	}
	public double getYResolution() {
		return YResolution;
	}

	public void setXResolution(double xResolution) {
		XResolution = xResolution;
	}

	public void setYResolution(double yResolution) {
		YResolution = yResolution;
	}

	public void setImageWidth(int imageWidth) {
		ImageWidth = imageWidth;
	}

	public void setImageLength(int imageLength) {
		ImageLength = imageLength;
	}

	public void setSamplesPerPixel(int samplesPerPixel) {
		SamplesPerPixel = samplesPerPixel;
	}

	public void setBitsPerSample(int[] bitsPerSample) {
		BitsPerSample = bitsPerSample;
	}

	public void setBitsPerPixel(int bitsPerPixel) {
		BitsPerPixel = bitsPerPixel;
	}

	public void setPixelType(int pixelType) {
		PixelType = pixelType;
	}

	public void setCompression(int compression) {
		Compression = compression;
	}
}