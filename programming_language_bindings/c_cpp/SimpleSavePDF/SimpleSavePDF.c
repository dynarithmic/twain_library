#include "dtwain.h"
#include <stdio.h>

DTWAIN_SOURCE current_source;

// Demonstrate callback mechanism 
LRESULT CALLBACK TwainCallbackProc(WPARAM wParam, LPARAM lParam, LONG_PTR UserData)
{
	// Page count  
	static int pdf_page_count = 1;

	// detect if the notification is due to the page about to be saved
	if (wParam == DTWAIN_TN_FILEPAGESAVING)
	{
		// write text to the bottom left of the PDF page 
		char text[50]; // create the text string
		sprintf(text, "Page %d", pdf_page_count);
		++pdf_page_count;

		// add the text to the page 
		DTWAIN_AddPDFTextA(current_source,
			text,     // text to write on the page 
			100, 100, // (x, y) position 
			"Helvetica", // font to use 
			14, // font height, in PDF points
			DTWAIN_MakeRGB(255, 0, 0), // Red text 
			0, 100.0, 0, 0.0, 0, // scaling, lead, etc. 
			DTWAIN_PDFTEXT_CURRENTPAGE); // flags denoting when to write this text
	}
	return 1;
}

int main()
{
	// initialize and acquire image and save to BMP file 
	if (!DTWAIN_SysInitialize())
		return 0;  // TWAIN could not be initialized 
	current_source = DTWAIN_SelectSource();
	if (current_source)
	{
		// Enable the callback notification/mechanism 
		DTWAIN_EnableMsgNotify(TRUE);
		DTWAIN_SetCallback(TwainCallbackProc, 0);

		// Acquire to a multipage PDF file 
		DTWAIN_AcquireFileA(current_source, "test.pdf", DTWAIN_PDFMULTI, DTWAIN_USENATIVE | DTWAIN_USENAME,
			DTWAIN_PT_DEFAULT, DTWAIN_MAXACQUIRE, TRUE, TRUE, NULL);
	}
	DTWAIN_SysDestroy();
}