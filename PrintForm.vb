Public Class PrintForm
    Private printDocument As New Printing.PrintDocument()
    Private printData As List(Of String)

    Public Sub New(coinName As String, hashRate As String, power As String,
                 elecCost As String, hardwareCost As String,
                 dailyProfit As String, monthlyProfit As String)
        InitializeComponent()

        ' Store data to print
        printData = New List(Of String) From {
            "Cryptocurrency Mining Profit Report",
            "Date: " & DateTime.Now.ToString("yyyy-MM-dd"),
            "",
            "CALCULATION PARAMETERS",
            "Coin: " & coinName,
            "Hash Rate: " & hashRate & " MH/s",
            "Power Consumption: " & power & " W",
            "Electricity Cost: $" & elecCost & " per kWh",
            "Hardware Cost: " & hardwareCost,
            "",
            "PROFIT RESULTS",
            "Daily Profit: " & dailyProfit,
            "Monthly Profit: " & monthlyProfit
        }

        AddHandler printDocument.PrintPage, AddressOf PrintPageHandler
    End Sub

    Private Sub PrintPageHandler(sender As Object, e As Printing.PrintPageEventArgs)
        Dim fontTitle As New Font("Arial", 16, FontStyle.Bold)
        Dim fontHeader As New Font("Arial", 12, FontStyle.Bold)
        Dim fontNormal As New Font("Arial", 10)

        Dim yPos As Integer = 50
        Dim leftMargin As Integer = 50

        For Each line In printData
            If line.StartsWith("Cryptocurrency") Then
                e.Graphics.DrawString(line, fontTitle, Brushes.Black, leftMargin, yPos)
                yPos += 40
            ElseIf line.StartsWith("CALCULATION") Or line.StartsWith("PROFIT") Then
                e.Graphics.DrawString(line, fontHeader, Brushes.Black, leftMargin, yPos)
                yPos += 25
            Else
                e.Graphics.DrawString(line, fontNormal, Brushes.Black, leftMargin, yPos)
                yPos += 20
            End If
        Next
    End Sub

    Private Sub PrintForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim printPreview As New PrintPreviewDialog()
        printPreview.Document = printDocument
        printPreview.ShowDialog()
        Me.Close()
    End Sub
End Class