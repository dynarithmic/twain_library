<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestCapDlg
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.label1 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.cmbGetTypes = New System.Windows.Forms.ComboBox()
        Me.cmbContainer = New System.Windows.Forms.ComboBox()
        Me.cmbDataType = New System.Windows.Forms.ComboBox()
        Me.btnStartTest = New System.Windows.Forms.Button()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.label5 = New System.Windows.Forms.Label()
        Me.lstResults = New System.Windows.Forms.ListBox()
        Me.button1 = New System.Windows.Forms.Button()
        Me.label6 = New System.Windows.Forms.Label()
        Me.lblSetOperation = New System.Windows.Forms.Label()
        Me.lblSetContainer = New System.Windows.Forms.Label()
        Me.lblSetDataType = New System.Windows.Forms.Label()
        Me.cmbSetTypes = New System.Windows.Forms.ComboBox()
        Me.cmbContainerSet = New System.Windows.Forms.ComboBox()
        Me.cmbDataTypeSet = New System.Windows.Forms.ComboBox()
        Me.btnTestSet = New System.Windows.Forms.Button()
        Me.btnSetRevert = New System.Windows.Forms.Button()
        Me.lblResultsSet = New System.Windows.Forms.Label()
        Me.lstResultsSet = New System.Windows.Forms.ListBox()
        Me.lblInput = New System.Windows.Forms.Label()
        Me.editInputData = New System.Windows.Forms.TextBox()
        Me.lblTestGetResults = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(90, 27)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(403, 13)
        Me.label1.TabIndex = 26
        Me.label1.Text = "Note: Testing using non-default container or data type may have undesirable resul" &
    "ts."
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(21, 51)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(76, 13)
        Me.label2.TabIndex = 27
        Me.label2.Text = "Get Operation:"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(161, 51)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(55, 13)
        Me.label3.TabIndex = 28
        Me.label3.Text = "Container:"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(322, 51)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(60, 13)
        Me.label4.TabIndex = 29
        Me.label4.Text = "Data Type:"
        '
        'cmbGetTypes
        '
        Me.cmbGetTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGetTypes.FormattingEnabled = True
        Me.cmbGetTypes.Location = New System.Drawing.Point(21, 68)
        Me.cmbGetTypes.Name = "cmbGetTypes"
        Me.cmbGetTypes.Size = New System.Drawing.Size(134, 21)
        Me.cmbGetTypes.TabIndex = 30
        '
        'cmbContainer
        '
        Me.cmbContainer.FormattingEnabled = True
        Me.cmbContainer.Location = New System.Drawing.Point(161, 68)
        Me.cmbContainer.Name = "cmbContainer"
        Me.cmbContainer.Size = New System.Drawing.Size(145, 21)
        Me.cmbContainer.TabIndex = 31
        '
        'cmbDataType
        '
        Me.cmbDataType.FormattingEnabled = True
        Me.cmbDataType.Location = New System.Drawing.Point(322, 68)
        Me.cmbDataType.Name = "cmbDataType"
        Me.cmbDataType.Size = New System.Drawing.Size(121, 21)
        Me.cmbDataType.TabIndex = 32
        '
        'btnStartTest
        '
        Me.btnStartTest.Location = New System.Drawing.Point(244, 101)
        Me.btnStartTest.Name = "btnStartTest"
        Me.btnStartTest.Size = New System.Drawing.Size(75, 23)
        Me.btnStartTest.TabIndex = 33
        Me.btnStartTest.Text = "Test"
        Me.btnStartTest.UseVisualStyleBackColor = True
        '
        'btnReset
        '
        Me.btnReset.Location = New System.Drawing.Point(455, 68)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(75, 23)
        Me.btnReset.TabIndex = 34
        Me.btnReset.Text = "Revert"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(26, 120)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(45, 13)
        Me.label5.TabIndex = 35
        Me.label5.Text = "Results:"
        '
        'lstResults
        '
        Me.lstResults.FormattingEnabled = True
        Me.lstResults.Location = New System.Drawing.Point(21, 141)
        Me.lstResults.Name = "lstResults"
        Me.lstResults.Size = New System.Drawing.Size(509, 95)
        Me.lstResults.TabIndex = 36
        '
        'button1
        '
        Me.button1.AllowDrop = True
        Me.button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.button1.Location = New System.Drawing.Point(244, 460)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(75, 23)
        Me.button1.TabIndex = 37
        Me.button1.Text = "OK"
        Me.button1.UseVisualStyleBackColor = True
        '
        'label6
        '
        Me.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.label6.Location = New System.Drawing.Point(18, 254)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(526, 2)
        Me.label6.TabIndex = 38
        '
        'lblSetOperation
        '
        Me.lblSetOperation.AutoSize = True
        Me.lblSetOperation.Location = New System.Drawing.Point(21, 276)
        Me.lblSetOperation.Name = "lblSetOperation"
        Me.lblSetOperation.Size = New System.Drawing.Size(75, 13)
        Me.lblSetOperation.TabIndex = 39
        Me.lblSetOperation.Text = "Set Operation:"
        '
        'lblSetContainer
        '
        Me.lblSetContainer.AutoSize = True
        Me.lblSetContainer.Location = New System.Drawing.Point(172, 276)
        Me.lblSetContainer.Name = "lblSetContainer"
        Me.lblSetContainer.Size = New System.Drawing.Size(55, 13)
        Me.lblSetContainer.TabIndex = 40
        Me.lblSetContainer.Text = "Container:"
        '
        'lblSetDataType
        '
        Me.lblSetDataType.AutoSize = True
        Me.lblSetDataType.Location = New System.Drawing.Point(322, 276)
        Me.lblSetDataType.Name = "lblSetDataType"
        Me.lblSetDataType.Size = New System.Drawing.Size(60, 13)
        Me.lblSetDataType.TabIndex = 41
        Me.lblSetDataType.Text = "Data Type:"
        '
        'cmbSetTypes
        '
        Me.cmbSetTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSetTypes.FormattingEnabled = True
        Me.cmbSetTypes.Location = New System.Drawing.Point(21, 293)
        Me.cmbSetTypes.Name = "cmbSetTypes"
        Me.cmbSetTypes.Size = New System.Drawing.Size(148, 21)
        Me.cmbSetTypes.TabIndex = 42
        '
        'cmbContainerSet
        '
        Me.cmbContainerSet.FormattingEnabled = True
        Me.cmbContainerSet.Location = New System.Drawing.Point(172, 293)
        Me.cmbContainerSet.Name = "cmbContainerSet"
        Me.cmbContainerSet.Size = New System.Drawing.Size(145, 21)
        Me.cmbContainerSet.TabIndex = 43
        '
        'cmbDataTypeSet
        '
        Me.cmbDataTypeSet.FormattingEnabled = True
        Me.cmbDataTypeSet.Location = New System.Drawing.Point(322, 293)
        Me.cmbDataTypeSet.Name = "cmbDataTypeSet"
        Me.cmbDataTypeSet.Size = New System.Drawing.Size(121, 21)
        Me.cmbDataTypeSet.TabIndex = 44
        '
        'btnTestSet
        '
        Me.btnTestSet.Location = New System.Drawing.Point(244, 326)
        Me.btnTestSet.Name = "btnTestSet"
        Me.btnTestSet.Size = New System.Drawing.Size(75, 23)
        Me.btnTestSet.TabIndex = 45
        Me.btnTestSet.Text = "Test"
        Me.btnTestSet.UseVisualStyleBackColor = True
        '
        'btnSetRevert
        '
        Me.btnSetRevert.Location = New System.Drawing.Point(455, 293)
        Me.btnSetRevert.Name = "btnSetRevert"
        Me.btnSetRevert.Size = New System.Drawing.Size(75, 23)
        Me.btnSetRevert.TabIndex = 46
        Me.btnSetRevert.Text = "Revert"
        Me.btnSetRevert.UseVisualStyleBackColor = True
        '
        'lblResultsSet
        '
        Me.lblResultsSet.AutoSize = True
        Me.lblResultsSet.Location = New System.Drawing.Point(316, 350)
        Me.lblResultsSet.Name = "lblResultsSet"
        Me.lblResultsSet.Size = New System.Drawing.Size(45, 13)
        Me.lblResultsSet.TabIndex = 47
        Me.lblResultsSet.Text = "Results:"
        '
        'lstResultsSet
        '
        Me.lstResultsSet.FormattingEnabled = True
        Me.lstResultsSet.HorizontalExtent = 500
        Me.lstResultsSet.HorizontalScrollbar = True
        Me.lstResultsSet.Location = New System.Drawing.Point(317, 366)
        Me.lstResultsSet.Name = "lstResultsSet"
        Me.lstResultsSet.Size = New System.Drawing.Size(213, 82)
        Me.lstResultsSet.TabIndex = 48
        '
        'lblInput
        '
        Me.lblInput.AutoSize = True
        Me.lblInput.Location = New System.Drawing.Point(26, 350)
        Me.lblInput.Name = "lblInput"
        Me.lblInput.Size = New System.Drawing.Size(146, 13)
        Me.lblInput.TabIndex = 49
        Me.lblInput.Text = "Input (One data item per line):"
        '
        'editInputData
        '
        Me.editInputData.Location = New System.Drawing.Point(21, 366)
        Me.editInputData.Multiline = True
        Me.editInputData.Name = "editInputData"
        Me.editInputData.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.editInputData.Size = New System.Drawing.Size(208, 84)
        Me.editInputData.TabIndex = 50
        '
        'lblTestGetResults
        '
        Me.lblTestGetResults.AutoSize = True
        Me.lblTestGetResults.Location = New System.Drawing.Point(322, 107)
        Me.lblTestGetResults.Name = "lblTestGetResults"
        Me.lblTestGetResults.Size = New System.Drawing.Size(0, 13)
        Me.lblTestGetResults.TabIndex = 51
        '
        'TestCapDlg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(556, 494)
        Me.Controls.Add(Me.lblTestGetResults)
        Me.Controls.Add(Me.editInputData)
        Me.Controls.Add(Me.lblInput)
        Me.Controls.Add(Me.lstResultsSet)
        Me.Controls.Add(Me.lblResultsSet)
        Me.Controls.Add(Me.btnSetRevert)
        Me.Controls.Add(Me.btnTestSet)
        Me.Controls.Add(Me.cmbDataTypeSet)
        Me.Controls.Add(Me.cmbContainerSet)
        Me.Controls.Add(Me.cmbSetTypes)
        Me.Controls.Add(Me.lblSetDataType)
        Me.Controls.Add(Me.lblSetContainer)
        Me.Controls.Add(Me.lblSetOperation)
        Me.Controls.Add(Me.label6)
        Me.Controls.Add(Me.button1)
        Me.Controls.Add(Me.lstResults)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.btnStartTest)
        Me.Controls.Add(Me.cmbDataType)
        Me.Controls.Add(Me.cmbContainer)
        Me.Controls.Add(Me.cmbGetTypes)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TestCapDlg"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "TestCapDlg"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents label1 As Windows.Forms.Label
    Private WithEvents label2 As Windows.Forms.Label
    Private WithEvents label3 As Windows.Forms.Label
    Private WithEvents label4 As Windows.Forms.Label
    Private WithEvents cmbGetTypes As Windows.Forms.ComboBox
    Private WithEvents cmbContainer As Windows.Forms.ComboBox
    Private WithEvents cmbDataType As Windows.Forms.ComboBox
    Private WithEvents btnStartTest As Windows.Forms.Button
    Private WithEvents btnReset As Windows.Forms.Button
    Private WithEvents label5 As Windows.Forms.Label
    Private WithEvents lstResults As Windows.Forms.ListBox
    Private WithEvents button1 As Windows.Forms.Button
    Private WithEvents label6 As Windows.Forms.Label
    Private WithEvents lblSetOperation As Windows.Forms.Label
    Private WithEvents lblSetContainer As Windows.Forms.Label
    Private WithEvents lblSetDataType As Windows.Forms.Label
    Private WithEvents cmbSetTypes As Windows.Forms.ComboBox
    Private WithEvents cmbContainerSet As Windows.Forms.ComboBox
    Private WithEvents cmbDataTypeSet As Windows.Forms.ComboBox
    Private WithEvents btnTestSet As Windows.Forms.Button
    Private WithEvents btnSetRevert As Windows.Forms.Button
    Private WithEvents lblResultsSet As Windows.Forms.Label
    Private WithEvents lstResultsSet As Windows.Forms.ListBox
    Private WithEvents lblInput As Windows.Forms.Label
    Private WithEvents editInputData As Windows.Forms.TextBox
    Private WithEvents lblTestGetResults As Windows.Forms.Label
End Class
