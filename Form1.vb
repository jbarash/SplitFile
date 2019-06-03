Imports System.IO

Public Class Form1
    Dim txtFileName As String = ""
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Start.Click
        Dim line As String, cnt As Integer, fileno As Integer, header As String = String.Empty
        Dim destStream As StreamWriter
        Try
            cnt = 0
            fileno = 1
            If txtFileName.Length < 1 Then
                MessageBox.Show("You must Select a file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If Not IsNumeric(txtRowCount.Text) OrElse CInt(txtRowCount.Text) < 1 Then
                MessageBox.Show("You must enter a Row Count", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            Dim txtFilePre = Path.GetFileNameWithoutExtension(txtFileName)
            Dim txtExt = Path.GetExtension(txtFileName)
            Dim txtFilePath = Path.GetDirectoryName(txtFileName)
            destStream = New StreamWriter(txtFilePath & "\" & txtFilePre & "-" & fileno.ToString() & txtExt)
            lblOutputFile.Text = txtFilePre & "-" & fileno.ToString() & txtExt
            Using sourceStream As StreamReader = New StreamReader(txtFileName)
                If CheckBox1.Checked Then
                    header = sourceStream.ReadLine
                    destStream.WriteLine(header)
                    cnt += 1
                End If
                While sourceStream.Peek() >= 0
                    line = sourceStream.ReadLine
                    If cnt >= CInt(txtRowCount.Text) Then
                        destStream.Close()
                        destStream = Nothing
                        fileno += 1
                        destStream = New StreamWriter(txtFilePath & "\" & txtFilePre & "-" & fileno.ToString() & txtExt)
                        lblOutputFile.Text = txtFilePre & "-" & fileno.ToString() & txtExt
                        lblOutputFile.Refresh()
                        If CheckBox1.Checked Then 'header
                            destStream.WriteLine(header)
                            cnt = 1
                        Else
                            cnt = 0 ' no header
                        End If

                    End If
                    destStream.WriteLine(line)
                    cnt += 1
                    If cnt Mod 1000 = 0 Then
                        lblRowCount.Text = cnt.ToString()
                        lblRowCount.Refresh()
                    End If
                End While
            End Using
            destStream.Close()
            destStream = Nothing
            MessageBox.Show("Done")
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

    End Sub

    Private Sub btnSelectFile_Click(sender As Object, e As EventArgs) Handles btnSelectFile.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.Title = "Select File to Split"
        fd.InitialDirectory = "C:\"
        fd.Filter = "Text file|*.txt;*.csv|All files (*.*)|*.*"
        fd.FilterIndex = 1
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            txtFileName = fd.FileName
            lblFileName.Text = txtFileName
        End If
    End Sub


End Class
