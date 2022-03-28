package com.dynarithmic.twain;
public class DTwainAcquireType
{
    // Constants
    public static final int ACQUIRETYPE_ACQUIREINVALID = -1;
    public static final int ACQUIRETYPE_NATIVE = 1;
    public static final int ACQUIRETYPE_BUFFERED = 2;
    public static final int ACQUIRETYPE_FILE = 3;
    public static final int ACQUIRETYPE_NATIVEFILE = DTwainJavaAPIConstants.DTWAIN_USENATIVE;
    public static final int ACQUIRETYPE_BUFFEREDFILE= DTwainJavaAPIConstants.DTWAIN_USEBUFFERED;
    public static final int ACQUIRETYPE_NATIVECLIPBOARD = 6;
    public static final int ACQUIRETYPE_BUFFEREDCLIPBOARD = 7;
}
