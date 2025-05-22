Public Class RegisterForm
    Private ReadOnly authService As New AuthService()

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Dim username As String = txtUsername.Text.Trim()
        Dim email As String = txtEmail.Text.Trim()
        Dim password As String = txtPassword.Text
        Dim confirmPassword As String = txtConfirmPassword.Text

        ' Validation
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(email) OrElse
           String.IsNullOrEmpty(password) OrElse String.IsNullOrEmpty(confirmPassword) Then
            MessageBox.Show("Please fill in all fields")
            Return
        End If

        If password <> confirmPassword Then
            MessageBox.Show("Passwords do not match")
            Return
        End If

        If authService.UsernameExists(username) Then
            MessageBox.Show("Username already exists")
            Return
        End If

        If authService.EmailExists(email) Then
            MessageBox.Show("Email already exists")
            Return
        End If

        ' Registration
        If authService.Register(username, email, password) Then
            MessageBox.Show("Registration successful! You can now login.")
            Me.Close()
        Else
            MessageBox.Show("Registration failed. Please try again.")
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class