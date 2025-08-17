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
procedure TForm1.RetrieveDib(Sender: TObject);

var
   SelectedSource: DTWAIN_SOURCE;
   ErrStatus: LONG;
   AcquisitionArray: DWORD;
   SourceName, SourceDetails: AnsiString;
   NumChars: LONG;
   RetCode : BOOL;
   PageCount : LONG;
begin
   { Check for Twain availability }
   if (DTWAIN_IsTwainAvailable <> 0) then
   begin
      { Initialize DTWAIN }
      if (DTWAIN_SysInitialize <> 0) then
      begin
           SelectedSource := DTWAIN_SelectSource2A(0, 'Select Source',0,0,DTWAIN_DLG_CENTER_SCREEN);
           if SelectedSource <> 0 then
           begin
              { Open the source }
              DTWAIN_OpenSource(SelectedSource);
              { Acquire a BMP image }

              RetCode := DTWAIN_AcquireFileA(
                          SelectedSource,    { the Source }
                          'test.bmp',        { File name to save }
                          DTWAIN_BMP,        { Image format }
                          DTWAIN_USELONGNAME, { File action constants }
                          DTWAIN_PT_DEFAULT, { default pixel type }
                          DTWAIN_ACQUIREALL, { get all documents }
                          1,              { show the UI }
                          1,              { close Source when done }
                          @ErrStatus);    { Error status }

              if (RetCode = 0) then
              begin
                  { didn't work }
                  Application.MessageBox('TWAIN Acquisition Failed!',
                                         'Twain Error', MB_ICONSTOP );
              end
              else
              begin
                  PageCount := DTWAIN_GetSavedFilesCount(SelectedSource);
                  if (PageCount = 0) then
                  begin
                    { user canceled or no page acquired }
                    Application.MessageBox('User canceled acquisition',
                                         'Twain Information', MB_ICONSTOP );
                  end
                  else
                  begin
                    { display image }
                    Image1.Picture.LoadFromFile('test.bmp');
                  end;
              end;
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
