Imports MySql.Data.MySqlClient

Public Class MiningCalculatorService
    Private ReadOnly db As New DatabaseHelper()

    Public Function GetCryptocurrencies() As DataTable
        Dim query As String = "SELECT coin_id, coin_name, coin_symbol, coingecko_id FROM cryptocurrencies"
        Return db.ExecuteQuery(query)
    End Function

    Public Function SaveCalculation(userId As Integer, coinName As String, hashRate As Decimal,
                                    power As Decimal, elecCost As Decimal, hwCost As Decimal,
                                    dailyProfit As Decimal, monthlyProfit As Decimal, calcDate As DateTime) As Boolean
        Try
            Using conn As New MySqlConnection("server=localhost;userid=root;password=tiger;database=crypto_mining_db")
                conn.Open()
                Dim query As String = "INSERT INTO calculation_history (user_id, coin_name, hash_rate, power_consumption, electricity_cost, hardware_cost, daily_profit, monthly_profit, calculation_date, currency) " &
                                      "VALUES (@userId, @coinName, @hashRate, @power, @elecCost, @hwCost, @daily, @monthly, @date, 'USD')"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@userId", userId)
                    cmd.Parameters.AddWithValue("@coinName", coinName)
                    cmd.Parameters.AddWithValue("@hashRate", hashRate)
                    cmd.Parameters.AddWithValue("@power", power)
                    cmd.Parameters.AddWithValue("@elecCost", elecCost)
                    cmd.Parameters.AddWithValue("@hwCost", hwCost)
                    cmd.Parameters.AddWithValue("@daily", dailyProfit)
                    cmd.Parameters.AddWithValue("@monthly", monthlyProfit)
                    cmd.Parameters.AddWithValue("@date", calcDate)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            ' Log the exception or handle the error
            Return False
        End Try
    End Function

    Public Function GetCalculationHistory(userId As Integer) As DataTable
        Dim query As String = "
        SELECT 
    c.coin_name,
    mc.hash_rate + 0.0 AS hash_rate,
    mc.power_consumption + 0.0 AS power_consumption,
    mc.electricity_cost + 0.0 AS electricity_cost,
    mc.hardware_cost + 0.0 AS hardware_cost,
    mc.daily_profit + 0.0 AS daily_profit,
    mc.monthly_profit + 0.0 AS monthly_profit,
    DATE_FORMAT(mc.calculation_date, '%Y-%m-%d %H:%i:%s') AS calculation_date,
    mc.currency
FROM 
    mining_calculations mc
JOIN 
    coins c ON mc.coin_id = c.coin_id
WHERE 
    mc.user_id = @user_id
ORDER BY 
    mc.calculation_date DESC
    "

        Dim parameters As New List(Of MySqlParameter) From {
        New MySqlParameter("@userId", userId)
    }

        Dim rawTable As DataTable = db.ExecuteQuery(query, parameters)

        ' Now build a clean final DataTable
        Dim resultTable As New DataTable()
        resultTable.Columns.Add("Coin", GetType(String))
        resultTable.Columns.Add("Hash Rate (MH/s)", GetType(Decimal))
        resultTable.Columns.Add("Power (W)", GetType(Decimal))
        resultTable.Columns.Add("Electricity Cost", GetType(Decimal))
        resultTable.Columns.Add("Hardware Cost", GetType(Decimal))
        resultTable.Columns.Add("Daily Profit", GetType(Decimal))
        resultTable.Columns.Add("Monthly Profit", GetType(Decimal))
        resultTable.Columns.Add("Date", GetType(DateTime))
        resultTable.Columns.Add("Currency", GetType(String))

        For Each row As DataRow In rawTable.Rows
            resultTable.Rows.Add(
            row("coin_name").ToString(),
            Convert.ToDecimal(row("hash_rate")),
            Convert.ToDecimal(row("power_consumption")),
            Convert.ToDecimal(row("electricity_cost")),
            Convert.ToDecimal(row("hardware_cost")),
            Convert.ToDecimal(row("daily_profit")),
            Convert.ToDecimal(row("monthly_profit")),
            Convert.ToDateTime(row("calculation_date")),
            row("currency").ToString()
        )
        Next

        Return resultTable
    End Function

    Public Function CalculateProfit(hashRate As Decimal, powerConsumption As Decimal,
                                    electricityCost As Decimal, hardwareCost As Decimal,
                                    coinPrice As Decimal, rewardPerBlock As Decimal,
                                    blockTimeSeconds As Decimal) As (Decimal, Decimal)

        ' Constants
        Dim secondsPerDay As Decimal = 86400D
        Dim networkHashRate As Decimal = 1000000D ' Example: 1 TH/s → adjust if needed

        ' Daily Earnings Formula:
        ' Daily Coins = (YourHashrate / NetworkHashrate) * (Seconds in a day / Block time) * Reward per block
        Dim dailyCoins As Decimal = (hashRate / networkHashRate) * (secondsPerDay / blockTimeSeconds) * rewardPerBlock
        Dim dailyRevenue As Decimal = dailyCoins * coinPrice
        Dim dailyCost As Decimal = (powerConsumption * 24 / 1000D) * electricityCost
        Dim dailyProfit As Decimal = dailyRevenue - dailyCost
        Dim monthlyProfit As Decimal = dailyProfit * 30D

        Return (dailyProfit, monthlyProfit)
    End Function
End Class