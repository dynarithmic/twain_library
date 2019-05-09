package com.dynarithmic.twain;

public final class DTwainAcquireParams
{
	public static final int MAX_PAGES = DTwainJavaAPIConstants.DTWAIN_MAXACQUIRE;
    private int fileType;
    private int maxPages;
    private boolean useUI;
    private boolean useProgress;
    private boolean useMultiPage;
    private int colorType;
    private int paperSize;
    private int acquireType;
    private String tempDir;
    private int compressionType;
    private boolean scaleImages;
    private double scaleX;
    private double scaleY;
    private boolean manualDuplexMode;
    private int duplexType;
    private int multiPageMode;
    private int maxAcquisitions;
    private boolean enableFeeder;
    private boolean enableDuplex;
    private int fileIncrementValue;
    private int multipageModeType;
    private boolean closeSource;
    private boolean useSourceMode;
    private int memoryImageType;
    private boolean attachBMPHeader;
    private int jpegQuality;
    
    public class DTwainPDFOptions
    {
        private String sAuthor;
        private String sProducer;
        private String sTitle;
        private String sSubject;
        private String sKeywords;
        private String sCreator;
        private int nOrientation;
        private int nPageSize;
        private double dWidth;
        private double dHeight;
        private double xScale;
        private double yScale;
        private int nScaleOptions;
        private int nSizeOptions;
        private boolean useEncryption;
        private boolean useA85Compression;
        private boolean use128bitEncryption;
        private int nPermissions;
        private int nJpegQuality;
        private String sUserPass;
        private String sOwnerPass;
        {
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
            useA85Compression = false;
            nJpegQuality = 75;
            sUserPass = "";
            sOwnerPass = "";
        }
        
        public DTwainPDFOptions setAuthor(final String value) {
            sAuthor = value;
            return this;
        }

        public DTwainPDFOptions setCreator(final String value) {
            sCreator = value;
            return this;
        }
        
        public DTwainPDFOptions setProducer(final String value) {
            sProducer = value;
            return this;
        }
        public DTwainPDFOptions setTitle(final String value) {
            sTitle = value;
            return this;
        }
        
        public DTwainPDFOptions setSubject(final String value) {
            sSubject = value;
            return this;
        }
        public DTwainPDFOptions setKeywords(final String value) {
            sKeywords = value;
            return this;
        }

        public DTwainPDFOptions setOrientation(final int value) {
            nOrientation = value;
            return this;
        }
        
        public DTwainPDFOptions setPageSize(final int value) {
            nPageSize = value;
            return this;
        }
        
        public DTwainPDFOptions setJpegQuality(final int value) {
            nJpegQuality = value;
            return this;
        }
        
        public DTwainPDFOptions setA85Compression(final boolean value) {
        	useA85Compression = value;
        	return this;
        }

       public DTwainPDFOptions setCustomPageSize(final double width, 
                                                 final double height) {
            dWidth = width;
            dHeight = height;
            nSizeOptions = DTwainJavaAPIConstants.DTWAIN_PDF_CUSTOMSIZE;
            return this;
        }
        
       public DTwainPDFOptions setPageScale(final int options, 
                           final double xscale, final double yscale) {
            nScaleOptions = options;
            xScale = xscale;
            yScale = yscale;        
            return this;
        }
          
       public DTwainPDFOptions setUseEncryption(final boolean bSet) {
            useEncryption = bSet;
            return this;
        }
          
       public DTwainPDFOptions setUserPassword(final String value) {
            sUserPass = value;
            return this;
        }
          
       public DTwainPDFOptions setOwnerPassword(final String value) {
            sOwnerPass = value;
            return this;
        }
        
       public DTwainPDFOptions setPermissions(final int value) {
            nPermissions =  value;
            return this;
        }
        
       public DTwainPDFOptions setUseEncryption128(final boolean value) {
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
        String getKeywords() { return sKeywords; }
        String getCreator() { return sCreator; }
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
        boolean isA85CompressionUsed() { return useA85Compression; }
        String getUserPassword() { return sUserPass; }
        String getOwnerPassword() { return sOwnerPass; }
        int getPermissions() { return nPermissions; }    	
    }

    public class DTwainJobControlOptions {
    	boolean useJobControl;
    	int jobControlOption;
    	boolean useAutoJobFileHandling;
    	
    	public DTwainJobControlOptions() {
    		jobControlOption = DTwainJavaAPIConstants.DTWAIN_JC_NONE;
    	}
		public int getJobControlOption() {
			return jobControlOption;
		}
		public boolean isUseAutoJobFileHandling() {
			return useAutoJobFileHandling;
		}
		public boolean isUseJobControl() {
			return useJobControl;
		}
		public void setJobControlOption(int jobControlOption) {
			this.jobControlOption = jobControlOption;
		}
		public void setUseAutoJobFileHandling(boolean useAutoJobFileHandling) {
			this.useAutoJobFileHandling = useAutoJobFileHandling;
		}
		public void setUseJobControl(boolean useJobControl) {
			this.useJobControl = useJobControl;
		}
    }
    

    public DTwainPDFOptions PDFOptions=new DTwainAcquireParams.DTwainPDFOptions();
    public DTwainJobControlOptions JobControlOptions = new DTwainAcquireParams.DTwainJobControlOptions();
    
    public DTwainAcquireParams()
    {
        acquireType = DTwainAcquireType.ACQUIRETYPE_NATIVEFILE;
        fileType = DTwainFileType.FILETYPE_BMP;
        maxPages = 1;
        useUI = true;
        useProgress = true;
        useMultiPage = true;
        colorType = DTwainJavaAPIConstants.DTWAIN_PT_DEFAULT;
        paperSize = DTwainPaperSize.PAGESIZE_CURRENT;
        tempDir = "";
        closeSource = true;
        scaleImages = false;
        scaleX = 0.1;
        scaleY = 0.1;
        manualDuplexMode = false;
        duplexType = DTwainJavaAPIConstants.DTWAIN_MANDUP_FACEDOWNBOTTOMPAGE;
        maxAcquisitions = DTwainJavaAPIConstants.DTWAIN_MAXACQUIRE;
        closeSource = false;
        enableDuplex = false;
        multipageModeType = 
        fileIncrementValue = 0;	
        compressionType = DTwainCompressionType.COMPRESS_NONE;
        useSourceMode = false;
        memoryImageType = DTwainJavaAPIConstants.DTWAIN_BMP;
        attachBMPHeader = true;
        jpegQuality = 75;
    }

    public final int getAcquireType() {
        return acquireType;
    }

    public DTwainAcquireParams setAcquireType(final int acquireType) {
        this.acquireType = acquireType;
        return this;
    }

    public boolean isCloseSource() {
        return closeSource;
    }

    public DTwainAcquireParams setCloseSource(final boolean closeSource) {
        this.closeSource = closeSource;
        return this;
    }

    public int getColorType() {
        return colorType;
    }

    public DTwainAcquireParams setColorType(final int colorType) {
        this.colorType = colorType;
        return this;
    }

    public int getCompressionType() {
        return compressionType;
    }

    public DTwainAcquireParams setCompressionType(final int compressionType) {
        this.compressionType = compressionType;
        return this;
    }

    public int getDuplexType() {
        return duplexType;
    }

    public DTwainAcquireParams setDuplexType(final int duplexType) {
        this.duplexType = duplexType;
        return this;
    }

    public boolean isEnableDuplex() {
        return enableDuplex;
    }

    public DTwainAcquireParams setEnableDuplex(final boolean enableDuplex) {
        this.enableDuplex = enableDuplex;
        return this;
    }

    public boolean isEnableFeeder() {
        return enableFeeder;
    }

    public DTwainAcquireParams setEnableFeeder(final boolean enableFeeder) {
        this.enableFeeder = enableFeeder;
        return this;
    }

    public int getFileIncrementValue() {
        return fileIncrementValue;
    }

    public DTwainAcquireParams setFileIncrementValue(final int fileIncrementValue) {
        this.fileIncrementValue = fileIncrementValue;
        return this;
    }

    public boolean isManualDuplexMode() {
        return manualDuplexMode;
    }

    public DTwainAcquireParams setManualDuplexMode(final boolean manualDuplexMode) {
        this.manualDuplexMode = manualDuplexMode;
        return this;
    }

    public int getMaxAcquisitions() {
        return maxAcquisitions;
    }

    public DTwainAcquireParams setMaxAcquisitions(final int maxAcquisitions) {
        this.maxAcquisitions = maxAcquisitions;
        return this;
    }

    public int getMaxPages() {
        return maxPages;
    }

    public DTwainAcquireParams setMaxPages(final int maxPages) {
        this.maxPages = maxPages;
        return this;
    }

    public int getMemoryImageType() {
        return memoryImageType;
    }

    public DTwainAcquireParams setMemoryImageType(final int memoryImageType) {
        this.memoryImageType = memoryImageType;
        return this;
    }

    public int getMultiPageMode() {
        return multiPageMode;
    }

    public DTwainAcquireParams setMultiPageMode(final int multiPageMode) {
        this.multiPageMode = multiPageMode;
        return this;
    }

    public int getMultipageModeType() {
        return multipageModeType;
    }

    public DTwainAcquireParams setMultipageModeType(final int multipageModeType) {
        this.multipageModeType = multipageModeType;
        return this;
    }

    public int getPaperSize() {
        return paperSize;
    }

    public DTwainAcquireParams setPaperSize(final int paperSize) {
        this.paperSize = paperSize;
        return this;
    }

    public boolean isScaleImages() {
        return scaleImages;
    }

    public DTwainAcquireParams setScaleImages(final boolean scaleImages) {
        this.scaleImages = scaleImages;
        return this;
    }

    public double getScaleX() {
        return scaleX;
    }

    public DTwainAcquireParams setScaleX(final double scaleX) {
        this.scaleX = scaleX;
        return this;
    }

    public double getScaleY() {
        return scaleY;
    }

    public DTwainAcquireParams setScaleY(final double scaleY) {
        this.scaleY = scaleY;
        return this;
    }

    public String getTempDir() {
        return tempDir;
    }

    public DTwainAcquireParams setTempDir(final String tempDir) {
        this.tempDir = tempDir;
        return this;
    }
    
    public boolean isUseMultiPage() {
        return useMultiPage;
    }

    public DTwainAcquireParams setUseMultiPage(final boolean useMultiPage) {
        this.useMultiPage = useMultiPage;
        return this;
    }

    public boolean isUseProgress() {
        return useProgress;
    }

    public DTwainAcquireParams setUseProgress(final boolean useProgress) {
        this.useProgress = useProgress;
        return this;
    }

    public boolean isUseDeviceMode() {
        return useSourceMode;
    }

    public DTwainAcquireParams setUseDeviceMode(final boolean useSourceMode) {
        this.useSourceMode = useSourceMode;
        return this;
    }

    public boolean isUseUI() {
        return useUI;
    }

    public DTwainAcquireParams setUseUI(final boolean useUI) {
        this.useUI = useUI;
        return this;
    }

    public boolean isattachBMPHeader() {
        return attachBMPHeader;
    }

    public DTwainAcquireParams setattachBMPHeader(final boolean attachBMPHeader) {
        this.attachBMPHeader = attachBMPHeader;
        return this;
    }

    public DTwainAcquireParams setFileType(final int filetype) {
        this.fileType = filetype;
        return this;
    }

    public int getFileType() {
        return fileType;
    }

	public int getJpegQuality() {
		return jpegQuality;
	}

	public DTwainAcquireParams setJpegQuality(final int jpegQuality) {
		this.jpegQuality = jpegQuality;
		return this;
	}
}
