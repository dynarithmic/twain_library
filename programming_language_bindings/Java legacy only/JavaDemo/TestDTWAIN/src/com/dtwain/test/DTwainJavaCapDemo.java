package com.dtwain.test;
import com.dynarithmic.twain.*;

public class DTwainJavaCapDemo {

private DTwainJavaAPI twainInterface=null;
private DTwainSource twainSource = null;

public void runDemo() {
        // Create DTwainInterface instance 
        twainInterface = new DTwainJavaAPI();
        try {
            // Initialize the interface by creating a TWAIN hot-link
            twainInterface.DTWAIN_JavaSysInitialize();
            
            // set up a logger
            DTwainLogger theLogger = new DTwainLogger();
            theLogger.startLogger(twainInterface);

            // Now that TWAIN hot-link is created, select a device using 
            // the Swing version of the Select Source dialog
            twainSource = new DTwainSource(twainInterface.DTWAIN_SelectSource(), twainInterface);

            // Now check if user chose a device or cancelled. 
            if ( twainSource.isValid() ) {
                String unit[] = {"Dots per Inch", "Dots per centimeter", "Picas", "Points", "TWIPS", "Pixels"};

                // turn on range expansion, just in case cap values returned are stored in a range
                twainSource.setExpandRanges( true );

                // Test if X-RESOULUTION is supported
                if ( twainSource.isCapSupported(DTwainJavaAPIConstants.DTWAIN_CV_ICAPXRESOLUTION) ) {

                    // X-resolution supported, let's see what the available values are for this device
                    System.out.println( "The X resolution values are as follows:" );

                    // Now get the values
                    double [] xres = twainSource.getAllXResolutions();

                    // Now get the unit setting.  This must be supported by all devices, however we
                    // will check it for support using exception handling instead of explicitly calling
                    // a function and testing the return value (as we did with the X-Resolution).

                    int currentUnit = 0;
                    try {
                        // Get the current unit of Measure
                        currentUnit = twainSource.getCurrentUnits();
                    }
                    catch (DTwainRuntimeException capFail) {
                        System.out.println(capFail.getMessage() );
                    }

                    // Output each value
                    for ( int i = 0; i < xres.length; ++i )
                        System.out.println( xres[i] + " " + unit[currentUnit]);

                    // Now set the resolution to first value.
                    if ( !twainSource.setXResolution( xres[0] ) )
                        System.out.println("Setting resolution failed for this value: " + xres[0]);
                    else
                        System.out.println("Setting resolution successful for this value: " + xres[0]);
                    
                    // end the TWAIN session    
                    twainInterface.DTWAIN_JavaSysDestroy();
                }
            }
        }
        catch (Exception e1) {
            System.out.println(e1.getMessage());
    }
}

public static void main(String[] args) {
        DTwainJavaCapDemo simpleProg = new DTwainJavaCapDemo();
        simpleProg.runDemo();
    }
}