Imports System.Data
Imports System.Windows.Forms

Public Class HistoryForm
    Private ReadOnly userId As Integer
    Private ReadOnly calculatorService As New MiningCalculatorService()

    Public Sub New(userId As Integer)
        InitializeComponent()
        Me.userId = userId
        LoadHistory()
    End Sub

    Private Sub LoadHistory()
        Dim history As DataTable = calculatorService.GetCalculationHistory(userId)
        dgvHistory.DataSource = Nothing
        dgvHistory.Columns.Clear()
        dgvHistory.DataSource = history

        ' Format columns AFTER data is loaded
        If dgvHistory.Columns.Count > 0 Then
            dgvHistory.Columns("coin_name").HeaderText = "Coin"
            dgvHistory.Columns("hash_rate").HeaderText = "Hash Rate (MH/s)"
            dgvHistory.Columns("power_consumption").HeaderText = "Power (W)"
            dgvHistory.Columns("electricity_cost").HeaderText = "Elec. Cost ($)"
            dgvHistory.Columns("hardware_cost").HeaderText = "Hardware Cost ($)"
            dgvHistory.Columns("daily_profit").HeaderText = "Daily Profit"
            dgvHistory.Columns("monthly_profit").HeaderText = "Monthly Profit"
            dgvHistory.Columns("calculation_date").HeaderText = "Date"
            dgvHistory.Columns("currency").HeaderText = "Currency"

            ' Optional format
            dgvHistory.Columns("electricity_cost").DefaultCellStyle.Format = "c4"
            dgvHistory.Columns("hardware_cost").DefaultCellStyle.Format = "c2"
            dgvHistory.Columns("daily_profit").DefaultCellStyle.Format = "c2"
            dgvHistory.Columns("monthly_profit").DefaultCellStyle.Format = "c2"
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
