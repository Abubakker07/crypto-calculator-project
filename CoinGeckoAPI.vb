Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq

Public Class CoinGeckoAPI
    ' Function to get coin price from CoinGecko
    Public Shared Function GetCoinPrice(coinId As String, currency As String) As Decimal
        Try
            Dim url As String = $"https://api.coingecko.com/api/v3/simple/price?ids={coinId}&vs_currencies={currency}"
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim jsonResponse As String = reader.ReadToEnd()
                    Dim data As JObject = JObject.Parse(jsonResponse)

                    If data(coinId) IsNot Nothing AndAlso data(coinId)(currency) IsNot Nothing Then
                        Return Convert.ToDecimal(data(coinId)(currency))
                    Else
                        Throw New Exception("Coin price not found in API response.")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("API Error: " & ex.Message)
            Return 0D
        End Try
    End Function
End Class