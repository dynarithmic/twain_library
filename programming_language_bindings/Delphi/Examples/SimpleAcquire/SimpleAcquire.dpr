program SimpleAcquire;

uses
  Forms,
  dtwexam1 in 'dtwexam1.pas' {Form1},
  dtwain32u in 'dtwain32u.pas';

{$R *.RES}

begin
  Application.Initialize;
  Application.CreateForm(TForm1, Form1);
  Application.Run;
end.
