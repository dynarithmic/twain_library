package com.dynarithmic.twain;
import java.util.ArrayList;
import java.util.TreeMap;

public class DTwainMessageListener {

    private boolean activated;
    private boolean logUnhanledEvents;
    private static ArrayList<DTwainMessageListener> allListeners = new ArrayList<DTwainMessageListener>();
    private static final String eventUnhandled = "Unhandled listener code: ";

    private static final TreeMap<Integer, String> s_mapData = new TreeMap<Integer, String>();
    static
    {
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_ACQUIREDONE, "onAcquireDone()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_ACQUIRESTARTED, "onAcquireStarted()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_UIOPENED, "onUIOpened()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_UICLOSING, "onUIClosing()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_UICLOSED, "onUIClosed()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERREADY, "onTransferReady()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERDONE, "onTransferDone()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERCANCELLED, "onTransferCancelled()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_ACQUIREFAILED, "onAcquireFailed()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_ACQUIRECANCELLED, "onAcquireCancelled()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_PROCESSEDDIB, "onProcessedDib()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_FILESAVECANCELLED, "onFileSaveCancelled()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_FILESAVEOK, "onFileSaveOk()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_FILESAVEERROR, "onFileSaveError()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_FILEPAGESAVING, "onFilePageSaving()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_FILEPAGESAVEOK, "onFilePageSaveOk()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_FILEPAGESAVEERROR, "onFilePageSaveError()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_ACQUIRETERMINATED, "onAcquireTerminated()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_PAGECONTINUE, "onPageContinue()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_PAGECANCELLED, "onPageCancelled()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_PAGEDISCARDED, "onPageDiscarded()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_PAGEFAILED, "onPageFailed()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_QUERYPAGEDISCARD, "onQueryPageDiscard()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERSTRIPREADY, "onTransferStripReady()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERSTRIPDONE, "onTransferStripDone()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERSTRIPFAILED, "onTransferStripFailed()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_IMAGEINFOERROR, "onImageInfoError()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_CLIPTRANSFERDONE, "onClipTransferDone()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_PROCESSEDDIBFINAL, "onProcessedDibFinal()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_EOJDETECTED, "onEOJDetected()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_TWAINPAGECANCELLED, "onTwainPageCancelled()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_TWAINPAGEFAILED, "onTwainPageFailed()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_EOJDETECTED_XFERDONE, "onEOJDetectedEndTransfer()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_DEVICEEVENT, "onDeviceEvent()");
        s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_CROPFAILED, "onCropImageFailedEvent()"); 
		s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDETECTED1, "onBlankPageDetectedEvent1()"); 
		s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDETECTED2, "onBlankPageDetectedEvent2()");                                  
		s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDETECTED3, "onBlankPageDetectedEvent3()");                                  
		s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDISCARDED1, "onBlankPageDiscardedEvent1()");                                   
		s_mapData.put(DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDISCARDED2, "onBlankPageDiscardedEvent2()");                                  
   }

    public DTwainMessageListener() {
    	allListeners.add(this);
    	activated = false;
        logUnhanledEvents = false;
    }

    public void logUnhandledEvents(boolean bLog) 
    { logUnhanledEvents = bLog; }
    
    public boolean isLogUnhandledEventsOn() 
    { return logUnhanledEvents; }
    
    public void activate()
    { activated = true; }

    public void deactivate()
    { activated = false; }

    public boolean isActivated()
    { return activated; }

    protected void finalize()
    { removeListener(); }

    public void removeListener()
    {
    	int nSize = allListeners.size();
    	for (int i = 0; i < nSize; ++i )
    	{
            DTwainMessageListener listener = (DTwainMessageListener)allListeners.get(i);
            if (listener.hashCode() == this.hashCode())
            {
                    allListeners.remove(i);
                    break;
            }
    	}
    }

    public static int onTwainEvent(int event, long sourceHandle)
    {
        // userdata is the handle to the AcquireEngine
        // use map to find the AcquireEngine
        // Once found, write loop to call all listeners added to the engine
        int nListeners = allListeners.size();
        int returner = 1;

        for ( int i = 0; i < nListeners; ++i )
        {
            DTwainMessageListener theListener = (DTwainMessageListener)allListeners.get(i);
            if (theListener.activated)
            {
                switch (event)
                {
                    case DTwainJavaAPIConstants.DTWAIN_TN_ACQUIREDONE:
                            returner = theListener.onAcquireDone(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_ACQUIRESTARTED:
                            returner = theListener.onAcquireStarted(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_UIOPENED:
                            returner = theListener.onUIOpened(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_UICLOSING:
                            returner = theListener.onUIClosing(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_UICLOSED:
                            returner = theListener.onUIClosed(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERREADY:
                            returner = theListener.onTransferReady(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERDONE:
                            returner = theListener.onTransferDone(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERCANCELLED:
                            returner = theListener.onTransferCancelled(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_ACQUIREFAILED:
                            returner = theListener.onAcquireFailed(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_ACQUIRECANCELLED:
                            returner = theListener.onAcquireCancelled(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_PROCESSEDDIB:
                            returner = theListener.onProcessedDib(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_FILESAVECANCELLED:
                            returner = theListener.onFileSaveCancelled(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_FILESAVEOK:
                            returner = theListener.onFileSaveOk(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_FILESAVEERROR:
                            returner = theListener.onFileSaveError(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_FILEPAGESAVEOK:
                            returner = theListener.onFilePageSaveOk(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_FILEPAGESAVING:
                            returner = theListener.onFilePageSaving(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_FILEPAGESAVEERROR:
                            returner = theListener.onFilePageSaveError(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_ACQUIRETERMINATED:
                            returner = theListener.onAcquireTerminated(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_PAGECONTINUE:
                            returner = theListener.onPageContinue(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_QUERYPAGEDISCARD:
                            returner = theListener.onQueryPageDiscard(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_PAGEDISCARDED:
                            returner = theListener.onPageDiscarded(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_PAGECANCELLED:
                            returner = theListener.onPageCancelled(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_PAGEFAILED:
                            returner = theListener.onPageFailed(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERSTRIPREADY:
                            returner = theListener.onTransferStripReady(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERSTRIPDONE:
                            returner = theListener.onTransferStripDone(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_TRANSFERSTRIPFAILED:
                            returner = theListener.onTransferStripFailed(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_IMAGEINFOERROR:
                            returner = theListener.onImageInfoError(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_CLIPTRANSFERDONE:
                            returner = theListener.onClipTransferDone(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_PROCESSEDDIBFINAL:
                            returner = theListener.onProcessedDibFinal(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_EOJDETECTED:
                            returner = theListener.onEOJDetected(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_EOJDETECTED_XFERDONE:
                            returner = theListener.onEOJDetectedEndTransfer(event, sourceHandle);
                    break;
                    case DTwainJavaAPIConstants.DTWAIN_TN_TWAINPAGECANCELLED:
                            returner = theListener.onTwainPageCancelled(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_TWAINPAGEFAILED:
                            returner = theListener.onTwainPageFailed(event, sourceHandle);
                    break;

                    case DTwainJavaAPIConstants.DTWAIN_TN_DEVICEEVENT:
                            returner = theListener.onDeviceEvent(event, sourceHandle);
                    break;
    
                    case DTwainJavaAPIConstants.DTWAIN_TN_CROPFAILED:
                            returner = theListener.onCropImageFailedEvent(event, sourceHandle);
                    break;
                        
                    case DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDETECTED1:
                            returner = theListener.onBlankPageDetectedEvent1(event, sourceHandle);
                    break;
                        
                    case DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDETECTED2:
                            returner = theListener.onBlankPageDetectedEvent2(event, sourceHandle);
                    break;
                        
                    case DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDETECTED3:
                            returner = theListener.onBlankPageDetectedEvent3(event, sourceHandle);
                    break;
                        
                    case DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDISCARDED1:
                            returner = theListener.onBlankPageDiscardedEvent1(event, sourceHandle);
                    break;
                        
                    case DTwainJavaAPIConstants.DTWAIN_TN_BLANKPAGEDISCARDED2:
                            returner = theListener.onBlankPageDiscardedEvent2(event, sourceHandle);
                    break;
                }
            }
        }
        return returner;
    }


    private int defaultImpl(int curEvent)
    {
        if ( logUnhanledEvents  )
        {
            String s = (String)s_mapData.get(new Integer(curEvent));
            if ( s == null )
               s = "";
            System.out.println(eventUnhandled + curEvent + "  " + s);
        }
        return 1;
    }

    public int onAcquireStarted(int event, long sourceHandle) { return defaultImpl(event); }
    public int onAcquireDone(int event, long sourceHandle)  { return defaultImpl(event); }
    public int onUIOpened(int event, long sourceHandle)     { return defaultImpl(event); }
    public int onUIClosing(int event, long sourceHandle)    { return defaultImpl(event); }
    public int onUIClosed(int event, long sourceHandle)     { return defaultImpl(event); }
    public int onTransferReady(int event, long sourceHandle)  { return defaultImpl(event); }
    public int onTransferDone(int event, long sourceHandle)  { return defaultImpl(event); }
    public int onTransferCancelled(int event, long sourceHandle) { return defaultImpl(event); }
    public int onInvalidImageFormat(int event, long sourceHandle)  { return defaultImpl(event); }
    public int onAcquireFailed(int event, long sourceHandle) { return defaultImpl(event); }
    public int onAcquireCancelled(int event, long sourceHandle) { return defaultImpl(event); }
    public int onProcessedDib(int event, long sourceHandle)  { return defaultImpl(event); }
    public int onFileSaveCancelled(int event, long sourceHandle) { return defaultImpl(event); }
    public int onFileSaveOk(int event, long sourceHandle) { return defaultImpl(event); }
    public int onFileSaveError(int event, long sourceHandle) { return defaultImpl(event); }
    public int onFilePageSaveOk(int event, long sourceHandle) { return defaultImpl(event); }
    public int onFilePageSaveError(int event, long sourceHandle) { return defaultImpl(event); }
    public int onAcquireTerminated(int event, long sourceHandle) { return defaultImpl(event); }
    public int onPageContinue(int event, long sourceHandle) { return defaultImpl(event); }
    public int onQueryPageDiscard(int event, long sourceHandle) { return defaultImpl(event); }
    public int onFilePageSaving(int event, long sourceHandle) { return defaultImpl(event); }
    public int onPageCancelled(int event, long sourceHandle) { return defaultImpl(event); }
    public int onPageFailed(int event, long sourceHandle) { return defaultImpl(event); }
    public int onPageDiscarded(int event, long sourceHandle) { return defaultImpl(event); }
    public int onTransferStripReady(int event, long sourceHandle) { return defaultImpl(event); }
    public int onTransferStripDone(int event, long sourceHandle) { return defaultImpl(event); }
    public int onTransferStripFailed(int event, long sourceHandle) { return defaultImpl(event); }
    public int onImageInfoError(int event, long sourceHandle) { return defaultImpl(event); }
    public int onClipTransferDone(int event, long sourceHandle) { return defaultImpl(event); }
    public int onProcessedDibFinal(int event, long sourceHandle) { return defaultImpl(event); }
    public int onEOJDetected(int event, long sourceHandle) { return defaultImpl(event); }
    public int onEOJDetectedEndTransfer(int event, long sourceHandle) { return defaultImpl(event); }
    public int onTwainPageCancelled(int event, long sourceHandle) { return defaultImpl(event); }
    public int onTwainPageFailed(int event, long sourceHandle) { return defaultImpl(event); }
    public int onDeviceEvent(int event, long sourceHandle) { return defaultImpl(event); }
    public int onCropImageFailedEvent(int event, long sourceHandle) { return defaultImpl(event); }
    public int onBlankPageDetectedEvent1(int event, long sourceHandle) { return defaultImpl(event); }
    public int onBlankPageDetectedEvent2(int event, long sourceHandle) { return defaultImpl(event); }
    public int onBlankPageDetectedEvent3(int event, long sourceHandle) { return defaultImpl(event); }
    public int onBlankPageDiscardedEvent1(int event, long sourceHandle) { return defaultImpl(event); }
    public int onBlankPageDiscardedEvent2(int event, long sourceHandle) { return defaultImpl(event); }
}
