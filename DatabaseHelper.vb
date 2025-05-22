Imports MySql.Data.MySqlClient

Public Class DatabaseHelper
    Private ReadOnly connectionString As String = "Server=localhost;Database=crypto_mining_db;Uid=root;Pwd=tiger;"

    Public Function GetConnection() As MySqlConnection
        Return New MySqlConnection(connectionString)
    End Function

    Public Function TestConnection() As Boolean
        Using conn As MySqlConnection = GetConnection()
            Try
                conn.Open()
                Return True
            Catch ex As Exception
                MessageBox.Show("Database connection failed: " & ex.Message)
                Return False
            End Try
        End Using
    End Function

    Public Function ExecuteQuery(query As String, Optional parameters As List(Of MySqlParameter) = Nothing) As DataTable
        Dim result As New DataTable()

        Using conn As MySqlConnection = GetConnection()
            Try
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    If parameters IsNot Nothing Then
                        For Each param In parameters
                            cmd.Parameters.Add(param)
                        Next
                    End If

                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(result)
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Query execution error: " & ex.Message)
            End Try
        End Using

        Return result
    End Function

    Public Function ExecuteNonQuery(query As String, parameters As List(Of MySqlParameter)) As Integer
        Using conn As MySqlConnection = GetConnection()
            Try
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    For Each param In parameters
                        cmd.Parameters.Add(param)
                    Next

                    Return cmd.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                MessageBox.Show("Non-query execution error: " & ex.Message)
                Return -1
            End Try
        End Using
    End Function
End Class