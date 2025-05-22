Imports MySql.Data.MySqlClient

Public Class AuthService
    Private ReadOnly db As New DatabaseHelper()

    Public Function Login(username As String, password As String) As Integer
        Dim query As String = "SELECT user_id FROM users WHERE username = @username AND password = @password"
        Dim parameters As New List(Of MySqlParameter) From {
            New MySqlParameter("@username", username),
            New MySqlParameter("@password", password)
        }

        Dim result As DataTable = db.ExecuteQuery(query, parameters)

        If result.Rows.Count > 0 Then
            Return Convert.ToInt32(result.Rows(0)("user_id"))
        Else
            Return -1
        End If
    End Function

    Public Function Register(username As String, email As String, password As String) As Boolean
        Dim query As String = "INSERT INTO users (username, email, password) VALUES (@username, @email, @password)"
        Dim parameters As New List(Of MySqlParameter) From {
            New MySqlParameter("@username", username),
            New MySqlParameter("@email", email),
            New MySqlParameter("@password", password)
        }

        Return db.ExecuteNonQuery(query, parameters) > 0
    End Function

    Public Function UsernameExists(username As String) As Boolean
        Dim query As String = "SELECT 1 FROM users WHERE username = @username"
        Dim parameters As New List(Of MySqlParameter) From {
            New MySqlParameter("@username", username)
        }

        Dim result As DataTable = db.ExecuteQuery(query, parameters)
        Return result.Rows.Count > 0
    End Function

    Public Function EmailExists(email As String) As Boolean
        Dim query As String = "SELECT 1 FROM users WHERE email = @email"
        Dim parameters As New List(Of MySqlParameter) From {
            New MySqlParameter("@email", email)
        }

        Dim result As DataTable = db.ExecuteQuery(query, parameters)
        Return result.Rows.Count > 0
    End Function
End Class