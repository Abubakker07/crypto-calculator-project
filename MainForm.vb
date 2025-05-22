Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports ZstdSharp.Unsafe
Imports System.Net.Mail
Imports Mysqlx.XDevAPI
Imports System.Runtime.Remoting.Messaging
Imports System.Buffers
Imports Org.BouncyCastle.Crypto.Utilities
Imports System.Net.Security
Imports Org.BouncyCastle.Tls

Public Class MainForm
    Inherits Form

    Private ReadOnly userId As Integer
    Private ReadOnly calculatorService As New MiningCalculatorService()

    ' Controls
    Private WithEvents cmbCoin As New ComboBox()
    Private WithEvents txtHardwareCost As New TextBox()
    Private WithEvents txtHashRate As New TextBox()
    Private WithEvents txtPowerConsumption As New TextBox()
    Private WithEvents txtElectricityCost As New TextBox()
    Private WithEvents lblDailyProfit As New Label()
    Private WithEvents lblMonthlyProfit As New Label()
    Private WithEvents btnCalculate As New Button()
    Private WithEvents btnHistory As New Button()
    Private WithEvents btnPrint As New Button()
    Private WithEvents btnLogout As New Button()

    Public Sub New(userId As Integer)
        Me.userId = userId
        InitializeForm()
        LoadCryptocurrencies()
    End Sub

    Private Sub InitializeForm()
        ' Main Form
        Me.Text = "Crypto Mining Profit Calculator"
        Me.Size = New Size(900, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.BackColor = Color.FromArgb(10, 10, 20)
        Me.DoubleBuffered = True

        ' Background Image
        Me.BackgroundImage = Image.FromFile("C:\Users\ABU BAKKER\source\repos\cryptocalc_project\Resources\6256878.jpg")
        Me.BackgroundImageLayout = ImageLayout.Stretch

        ' Center Panel
        Dim panel As New Panel With {
            .Size = New Size(400, 520),
            .Location = New Point((Me.ClientSize.Width - 400) \ 2, (Me.ClientSize.Height - 520) \ 2),
            .BackColor = Color.FromArgb(180, 20, 30, 50)
        }
        Me.Controls.Add(panel)

        ' Header Label
        Dim lblHeader As New Label With {
            .Text = "Cryptocurrency Mining Calculator",
            .ForeColor = Color.Cyan,
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .AutoSize = True,
            .Location = New Point((panel.Width - 300) \ 2, 20)
        }
        panel.Controls.Add(lblHeader)

        ' Input Fields
        Dim yOffset As Integer = 60
        Dim spacing As Integer = 50

        CreateInput(panel, "Coin", cmbCoin, yOffset)
        CreateInput(panel, "Hardware Costs", txtHardwareCost, yOffset + spacing)
        CreateInput(panel, "Hashrate", txtHashRate, yOffset + spacing * 2)
        CreateInput(panel, "Power Consumption", txtPowerConsumption, yOffset + spacing * 3)
        CreateInput(panel, "Electricity Cost", txtElectricityCost, yOffset + spacing * 4)

        ' Output Labels
        CreateOutput(panel, "Daily Profit", lblDailyProfit, yOffset + spacing * 5)
        CreateOutput(panel, "Monthly Profit", lblMonthlyProfit, yOffset + spacing * 6)

        ' Buttons
        Dim btnY As Integer = yOffset + spacing * 7 + 10
        Dim btnWidth = 150
        Dim btnHeight = 35
        Dim gap = 20

        CreateButton(panel, btnCalculate, "CALCULATE", 30, btnY, btnWidth, btnHeight)
        CreateButton(panel, btnHistory, "VIEW HISTORY", 220, btnY, btnWidth, btnHeight)
        CreateButton(panel, btnPrint, "PRINT", 30, btnY + btnHeight + gap, btnWidth, btnHeight)
        CreateButton(panel, btnLogout, "LOGOUT", 220, btnY + btnHeight + gap, btnWidth, btnHeight)
    End Sub

    Private Sub CreateInput(panel As Panel, labelText As String, input As Control, y As Integer)
        Dim lbl As New Label With {
            .Text = labelText,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10),
            .Location = New Point(30, y),
            .AutoSize = True
        }
        panel.Controls.Add(lbl)

        input.Font = New Font("Segoe UI", 10)
        input.BackColor = Color.FromArgb(40, 40, 60)
        input.ForeColor = Color.White
        input.Size = New Size(320, 25)
        input.Location = New Point(30, y + 20)
        panel.Controls.Add(input)
    End Sub

    Private Sub CreateOutput(panel As Panel, labelText As String, outputLabel As Label, y As Integer)
        Dim lbl As New Label With {
            .Text = labelText,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .Location = New Point(30, y),
            .AutoSize = True
        }
        panel.Controls.Add(lbl)

        outputLabel.Text = "$0.00"
        outputLabel.ForeColor = Color.Lime
        outputLabel.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        outputLabel.Location = New Point(180, y)
        outputLabel.AutoSize = True
        panel.Controls.Add(outputLabel)
    End Sub

    Private Sub CreateButton(panel As Panel, btn As Button, text As String, x As Integer, y As Integer, width As Integer, height As Integer)
        btn.Text = text
        btn.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        btn.Size = New Size(width, height)
        btn.Location = New Point(x, y)
        btn.BackColor = Color.FromArgb(70, 70, 100)
        btn.ForeColor = Color.White
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderColor = Color.Cyan
        btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(90, 90, 120)
        panel.Controls.Add(btn)
    End Sub

    Private Sub LoadCryptocurrencies()
        Try
            Dim coins As DataTable = calculatorService.GetCryptocurrencies()
            If coins IsNot Nothing AndAlso coins.Rows.Count > 0 Then
                cmbCoin.DataSource = coins
                cmbCoin.DisplayMember = "coin_name"
                cmbCoin.ValueMember = "coin_id"
            Else
                cmbCoin.Items.AddRange({"Bitcoin", "Ethereum", "Litecoin", "Dogecoin"})
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading coins: " & ex.Message)
        End Try
    End Sub

    ' Button Event Handlers
    Private Sub BtnCalculate_Click(sender As Object, e As EventArgs) Handles btnCalculate.Click
        Try
            ' Step 1: Get selected coin from ComboBox
            Dim selectedRow As DataRowView = CType(cmbCoin.SelectedItem, DataRowView)
            Dim coinId As Integer = Convert.ToInt32(selectedRow("coin_id"))
            Dim coingeckoId As String = selectedRow("coingecko_id").ToString()

            ' Step 2: Get coin price from CoinGecko
            Dim coinPrice As Decimal = CoinGeckoAPI.GetCoinPrice(coingeckoId, "usd")

            ' Step 3: Collect user inputs safely
            Dim hardwareCost, hashrate, power, electricityCost As Decimal

            If Not Decimal.TryParse(txtHardwareCost.Text, hardwareCost) Then
                MessageBox.Show("Please enter a valid Hardware Cost")
                Exit Sub
            End If
            If Not Decimal.TryParse(txtHashRate.Text, hashrate) Then
                MessageBox.Show("Please enter a valid Hashrate")
                Exit Sub
            End If
            If Not Decimal.TryParse(txtPowerConsumption.Text, power) Then
                MessageBox.Show("Please enter a valid Power Consumption")
                Exit Sub
            End If
            If Not Decimal.TryParse(txtElectricityCost.Text, electricityCost) Then
                MessageBox.Show("Please enter a valid Electricity Cost")
                Exit Sub
            End If

            ' Step 4: Calculate profits
            Dim dailyRevenue As Decimal = (hashrate * coinPrice) / 1000D
            Dim dailyPowerCost As Decimal = ((power * 24) / 1000D) * electricityCost
            Dim dailyProfit As Decimal = dailyRevenue - dailyPowerCost
            Dim monthlyProfit As Decimal = (dailyProfit * 30) - hardwareCost

            ' Step 5: Display profits
            lblDailyProfit.Text = "$" & dailyProfit.ToString("F2")
            lblMonthlyProfit.Text = "$" & monthlyProfit.ToString("F2")

            ' Step 6: Save to database with currency = "USD"
            Dim saved As Boolean = calculatorService.SaveCalculation(
            userId,
            coinId,
            hashrate,
            power,
            electricityCost,
            hardwareCost,
            dailyProfit,
            monthlyProfit,
            "USD"
        )

            If saved Then
                MessageBox.Show("Calculation saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Failed to save calculation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MessageBox.Show("Calculation Error: " & ex.Message)
        End Try
    End Sub


    Private Sub btnHistory_Click(sender As Object, e As EventArgs) Handles btnHistory.Click
        Dim historyForm As New HistoryForm(userId)
        historyForm.ShowDialog()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If String.IsNullOrEmpty(lblDailyProfit.Text) OrElse lblDailyProfit.Text = "$0.00" Then
            MessageBox.Show("Please perform a calculation first")
            Return
        End If

        Dim printForm As New PrintForm(
            cmbCoin.Text,
            txtHashRate.Text,
            txtPowerConsumption.Text,
            txtElectricityCost.Text,
            txtHardwareCost.Text,
            lblDailyProfit.Text,
            lblMonthlyProfit.Text
        )
        printForm.ShowDialog()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Dim loginForm As New LoginForm()
        loginForm.Show()
        Me.Close()
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'MainForm
        '
        Me.ClientSize = New System.Drawing.Size(282, 253)
        Me.Name = "MainForm"
        Me.ResumeLayout(False)

    End Sub
End Class
