package com.dynarithmic.twain;

public class DTwainVersionInfo 
{
	private int majorVersion;
	private int minorVersion;
	private int patchVersion;
	private int versionType;
	private String versionString;
	
	public String toString() { return versionString; }
	
	public DTwainVersionInfo() 
	{
		setMajorVersion(-1);
		setMinorVersion(-1);
		setPatchVersion(-1);
		setVersionType(-1);
		versionString = "Invalid DTWAIN version";
	}
	
	public DTwainVersionInfo(int majorV, int minorV, int patchV, int versionV, String strVer) 
	{
		setMajorVersion(majorV);
		setMinorVersion(minorV);
		setPatchVersion(patchV);
		setVersionType(versionV);
		versionString = strVer;
	}

	public int getMajorVersion() {
		return majorVersion;
	}

	public void setMajorVersion(int majorVersion) {
		this.majorVersion = majorVersion;
	}

	public int getMinorVersion() {
		return minorVersion;
	}

	public void setMinorVersion(int minorVersion) {
		this.minorVersion = minorVersion;
	}

	public int getPatchVersion() {
		return patchVersion;
	}

	public void setPatchVersion(int patchVersion) {
		this.patchVersion = patchVersion;
	}

	public int getVersionType() {
		return versionType;
	}

	public void setVersionType(int versionType) {
		this.versionType = versionType;
	}
}
