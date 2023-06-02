package com.dynarithmic.twain;

public class DTwainPDFOptions
{
    private String sAuthor;
    private String sProducer;
    private String sCreator;
    private String sTitle;
    private String sSubject;
    private String sKeywords;
    private int nOrientation;
    private int nPageSize;
    private double dWidth;
    private double dHeight;
    private double xScale;
    private double yScale;
    private int nScaleOptions;
    private int nSizeOptions;
    private boolean useEncryption;
    private boolean use128bitEncryption;
    private boolean useASCIICompression;
    private int nPermissions;
    private int nJpegQuality;
    private String sUserPass;
    private String sOwnerPass;
    
    public DTwainPDFOptions() {
        sAuthor = "";
        sProducer = "";
        sTitle = "";
        sSubject = "";
        sKeywords = "";
        sCreator = "";
        nOrientation = DTwainJavaAPIConstants.DTWAIN_PDF_PORTRAIT;
        nPageSize = DTwainJavaAPIConstants.DTWAIN_FS_USLETTER;
        dWidth = dHeight = 0.0;
        xScale = yScale = 1.0;
        nScaleOptions = DTwainJavaAPIConstants.DTWAIN_PDF_FITPAGE;
        nSizeOptions = 0;
        useEncryption = false;
        use128bitEncryption = false;
        nJpegQuality = 75;
        sUserPass = "";
        sOwnerPass = "";
        useASCIICompression = false;
    }
    
    public DTwainPDFOptions setASCIICompression(boolean bSet)
    {
        useASCIICompression = bSet;
        return this;
    }
    
    public boolean isASCIICompression()
    {
        return useASCIICompression;
    }
    
    public DTwainPDFOptions setAuthor(String value) {
        sAuthor = value;
        return this;
    }

    public DTwainPDFOptions setProducer(String value) {
        sProducer = value;
        return this;
    }
    
    public DTwainPDFOptions setCreator(String value) {
        sCreator = value;
        return this;
    }
    
    public DTwainPDFOptions setTitle(String value) {
        sTitle = value;
        return this;
    }
    
    public DTwainPDFOptions setSubject(String value) {
        sSubject = value;
        return this;
    }
    public DTwainPDFOptions setKeywords(String value) {
        sKeywords = value;
        return this;
    }

    public DTwainPDFOptions setOrientation(int value) {
        nOrientation = value;
        return this;
    }
    
    public DTwainPDFOptions setPageSize(int value) {
        nPageSize = value;
        return this;
    }
    
    public DTwainPDFOptions setJpegQuality(int value) {
        nJpegQuality = value;
        return this;
    }

   public DTwainPDFOptions setCustomPageSize(double width, 
                                             double height) {
        dWidth = width;
        dHeight = height;
        nSizeOptions = DTwainJavaAPIConstants.DTWAIN_PDF_CUSTOMSIZE;
        return this;
    }
    
   public DTwainPDFOptions setPageScale(int options, 
                       double xscale, double yscale) {
        nScaleOptions = options;
        xScale = xscale;
        yScale = yscale;        
        return this;
    }
      
   public DTwainPDFOptions useEncryption(boolean bSet) {
        useEncryption = bSet;
        return this;
    }
      
   public DTwainPDFOptions setUserPassword(String value) {
        sUserPass = value;
        return this;
    }
      
   public DTwainPDFOptions setOwnerPassword(String value) {
        sOwnerPass = value;
        return this;
    }
    
   public DTwainPDFOptions setPermissions(int value) {
        nPermissions =  value;
        return this;
    }
    
   public DTwainPDFOptions setEncryption128(boolean value) {
        if ( value )
            use128bitEncryption = true;
        else
            use128bitEncryption = false;
        return this;
    }
    
    String getAuthor() { return sAuthor; }
    String getProducer(){ return sProducer; }
    String getTitle() { return sTitle; }
    String getSubject() { return sSubject; }
    String getCreator() { return sCreator; }
    String getKeywords() { return sKeywords; }
    int getOrientation() {return nOrientation; }
    int getPageSize() { return nPageSize; }
    int getPageSizeOptions() { return nSizeOptions; }
    double getCustomPageSizeW() { return dWidth; }
    double getCustomPageSizeH() { return dHeight; }
    double getPageScaleX() { return xScale; }
    double getPageScaleY() { return yScale; }
    int getPageScaleOptions() { return nScaleOptions; }
    int getJpegQuality() { return nJpegQuality; }
    boolean isEncryptionUsed() {return useEncryption; }
    boolean is128EncryptionUsed() { return use128bitEncryption; }
    String getUserPassword() { return sUserPass; }
    String getOwnerPassword() { return sOwnerPass; }
    int getPermissions() { return nPermissions; }
}
