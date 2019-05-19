/* --- Script © 2005-2010 EC Software --- */
var ua = navigator.userAgent;
var dom = (document.getElementById) ? true : false;
var ie4 = (document.all && !dom) ? true : false;
var ie5_5 = ((ua.indexOf("MSIE 5.5")>=0 || ua.indexOf("MSIE 6")>=0) && ua.indexOf("Opera")<0) ? true : false;
var ns4 = (document.layers && !dom) ? true : false;
var offsxy = 6;
function hmshowPopup(e, txt, stick) {
  var tip = '<table  cellpadding="6" cellspacing="0" style="background-color:#FFFFFF; border:1px solid #000000; border-collapse:collapse;"><tr valign=top><td>'+ txt + '<\/td><\/tr><\/table>';
  var tooltip = atooltip();
  e = e?e:window.event;
  var mx = ns4 ? e.PageX : e.clientX;
  var my = ns4 ? e.PageY : e.clientY;
  var obj   = (window.document.compatMode && window.document.compatMode == "CSS1Compat") ? window.document.documentElement : window.document.body;
  var bodyl = (window.pageXOffset) ? window.pageXOffset : obj.scrollLeft;
  var bodyt = (window.pageYOffset) ? window.pageYOffset : obj.scrollTop;
  var bodyw = (window.innerWidth)  ? window.innerWidth  : obj.offsetWidth;
  if (ns4) {
    tooltip.document.write(tip);
    tooltip.document.close();
    if ((mx + offsxy + bodyl + tooltip.width) > bodyw) { mx = bodyw - offsxy - bodyl - tooltip.width; if (mx < 0) mx = 0; }
    tooltip.left = mx + offsxy + bodyl;
    tooltip.top = my + offsxy + bodyt;
  }
  else {
    tooltip.innerHTML = tip;
    if (tooltip.offsetWidth) if ((mx + offsxy + bodyl + tooltip.offsetWidth) > bodyw) { mx = bodyw - offsxy - bodyl - tooltip.offsetWidth; if (mx < 0) mx = 0; }
    tooltip.style.left = (mx + offsxy + bodyl)+"px";
    tooltip.style.top  = (my + offsxy + bodyt)+"px";
  }
  with(tooltip) { ns4 ? visibility="show" : style.visibility="visible" }
  if (stick) document.onmouseup = hmhidePopup;
}
function hmhidePopup() {
  var tooltip = atooltip();
  ns4 ? tooltip.visibility="hide" : tooltip.style.visibility="hidden";
}
function atooltip(){
 return ns4 ? document.hmpopupDiv : ie4 ? document.all.hmpopupDiv : document.getElementById('hmpopupDiv')
}
popid_831869247="<p>In <span style=\"font-weight: bold;\">State 6<\/span>, the Source is starting or ending the image transfer from the device to DTWAIN. &nbsp;If the transfer has ended successfully, the image data is stored as one or more <a href=\"indmp4l.htm\">Device Independent Bitmaps<\/a> (DIBs). Or as a <a href=\"bufferedtransfer.htm\">compressed image<\/a><\/p>\n\r"
popid_988312399="<p>The <span style=\"font-weight: bold;\">TW_RANGE <\/span>is a TWAIN \'C\' language structure that handles ranges of values. &nbsp;The <span style=\"font-weight: bold;\">DTWAIN_RANGE<\/span> data type emulates a TW_RANGE so that other languages can use the structure easily.<\/p>\n\r"
popid_1530542323X="<p>In <span style=\"font-weight: bold;\">State 5<\/span>, DTWAIN enables or disables the TWAIN Source user interface, or if no user interface, ready to acquire an image.<\/p>\n\r"
popid_1491286190X="<p>In <span style=\"font-weight: bold;\">State 1<\/span>, DTWAIN has not loaded the TWAIN Source Manager.  <\/p>\n\r"
popid_995107139X="<p>In <span style=\"font-weight: bold;\">State 4<\/span>, DTWAIN has opened or closed a TWAIN Source.<\/p>\n\r"
popid_853827246="<p><span style=\"font-weight: bold;\">DTWAIN_FAILURE1<\/span> is a 32-bit value equal to <span style=\"font-weight: bold;\">-1<\/span> (0xFFFFFFFF in hexadecimal).<\/p>\n\r"
popid_853827247="<p><span style=\"font-weight: bold;\">DTWAIN_FAILURE2<\/span> is a 32-bit value equal to <span style=\"font-weight: bold;\">-2<\/span> (0xFFFFFFFE in hexadecimal).<\/p>\n\r"
popid_655131249X="<p>A <span style=\"font-weight: bold;\">DTWAIN_FRAME <\/span>is a 32-bit value that emulates a TWAIN <span style=\"font-weight: bold;\">TW_FRAME<\/span>.structure type.  <\/p>\n\r"
popid_76618539="<p>The <span style=\"font-weight: bold;\">FALSE<\/span> value is equal to <span style=\"font-weight: bold;\">0.<\/span><\/p>\n\r"
popid_935026813X="<p>In <span style=\"font-weight: bold;\">State 3<\/span>, DTWAIN has opened or closed the TWAIN Source Manager.<\/p>\n\r"
popid_1665431485X="<p>In <span style=\"font-weight: bold;\">State 2<\/span>, DTWAIN has loaded or unloaded the TWAIN Source Manager.<\/p>\n\r<p><span style=\"color: #000000;\">&nbsp;<\/span><\/p>\n\r"
popid_1591487050X="<p>In <span style=\"font-weight: bold;\">State 7<\/span>, the image data is transferring from the image device to the application.<\/p>\n\r"
popid_797503151X="<p><span style=\"font-weight: bold; color: #010100;\">DTWAIN_MAXACQUIRE<\/span><span style=\"color: #010100;\"> is a 32-bit value equal to <\/span><span style=\"font-weight: bold; color: #010100;\">-1<\/span><span style=\"color: #010100;\"> (0xFFFFFFFF in hexadecimal).<\/span><\/p>\n\r"
popid_1003832297X="<p><span class=\"f_Heading1\">Windows API function<\/span><\/p>\n\r<p>This is a Windows API function. &nbsp;Consult your Windows API reference for more information on this function.<\/p>\n\r"
popid_2135934392X="<p>A <span style=\"font-weight: bold;\">callback function<\/span> is a user-defined function (a function written by you) that is called by the DLL. &nbsp;This allows the DLL to call your own function to perform some user-defined task(s). &nbsp;Your application never calls your own callback function, this is done by the DLL. &nbsp;Examples of functions that require callback functions are the Windows API <span style=\"font-weight: bold;\">EnumWindows <\/span>and <span style=\"font-weight: bold;\">EnumPrinters<\/span> functions.<\/p>\n\r"
popid_1673403318X="<p>This is a Windows API structure. &nbsp;Consult your Windows API reference for more information on this structure.<\/p>\n\r"
popid_159360544="<p>The Windows API Clipboard functions include OpenClipboard, SetClipboardData, GetClipboardData, and CloseClipboard just to name a few. &nbsp;Consult the Windows API reference for more information on these functions.<\/p>\n\r"
popid_228338154X="<p>A <span style=\"font-weight: bold;\">DTWAIN_ARRAY<\/span> is a 32-bit value denoting a handle to a <a href=\"overview_of_dtwain_arrays.htm\">special array type<\/a> defined by DTWAIN<\/p>\n\r"
popid_2454855="<p>The <span style=\"font-weight: bold;\">NULL<\/span> value is equal to 0.<\/p>\n\r"
popid_2926730="<p>The <span style=\"font-weight: bold;\">TRUE<\/span> value is equal to <span style=\"font-weight: bold;\">1<\/span>.<\/p>\n\r"
