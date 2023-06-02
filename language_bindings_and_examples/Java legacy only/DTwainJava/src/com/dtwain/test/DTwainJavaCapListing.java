package com.dtwain.test;
import com.dynarithmic.twain.*;

public class DTwainJavaCapListing {

private DTwainJavaAPI twainInterface=null;
private DTwainSource twainSource = null;

private void listCaps() {
   System.out.println();

   // get all the supported capabilities of the device
   int [] theCaps = twainSource.getSupportedCaps();

   // for each one, output the name of the capability along with the value (in parentheses)
   for (int i = 0; i < theCaps.length; ++i ) 
      System.out.println(DTwainCapabilityHandler.toString(theCaps[i]) + " (" + theCaps[i] + ")");
}

public void runDemo() {
        // Create DTwainJavaAPI instance first.  
        try {
			twainInterface = new DTwainJavaAPI(DTwainJavaAPI.JNIDLLName_64U);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
        try 
        {
            // Initialize the interface 
            twainInterface.DTWAIN_JavaSysInitialize();

            // Select a TWAIN device and crDTwainJavaAPISourcenSource helper object using the returned
            // TWAIN Source (which is a long type), and the interface we initialized above.
            twainSource = new DTwainSource(twainInterface.DTWAIN_SelectSource(), twainInterface);

            // Now check if user chose a device or canceled. 
            if ( twainSource.isValid() ) {
                // list the caps
                listCaps();
            }
            twainInterface.DTWAIN_JavaSysDestroy();
        }
        catch (Exception e1) 
        {
            System.out.println(e1.getMessage());
        }
}

public static void main(String[] args) {

        DTwainJavaCapListing simpleProg = new DTwainJavaCapListing();
        simpleProg.runDemo();

        // must be called, since TWAIN dialog is a Swing component
        // and AWT thread was started
        System.exit(0);
    }
}