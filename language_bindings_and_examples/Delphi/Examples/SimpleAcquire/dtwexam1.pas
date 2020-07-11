unit dtwexam1;
{ Illustrates a simple Native mode acquisition using the DTWAIN library.
   The application will allow DTWAIN to control the message loop }
interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  Menus, DTWAIN32U, ExtCtrls, Clipbrd;

type
  TForm1 = class(TForm)
    MainMenu1: TMainMenu;
    Image1: TImage;
    procedure RetrieveDib(Sender: TObject);
    procedure RetrieveAllDibs(AcquireArray:DWORD);
    procedure ExitApp(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.DFM}

procedure TForm1.RetrieveAllDibs(AcquireArray:DWORD);
var
    Count:Integer;
    DibCount:Integer;
    Count2:Integer;
    hDib: DWORD;
    numAcquisitions:Integer;

begin
    { Get the number of total acquisitions attempted }
    numAcquisitions := DTWAIN_ArrayGetCount(AcquireArray);

    { Loop for each acquisition attempted and free the DIBS
     Alternately, the DTWAIN_GetAcquiredImageArray function
    could have been used to get the array of DIBs }
    for Count := 0 to numAcquisitions-1 do
    begin
        DibCount := DTWAIN_GetNumAcquiredImages( AcquireArray, Count);
        for Count2 := 0 to DibCount - 1 do
        begin
        { Retrieve the DIB }
            hDib := DTWAIN_GetAcquiredImage(AcquireArray, Count, Count2);
            if ( hDib <> 0) then
            begin
                 { Copy DIB to clipboard }
                 OpenClipboard(0);
                 EmptyClipboard;
                 SetClipboardData(CF_DIB, hDib);
                 CloseClipboard;

                 { Now retrieve dib from clipboard and assign to picture object }
                 Image1.Picture.Assign(Clipboard);
                 Application.MessageBox('Image!',
                                         'Twain Message', MB_OK );
                 { Delete the DIB (must be done by application!!) }
                 GlobalFree(hDib);
            end;
         end;
    end;
end;

procedure TForm1.RetrieveDib(Sender: TObject);

var
   SelectedSource: DWORD;
   ErrStatus: LONG;
   AcquisitionArray: DWORD;
begin
   { Check for Twain availability }
   if (DTWAIN_IsTwainAvailable) then
   begin
      { Initialize DTWAIN }
      if (DTWAIN_SysInitialize <> 0) then
      begin
           SelectedSource := DTWAIN_SelectSource;
           if SelectedSource <> 0 then
           begin
              { Open the source }
              DTWAIN_OpenSource(SelectedSource);
              { Acquire an image using Native transfer mode
                and using DTWAIN_MODAL processing }
              AcquisitionArray := DTWAIN_AcquireNative(
                                    SelectedSource,    { the Source }
                                    DTWAIN_PT_DEFAULT, { default pixel type }
                                    DTWAIN_ACQUIREALL, { get all documents }
                                    TRUE,              { show the UI }
                                    TRUE,              { close Source when done }
                                    @ErrStatus);        { Error status }

              if (AcquisitionArray = 0) then
              begin
                  { didn't work }
                  Application.MessageBox('TWAIN Acquisition Failed!',
                                         'Twain Error', MB_ICONSTOP );
              end
              else
                  { display and delete Dibs }
                  RetrieveAllDibs(AcquisitionArray)
           end;
           { close all sources, sessions, and DTWAIN itself }
           DTWAIN_SysDestroy
      end
   end
end;

procedure TForm1.ExitApp(Sender: TObject);
begin
     Application.Terminate;
end;

end.
